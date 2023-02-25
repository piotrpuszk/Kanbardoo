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

@NgModule({
  declarations: [
    AppComponent,
    SignUpComponent,
    SignInComponent,
    NavbarComponent,
    DashboardComponent,
    DashboardCardComponent,
    NewBoardComponent
  ],
  imports: [
    TooltipModule.forRoot(),
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule
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
