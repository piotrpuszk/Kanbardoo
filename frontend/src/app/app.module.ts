import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SignUpComponent } from './_authentication/sign-up/sign-up.component';
import { SignInComponent } from './_authentication/sign-in/sign-in.component';
import { NavbarComponent } from './_layout/navbar/navbar.component';
import { DashboardComponent } from './_dashboard/dashboard/dashboard.component';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { DashboardCardComponent } from './_dashboard/dashboard-card/dashboard-card.component';
import { DelayInterceptor } from './_interceptors/delay.interceptor';
import { NewBoardComponent } from './new-board/new-board.component';
import { BoardComponent } from './board/board.component';
import { TableComponent } from './board/table/table.component';
import { ModalComponent } from './_shared/modal/modal.component';
import { BoardSettingsModalComponent } from './board/_modals/board-settings-modal/board-settings-modal.component';
import { TaskComponent } from './board/table/task/task.component';
import { NewTaskModalComponent } from './board/table/_modals/new-task-modal/new-task-modal.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { FormsModule } from '@angular/forms';
import { TaskModalComponent } from './board/table/task/_modals/task-modal/task-modal.component';
import { InviteModalComponent } from './board/_modals/invite-modal/invite-modal.component';
import { InvitationsModalComponent } from './_modals/invitations-modal/invitations-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    SignUpComponent,
    SignInComponent,
    NavbarComponent,
    DashboardComponent,
    DashboardCardComponent,
    NewBoardComponent,
    BoardComponent,
    TableComponent,
    ModalComponent,
    BoardSettingsModalComponent,
    TaskComponent,
    NewTaskModalComponent,
    TaskModalComponent,
    InviteModalComponent,
    InvitationsModalComponent
  ],
  imports: [
    TypeaheadModule.forRoot(),
    TooltipModule.forRoot(),
    BsDatepickerModule.forRoot(),
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: DelayInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
