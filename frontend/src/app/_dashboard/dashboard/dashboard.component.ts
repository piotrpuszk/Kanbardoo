import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription, take } from 'rxjs';
import { KanRoleID } from 'src/app/constants';
import { BoardFilters } from 'src/app/_models/board-filters';
import { KanBoard } from 'src/app/_models/kan-board';
import { BoardControllerService } from 'src/app/_services/board-controller.service';
import { BoardsService } from 'src/app/_services/boards.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit, OnDestroy {
  public ownershipBoards: KanBoard[] = [];
  public membershipBoards: KanBoard[] = [];

  public ownershipBoardsKey = 'ownershipBoards';
  public membershipBoardsKey = 'memberBoards';

  private readonly ownershipFilters: BoardFilters = {
    boardName: '',
    orderByClauses: [],
    roleID: KanRoleID.owner,
  };
  private readonly membershipFilters: BoardFilters = {
    boardName: '',
    orderByClauses: [],
    roleID: KanRoleID.member,
  };

  private subs: { [key: string]: Subscription } = {};
  private pedings: { [key: string]: boolean } = {};
 
  private sub = new Subscription();

  constructor(private boardsService: BoardsService, private boardController: BoardControllerService) {}

  ngOnInit() {
    this.subs[this.ownershipBoardsKey] = undefined!;
    this.subs[this.membershipBoardsKey] = undefined!;
    this.getOwnershipBoards();
    this.getMembershipBoards();

    this.sub.add(this.boardController.onAcceptInvitation$.subscribe(e => {
      this.getMembershipBoards();
    }));
  }

  ngOnDestroy() {
    for (const key in this.subs) {
      if (Object.prototype.hasOwnProperty.call(this.subs, key)) {
        if(!this.subs[key]) continue;
        this.subs[key].unsubscribe();
      }
    }
  }

  public isPending(key: string) {
    return this.pedings[key];
  }

  private getOwnershipBoards() {
    this.enablePending(this.ownershipBoardsKey);
    this.boardsService.getBoards(this.ownershipFilters).pipe(take(1)).subscribe(result => {
      this.ownershipBoards = result.content;
      this.disablePending(this.ownershipBoardsKey);
    });
  }

  private getMembershipBoards() {
    this.enablePending(this.membershipBoardsKey);
    this.boardsService.getBoards(this.membershipFilters).pipe(take(1)).subscribe(result => {
      this.membershipBoards = result.content;
      this.disablePending(this.membershipBoardsKey);
    });
  }

  private enablePending(key: string) {
    this.pedings[key] = true;
  }

  private disablePending(key: string) {
    this.pedings[key] = false;
  }
}
