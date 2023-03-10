import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { NewTable } from '../board/_models/new-table';
import { KanTable } from '../_models/kan-table';
import { KanTask } from '../_models/kan-task';
import { Result } from '../_models/result';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root'
})
export class TablesService {
  private readonly baseUrl = environment.apiUrl + 'tables';

  constructor(private usersService: UsersService, private http: HttpClient) { }

  public add(newTable: NewTable) {
    return this.http.post(this.baseUrl, newTable, this.usersService.getOptions());
  }

  public get(id: number) {
    return this.http.get<Result<KanTable>>(this.baseUrl + "/" + id, this.usersService.getOptions());
  }

  public update(table: KanTable) {
    return this.http.put(this.baseUrl, table, this.usersService.getOptions());
  }

  public delete(id: number) {
    var params = new HttpParams();
    params = params.append('id', id);
    return this.http.delete(this.baseUrl, {params, ...this.usersService.getOptions()});
  }
}
