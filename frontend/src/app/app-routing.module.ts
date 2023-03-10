import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BoardComponent } from './board/board.component';
import { NewBoardComponent } from './new-board/new-board.component';
import { SignInComponent } from './_authentication/sign-in/sign-in.component';
import { SignUpComponent } from './_authentication/sign-up/sign-up.component';
import { DashboardComponent } from './_dashboard/dashboard/dashboard.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    component: DashboardComponent
  },
  {
    path: 'sign-up',
    component: SignUpComponent
  },
  {
    path: 'sign-in',
    component: SignInComponent
  },
  {
    path: 'new-board',
    component: NewBoardComponent
  },
  {
    path: 'boards/:id',
    component: BoardComponent
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
