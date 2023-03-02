import { Component, ViewChild } from '@angular/core';
import { take } from 'rxjs';
import { Invitation } from 'src/app/_models/invitation';
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

  constructor(private invitationsService: InvitationsService) {}

  public open() {
    this.getInvitations();
  }

  public close() {
    this.modal.close();
  }

  public accept(invitation: Invitation) {
    this.invitationsService
      .accept({ id: invitation.id })
      .pipe(take(1))
      .subscribe((e) => {
        this.getInvitations();
      });
  }

  public decline(invitation: Invitation) {
    
  }

  private getInvitations() {
    this.invitationsService.get().subscribe((response) => {
      this.invitations = response.content;
      this.modal.open();
    });
  }
}
