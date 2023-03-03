import { EventEmitter, Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BoardControllerService {
  private _isBoardComponentActive = false;
  private readonly onOpenBoardSettings = new EventEmitter();
  private readonly onOpenInvite = new EventEmitter();
  private readonly onAcceptInvitation = new EventEmitter();

  public readonly onOpenBoardSettings$ = this.onOpenBoardSettings.asObservable();
  public readonly onOpenInvite$ = this.onOpenInvite.asObservable();
  public readonly onAcceptInvitation$ = this.onAcceptInvitation.asObservable();

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
}
