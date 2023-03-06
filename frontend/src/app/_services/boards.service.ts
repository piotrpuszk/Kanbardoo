import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { NewBoard } from '../new-board/_models/new-board';
import { BoardFilters } from '../_models/board-filters';
import { KanBoard } from '../_models/kan-board';
import { KanBoardUser } from '../_models/kan-board-user';
import { KanUser } from '../_models/kan-user';
import { Result } from '../_models/result';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root',
})
export class BoardsService {
  private readonly boardsUrl = environment.apiUrl + 'boards/';

  constructor(private http: HttpClient, private usersService: UsersService) {}

  public getBoards(boardFilters: BoardFilters) {
    return this.http.post<Result<KanBoard[]>>(
      this.boardsUrl,
      boardFilters,
      this.usersService.getOptions()
    );
  }

  public getBoard(id: number) {
    return this.http.get<Result<KanBoard>>(
      this.boardsUrl + id,
      this.usersService.getOptions()
    );
  }

  public create(newBoard: NewBoard) {
    return this.http.post(
      this.boardsUrl + 'add',
      newBoard,
      this.usersService.getOptions()
    );
  }

  public update(board: KanBoard) {
    return this.http.put(this.boardsUrl, board, this.usersService.getOptions());
  }

  public updatePriority(board: KanBoard) {
    return this.http.put(this.boardsUrl + "update-priority", board, this.usersService.getOptions());
  }

  public getBoardMembers(boardID: number) {
    return this.http.get<Result<KanBoardUser[]>>(`${this.boardsUrl}${boardID}/members`, this.usersService.getOptions());
  }
}
