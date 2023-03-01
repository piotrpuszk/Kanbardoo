import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { KanBoard } from 'src/app/_models/kan-board';
import { BoardsService } from 'src/app/_services/boards.service';
import { ModalComponent } from 'src/app/_shared/modal/modal.component';

@Component({
  selector: 'app-board-settings-modal',
  templateUrl: './board-settings-modal.component.html',
  styleUrls: ['./board-settings-modal.component.scss']
})
export class BoardSettingsModalComponent implements AfterViewInit, OnDestroy {
  @ViewChild('boardSettingsModal') modal!: ModalComponent;
  @Input() public board!: KanBoard;
  public title = '';
  public form!: FormGroup;
  public successButtonName = 'Save changes';
  public successButtonNameNotPending = 'Save changes';
  public successButtonNamePending = 'Saving changes...';
  public cancelButtonName = 'Cancel';

  private sub = new Subscription();
  
  constructor(private formBuilder: FormBuilder, private boardsService: BoardsService) {}

  ngAfterViewInit() {
    this.sub.add(this.modal.onSuccess$.subscribe(e => {
      this.update();
    }));

    this.sub.add(this.modal.onCancel$.subscribe(e => {
      this.close();
    }));
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
  
  public open() {
    this.successButtonName = this.successButtonNameNotPending;
    this.title = this.board.name;
    this.initForm();
    this.modal.open();
  }

  public close() {
    this.modal.close();
  }

  public isBoardNameTouched() {
    return this.form.controls['boardName'].touched;
  }

  public isBoardNameDirty() {
    return this.form.controls['boardName'].dirty;
  }

  public isBoardNameValid() {
    return this.form.controls['boardName'].valid;
  }

  public update() {
    this.form.markAllAsTouched();

    if(!this.form.valid) return;

    this.successButtonName = this.successButtonNamePending;
    this.modal.startPending();
    this.board.name = this.form.controls['boardName'].value;
    this.boardsService.update(this.board).subscribe(() => {
        this.successButtonName = this.successButtonNameNotPending;
        this.close();
      });
  }

  private initForm() {
    this.form = this.formBuilder.group({
      boardName: [
        this.board.name,
        [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(256),
        ],
      ],
    });
  }
}
