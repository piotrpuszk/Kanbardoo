import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject, skip, Subject } from 'rxjs';
import { getDefaultBoard, KanBoard } from '../_models/kan-board';

@Injectable({
  providedIn: 'root'
})
export class BoardControllerService {
  private _isBoardComponentActive = false;
  private readonly onOpenBoardSettings = new EventEmitter();
  private readonly onOpenInvite = new EventEmitter();
  private readonly onAcceptInvitation = new EventEmitter();
  private readonly onBoardLoaded = new BehaviorSubject<KanBoard>(getDefaultBoard());
  private readonly onBoardLoading = new BehaviorSubject<boolean>(false);
  private readonly onTableDeleted = new BehaviorSubject<number>(0);

  public readonly onOpenBoardSettings$ = this.onOpenBoardSettings.asObservable();
  public readonly onOpenInvite$ = this.onOpenInvite.asObservable();
  public readonly onAcceptInvitation$ = this.onAcceptInvitation.asObservable();
  public readonly onBoardLoaded$ = this.onBoardLoaded.pipe(skip(1));
  public readonly onBoardLoading$ = this.onBoardLoading.pipe(skip(1));
  public readonly onTableDeleted$ = this.onTableDeleted.pipe(skip(1));

  constructor() { }

  public setBoardComponentActive() {
    this._isBoardComponentActive = true;
  }

  public setBoardComponentDisabled() {
    this._isBoardComponentActive = false;
  }

  public isBoardComponentActive() {
    return this._isBoardComponentActive;
  }

  public openBoardSettings() {
    this.onOpenBoardSettings.emit();
  }

  public openInvite() {
    this.onOpenInvite.emit();
  }

  public acceptInvitation() {
    this.onAcceptInvitation.emit();
  }

  public boardLoaded(board: KanBoard) {
    this.onBoardLoaded.next(board);  
  }

  public boardLoading(loading: boolean) {
    this.onBoardLoading.next(loading);  
  }

  public tableDeleted(tableID: number) {
    this.onTableDeleted.next(tableID);
  }
} 
