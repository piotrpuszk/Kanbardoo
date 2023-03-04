import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { DragulaService } from 'ng2-dragula';
import { delay, of, Subscription, take } from 'rxjs';
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
  @ViewChild('inviteModalRef') inviteModal!: ModalComponent;
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
  private dragAndDropMode = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private boardsService: BoardsService,
    private formBuilder: FormBuilder,
    private tablesService: TablesService,
    private boardControllerService: BoardControllerService,
    private dragula: DragulaService
  ) {
    this.sub.add(
      this.dragula.dragend().subscribe((e) => {
        of(1)
          .pipe(take(1), delay(100))
          .subscribe((e) => {
            for (let j = 0; j < this.board!.tables.length; j++) {
              const table = this.board!.tables[j];
              table.priority = j;
              for (let i = 0; i < table.tasks.length; i++) {
                const element = table.tasks[i];
                element.priority = i;
                element.tableID = table.id;
              }
            }
            this.boardsService
                .update(this.board!)
                .pipe(take(1))
                .subscribe((e) => {});
          });
      })
    );
  }

  ngOnInit() {
    this.boardControllerService.setBoardComponentActive();
    this.pending = true;
    this.activatedRoute.params.pipe(take(1)).subscribe((params) => {
      const id = params['id'];
      this.getBoard(id);
    });

    this.setupNewTableForm();

    this.subscribeToEvents();
  }

  private setupNewTableForm() {
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
        this.inviteModal.open();
      })
    );

    this.sub.add(
      this.boardControllerService.onTableDeleted$.subscribe((tableID) => {
        this.board!.tables = this.board?.tables.filter(e => e.id !== tableID)!;
      })
    );
  }

  public startAddingTable() {
    this.newTable.inProgress = true;
  }

  public stopAddingTable() {
    this.newTable.inProgress = false;
    this.setupNewTableForm();
  }

  public cancelAddingTable() {
    this.stopAddingTable();
    this.newTableStopPending();
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

  public startDragAndDropTables() {
    this.dragAndDropMode = true;
  }

  public stopDragAndDropTables() {
    this.dragAndDropMode = false;
  }

  public isDragAndDropTablesEnabled() {
    return this.dragAndDropMode;
  }

  private getBoard(id: number) {
    this.boardControllerService.boardLoading(true);
    this.boardsService
      .getBoard(id)
      .pipe(take(1))
      .subscribe((result) => {
        this.board = result.content;
        this.pending = false;
        this.stopAddingTable();
        this.newTableStopPending();
        this.boardControllerService.boardLoaded({ ...result.content });
        this.boardControllerService.boardLoading(false);
      });
  }

  private newTableStartPending() {
    this.newTable.pending = true;
  }

  private newTableStopPending() {
    this.newTable.pending = false;
  }
}
