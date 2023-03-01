import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subscription, take } from 'rxjs';
import { KanBoard } from '../_models/kan-board';
import { BoardControllerService } from '../_services/board-controller.service';
import { BoardsService } from '../_services/boards.service';
import { TablesService } from '../_services/tables.service';
import { ModalComponent } from '../_shared/modal/modal.component';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss'],
})
export class BoardComponent implements OnInit, OnDestroy {
  @ViewChild('boardSettings') boardSettings!: ModalComponent;
  private sub = new Subscription();

  public board?: KanBoard;
  public pending = false;
  public newTable: {
    inProgress: boolean;
    form: FormGroup | undefined;
    pending: boolean;
  } = {
    inProgress: false,
    form: undefined,
    pending: false,
  };

  constructor(
    private activatedRoute: ActivatedRoute,
    private boardsService: BoardsService,
    private formBuilder: FormBuilder,
    private tablesService: TablesService,
    private boardControllerService: BoardControllerService
  ) {}

  ngOnInit() {
    this.boardControllerService.setBoardComponentActive();
    this.pending = true;
    this.activatedRoute.params.pipe(take(1)).subscribe((params) => {
      const id = params['id'];
      this.getBoard(id);
    });

    this.newTable.form = this.formBuilder.group({
      name: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(256),
        ],
      ],
    });

    this.subscribeToEvents();
  }

  ngOnDestroy() {
    this.boardControllerService.setBoardComponentDisabled();
  }

  private subscribeToEvents() {
    this.sub.add(
      this.boardControllerService.onOpenBoardSettings$.subscribe((e) => {
        this.boardSettings.open();
      })
    );

    this.sub.add(
      this.boardControllerService.onOpenInvite$.subscribe((e) => {
        //to do
      })
    );
  }

  public startAddingTable() {
    this.newTable.inProgress = true;
  }

  public stopAddingTable() {
    this.newTable.inProgress = false;
  }

  public addTable() {
    this.newTable.form?.markAllAsTouched();

    if (!this.newTable.form?.valid) return;

    this.newTableStartPending();
    this.tablesService
      .add({
        name: this.newTable.form.controls['name'].value,
        boardID: this.board!.id,
      })
      .subscribe((e) => {
        this.getBoard(this.board!.id);
      });
  }

  private getBoard(id: number) {
    this.boardsService
      .getBoard(id)
      .pipe(take(1))
      .subscribe((result) => {
        this.board = result.content;
        this.pending = false;
        this.stopAddingTable();
        this.newTableStopPending();
      });
  }

  private newTableStartPending() {
    this.newTable.pending = true;
  }

  private newTableStopPending() {
    this.newTable.pending = false;
  }
}
