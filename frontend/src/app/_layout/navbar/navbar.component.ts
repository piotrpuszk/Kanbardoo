import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, take, tap } from 'rxjs';
import { BoardComponent } from 'src/app/board/board.component';
import { KanRoleID, KanRoleName } from 'src/app/constants';
import { KanBoardUser } from 'src/app/_models/kan-board-user';
import { KanUser } from 'src/app/_models/kan-user';
import { BoardControllerService } from 'src/app/_services/board-controller.service';
import { BoardsService } from 'src/app/_services/boards.service';
import { UsersService } from 'src/app/_services/users.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  public allMembers: KanBoardUser[] = [];
  public boardLoading = false;
  public membersLoading = false;

  private sub = new Subscription();

  constructor(
    public usersService: UsersService,
    private router: Router,
    private boardControllerService: BoardControllerService,
    private boardsService: BoardsService
  ) {
    this.sub.add(this.boardControllerService.onBoardLoaded$
      .subscribe(board => {
        this.membersLoading = true;
        this.boardsService
        .getBoardMembers(board.id)
        .pipe(take(1))
        .subscribe(response => {
          const members = response.content;
          this.allMembers = [{...board.owner, roleName: KanRoleName.owner}, ...members];
          this.membersLoading = false;
        });
    }));

    this.sub.add(this.boardControllerService.onBoardLoading$.subscribe(loading => {
      this.boardLoading = loading;
    }));
  }

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

  public openInvite() {
    this.boardControllerService.openInvite();
  }
}
