import {
  AfterViewInit,
  Component,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription, take } from 'rxjs';
import { DueDateValidator } from 'src/app/_forms/due-date-validator';
import { KanBoard } from 'src/app/_models/kan-board';
import { KanTable } from 'src/app/_models/kan-table';
import { KanTask } from 'src/app/_models/kan-task';
import { KanTaskStatus } from 'src/app/_models/kan-task-status';
import { KanUser } from 'src/app/_models/kan-user';
import { NewKanTask } from 'src/app/_models/new-kan-task';
import { TablesService } from 'src/app/_services/tables.service';
import { TasksService } from 'src/app/_services/tasks.service';
import { UsersService } from 'src/app/_services/users.service';
import { ModalComponent } from 'src/app/_shared/modal/modal.component';

@Component({
  selector: 'app-new-task-modal',
  templateUrl: './new-task-modal.component.html',
  styleUrls: ['./new-task-modal.component.scss'],
})
export class NewTaskModalComponent implements OnInit, AfterViewInit {
  @ViewChild('newTaskModal') modal!: ModalComponent;
  @Input() public table!: KanTable;
  public title = 'New task';
  public form!: FormGroup;
  public successButtonName = 'Create';
  public successButtonNameNotPending = 'Create';
  public successButtonNamePending = 'Creating...';
  public cancelButtonName = 'Cancel';
  public taskStatuses: KanTaskStatus[] = [];
  public users: KanUser[] = [];
  public selected: any;

  private newTask!: NewKanTask;

  private sub = new Subscription();

  constructor(
    private formBuilder: FormBuilder,
    private tasksService: TasksService,
    private usersService: UsersService,
    private tablesService: TablesService
  ) {}

  ngOnInit() {
    this.tasksService
      .getTaskStatuses()
      .pipe(take(1))
      .subscribe((e) => {
        this.taskStatuses = e.content;
      });
    
    this.usersService.loggedUser$.pipe(take(1)).subscribe(user => {
      this.users.push({...user});
    });
  }

  ngAfterViewInit() {
    this.sub.add(
      this.modal.onSuccess$.subscribe((e) => {
        this.create();
      })
    );

    this.sub.add(
      this.modal.onCancel$.subscribe((e) => {
        this.close();
      })
    );
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  public open() {
    this.newTask = {
      name: undefined!,
      description: undefined!,
      dueDate: undefined!,
      status: undefined!,
      assignee: undefined!,
      tableID: undefined!,
    };
    this.successButtonName = this.successButtonNameNotPending;
    this.initForm();
    this.modal.open();
  }

  public close() {
    this.modal.close();
  }

  public isTouched(controlName: string) {
    return this.form.controls[controlName].touched;
  }

  public isDirty(controlName: string) {
    return this.form.controls[controlName].dirty;
  }

  public isValid(controlName: string) {
    return this.form.controls[controlName].valid;
  }

  public create() {
    this.form.markAllAsTouched();

    if (!this.form.valid) return;

    this.successButtonName = this.successButtonNamePending;
    this.modal.startPending();

    this.newTask.status = this.taskStatuses.find(
      (e) => e.name === this.form.controls['status'].value
    )!;
    this.newTask.assignee = this.users.find(e => e.userName === this.form.controls['assignee'].value)!;
    this.newTask.description = this.form.controls['description'].value;
    this.newTask.dueDate = this.form.controls['dueDate'].value;
    this.newTask.name = this.form.controls['name'].value;
    this.newTask.tableID = this.table.id;

    this.tasksService
      .add({ ...this.newTask })
      .pipe(take(1))
      .subscribe((e) => {
        this.tablesService.get(this.table.id).pipe(take(1)).subscribe(e => {
          this.table.tasks = e.content.tasks;
          this.close();
      });
      });
  }

  private initForm() {
    this.form = this.formBuilder.group({
      name: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(256),
        ],
      ],
      description: ['', [Validators.maxLength(1024)]],
      dueDate: [undefined, [DueDateValidator.todayOrAfter]],
      status: ['New'],
      assignee: [undefined],
    });
  }
}
