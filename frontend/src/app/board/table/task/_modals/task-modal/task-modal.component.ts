import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription, take } from 'rxjs';
import { DueDateValidator } from 'src/app/_forms/due-date-validator';
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
  selector: 'app-task-modal',
  templateUrl: './task-modal.component.html',
  styleUrls: ['./task-modal.component.scss']
})
export class TaskModalComponent implements OnInit, AfterViewInit {
  @ViewChild('taskModal') modal!: ModalComponent;
  @Input() public task!: KanTask;
  public title = 'Task';
  public form!: FormGroup;
  public successButtonName = 'Save changes';
  public successButtonNameNotPending = 'Save changes';
  public successButtonNamePending = 'Saving changes...';
  public cancelButtonName = 'Cancel';
  public taskStatuses: KanTaskStatus[] = [];
  public users: KanUser[] = [];
  public selected: any;

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
  }

  private initForm() {
    this.form = this.formBuilder.group({
      name: [
        this.task.name,
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(256),
        ],
      ],
      description: [this.task.description, [Validators.maxLength(1024)]],
      dueDate: [new Date(this.task.dueDate), [DueDateValidator.todayOrAfter]],
      status: [this.task.status.id],
      assignee: [this.task.assignee.userName],
    });
  }
}

