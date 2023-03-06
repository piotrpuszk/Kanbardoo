import {
  AfterViewInit,
  Component,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import {
  map,
  Observable,
  Observer,
  of,
  Subscription,
  switchMap,
  take,
  tap,
} from 'rxjs';
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
  styleUrls: ['./task-modal.component.scss'],
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
  public users$?: Observable<KanUser[]>;
  public selected: any;
  public search?: string;
  public isSearching = false;

  private usersSnapshot?: KanUser[] = [];
  private sub = new Subscription();

  constructor(
    private formBuilder: FormBuilder,
    private tasksService: TasksService,
    private usersService: UsersService,
    private tablesService: TablesService
  ) {}

  ngOnInit() {
    this.users$ = new Observable((observer: Observer<string | undefined>) => {
      observer.next(this.search);
    }).pipe(
      switchMap((query: string) => {
        if (!!query) {
          this.isSearching = true;
          return this.usersService.getUsers(query).pipe(map((e) => e.content));
        }
        this.isSearching = false;
        return of([]);
      }),
      tap((users) => {
        this.isSearching = false;
        this.usersSnapshot = users;
      })
    );
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
    this.tasksService
      .getTaskStatuses()
      .pipe(take(1))
      .subscribe((e) => {
        this.taskStatuses = e.content;
      });

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

    this.startPending();

    var assignee = this.task.assignee;
    if (!!this.usersSnapshot && this.usersSnapshot.length > 0) {
      assignee = this.usersSnapshot.find(
        (e) => e.userName === this.form.controls['assignee'].value
      )!;
    }

    const task = {
      id: this.task.id,
      name: this.form.controls['name'].value,
      description: this.form.controls['description'].value,
      dueDate: this.form.controls['dueDate'].value,
      status: this.taskStatuses.find(
        (e) => e.id === this.form.controls['status'].value
      )!,
      assignee: assignee,
      tableID: this.task.tableID,
      priority: this.task.priority,
    };

    this.tasksService.update(task).subscribe((e) => {
      this.task.name = task.name;
      this.task.description = task.description;
      this.task.dueDate = task.dueDate;
      this.task.status = task.status;
      this.task.assignee = task.assignee;
      this.stopPending();
      this.close();
    });
  }

  private initForm() {
    this.search = this.task.assignee.userName;
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

  private startPending() {
    this.successButtonName = this.successButtonNamePending;
    this.modal.startPending();
  }

  private stopPending() {
    this.successButtonName = this.successButtonNameNotPending;
    this.modal.stopPending();
  }
}
