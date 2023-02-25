import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { NewBoard } from '../new-board/_models/new-board';
import { BoardFilters } from '../_models/board-filters';
import { KanBoard } from '../_models/kan-board';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root',
})
export class BoardsService {
  private readonly boardsUrl = environment.apiUrl + 'boards/';

  constructor(private http: HttpClient, private usersService: UsersService) {}

  public getBoards(boardFilters: BoardFilters) {
    return this.http
      .post<KanBoard[]>(
        this.boardsUrl,
        boardFilters,
        this.usersService.getOptions()
      )
      .pipe(map((e: any) => e.content));
  }

  public create(newBoard: NewBoard) {
    return this.http.post(
      this.boardsUrl + 'add',
      newBoard,
      this.usersService.getOptions()
    );
  }
}
