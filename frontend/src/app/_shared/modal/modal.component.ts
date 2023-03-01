import { Component, EventEmitter, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnInit {
  @Input() public title = 'Title';
  @Input() public successButtonName = 'Save changes';
  @Input() public cancelButtonName = 'Cancel';
  public isOpen = false;

  private onSuccess = new EventEmitter();
  private onCancel = new EventEmitter();

  public onSuccess$ = this.onSuccess.asObservable();
  public onCancel$ = this.onCancel.asObservable();

  private pending = false;

  ngOnInit() {
    (document.querySelectorAll('.modal-background, .modal-close, .modal-card-head .delete, .modal-card-foot .button') || [])
    .forEach(($close) => {
      $close.addEventListener('click', () => {
        this.close();
      });
    });
  }

  public open() {
    this.isOpen = true;
    this.stopPending();
  }

  public close() {
    this.isOpen = false;
    this.stopPending();
  }

  public startPending() {
    this.pending = true;
  }

  public stopPending() {
    this.pending = false;
  }

  public success(event: Event) {
    event.stopImmediatePropagation();
    event.preventDefault();
    this.onSuccess.emit();
  }

  public cancel(event: Event) {
    event.stopImmediatePropagation();
    event.preventDefault();
    this.onCancel.emit();
  }

  public isPending() {
    return this.pending;
  }
}
