import { Component, ViewChild } from '@angular/core';
import { take } from 'rxjs';
import { Invitation } from 'src/app/_models/invitation';
import { BoardControllerService } from 'src/app/_services/board-controller.service';
import { BoardsService } from 'src/app/_services/boards.service';
import { InvitationsService } from 'src/app/_services/invitations.service';
import { ModalComponent } from 'src/app/_shared/modal/modal.component';

@Component({
  selector: 'app-invitations-modal',
  templateUrl: './invitations-modal.component.html',
  styleUrls: ['./invitations-modal.component.scss'],
})
export class InvitationsModalComponent {
  @ViewChild('invitationsModal') modal!: ModalComponent;
  public invitations?: Invitation[];
  public pending = false;
  public handleInvitationPending = false;

  constructor(private invitationsService: InvitationsService, private boardController: BoardControllerService) {}

  public open() {
    this.modal.open();
    this.getInvitations();
  }

  public close() {
    this.modal.close();
  }

  public accept(invitation: Invitation) {
    this.handleInvitationPending = true;
    this.invitationsService
      .accept({ id: invitation.id })
      .pipe(take(1))
      .subscribe((e) => {
        this.boardController.acceptInvitation();
        this.handleInvitationPending = false;
        this.getInvitations();
      });
  }

  public decline(invitation: Invitation) {
    this.handleInvitationPending = true;
    this.invitationsService
      .decline({ invitationID: invitation.id })
      .pipe(take(1))
      .subscribe((e) => {
        this.handleInvitationPending = false;
        this.getInvitations();
      });
  }

  private getInvitations() {
    this.pending = true;
    this.invitationsService.get().subscribe((response) => {
      this.invitations = response.content;
      this.pending = false;
    });
  }
}
