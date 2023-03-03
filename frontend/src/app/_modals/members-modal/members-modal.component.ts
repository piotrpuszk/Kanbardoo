import { Component, Input, ViewChild } from '@angular/core';
import { KanBoardUser } from 'src/app/_models/kan-board-user';
import { KanUser } from 'src/app/_models/kan-user';
import { ModalComponent } from 'src/app/_shared/modal/modal.component';

@Component({
  selector: 'app-members-modal',
  templateUrl: './members-modal.component.html',
  styleUrls: ['./members-modal.component.scss']
})
export class MembersModalComponent {
  @ViewChild('membersModal') modal!: ModalComponent;
  @Input() members!: KanBoardUser[];
  public pending = false;

  public open() {
    this.modal.open();
  }

  public close() {
    this.modal.close();
  }
}
