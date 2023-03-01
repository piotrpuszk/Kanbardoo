import {
  AfterViewInit,
  Component,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { map, Observable, Observer, of, Subscription, switchMap, take } from 'rxjs';
import { DueDateValidator } from 'src/app/_forms/due-date-validator';
import { KanBoard } from 'src/app/_models/kan-board';
import { KanTable } from 'src/app/_models/kan-table';
import { KanTaskStatus } from 'src/app/_models/kan-task-status';
import { KanUser } from 'src/app/_models/kan-user';
import { NewKanTask } from 'src/app/_models/new-kan-task';
import { InvitationsService } from 'src/app/_services/invitations.service';
import { TablesService } from 'src/app/_services/tables.service';
import { TasksService } from 'src/app/_services/tasks.service';
import { UsersService } from 'src/app/_services/users.service';
import { ModalComponent } from 'src/app/_shared/modal/modal.component';

@Component({
  selector: 'app-invite-modal',
  templateUrl: './invite-modal.component.html',
  styleUrls: ['./invite-modal.component.scss'],
})
export class InviteModalComponent implements OnInit, AfterViewInit {
  @ViewChild('inviteModal') modal!: ModalComponent;
  @Input() public board!: KanBoard;
  public title = '';
  public form!: FormGroup;
  public successButtonName = 'Invite';
  public successButtonNameNotPending = 'Invite';
  public successButtonNamePending = 'Inviting...';
  public cancelButtonName = 'Cancel';
  public users$?: Observable<KanUser[]>;
  public search?: string;

  private sub = new Subscription();

  constructor(
    private formBuilder: FormBuilder,
    private usersService: UsersService,
    private invitationsService: InvitationsService
  ) {}

  ngOnInit() {
    this.users$ = new Observable((observer: Observer<string | undefined>) => {
      observer.next(this.search);
    }).pipe(
      switchMap((query: string) => {
        if(query && query.length >= 3) {
          return this.usersService.getUsers(query).pipe(map(data => data && data.content || []))
        }
        return of([]);
      })
    );
  }

  ngAfterViewInit() {
    this.sub.add(
      this.modal.onSuccess$.subscribe((e) => {
        this.invite();
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
    this.title = 'Invite users to join ' + this.board.name;
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

  public invite() {
    this.form.markAllAsTouched();

    if (!this.form.valid) return;

    this.successButtonName = this.successButtonNamePending;
    this.modal.startPending();

    this.invitationsService
      .invite({
        boardID: this.board.id,
        userName: this.form.controls['user'].value,
      })
      .pipe(take(1))
      .subscribe((e) => {
        this.modal.startPending();
        this.close();
      });
  }

  private initForm() {
    this.form = this.formBuilder.group({
      user: [undefined],
    });
  }
}
