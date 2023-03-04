import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AcceptInvitation } from '../_models/accept-invitation';
import { CancelInvitation } from '../_models/cancel-invitation';
import { DeclineInvitation } from '../_models/decline-invitation';
import { Invitation } from '../_models/invitation';
import { NewInvitation } from '../_models/new-invitation';
import { Result } from '../_models/result';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root'
})
export class InvitationsService {
  private readonly baseUrl = environment.apiUrl + "invitations";

  constructor(private usersService: UsersService, private http: HttpClient) { }

  public get() {
    return this.http.get<Result<Invitation[]>>(this.baseUrl, this.usersService.getOptions());
  }

  public invite(newInvitation: NewInvitation) {
    return this.http.post(this.baseUrl + '/invite', newInvitation, this.usersService.getOptions());
  }

  public cancel(cancelInvitation: CancelInvitation) {
    const options = {...this.usersService.getOptions(), body: cancelInvitation};
    return this.http.delete(this.baseUrl, options);
  }

  public accept(acceptInvitation: AcceptInvitation) {
    return this.http.post(this.baseUrl + '/accept', acceptInvitation, this.usersService.getOptions());
  }

  public decline(declineInvitation: DeclineInvitation) {
    return this.http.post(this.baseUrl + '/decline', declineInvitation, this.usersService.getOptions());
  } 
}
