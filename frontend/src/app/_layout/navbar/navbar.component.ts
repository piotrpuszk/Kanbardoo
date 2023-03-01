import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BoardComponent } from 'src/app/board/board.component';
import { BoardControllerService } from 'src/app/_services/board-controller.service';
import { UsersService } from 'src/app/_services/users.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  constructor(
    public usersService: UsersService,
    private router: Router,
    private boardControllerService: BoardControllerService
  ) {}

  public signOut() {
    this.usersService.signOut();
    this.router.navigate(['/sign-in']);
  }

  public isBoardView() {
    return this.boardControllerService.isBoardComponentActive();
  }

  public openBoardSettings() {
    this.boardControllerService.openBoardSettings();
  }

  public invite() {

  }
}
