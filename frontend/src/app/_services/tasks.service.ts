import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { KanTask } from '../_models/kan-task';
import { KanTaskStatus } from '../_models/kan-task-status';
import { NewKanTask } from '../_models/new-kan-task';
import { Result } from '../_models/result';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private readonly baseUrl = environment.apiUrl + 'tasks';
  private readonly taskStatusesUrl = environment.apiUrl + 'taskStatuses';

  constructor(private usersService: UsersService, private http: HttpClient) { }

  public getTaskStatuses() {
    return this.http.get<Result<KanTaskStatus[]>>(this.taskStatusesUrl, this.usersService.getOptions());
  }

  public add(newTask: NewKanTask) {
    return this.http.post(this.baseUrl, newTask, this.usersService.getOptions());
  }

  public update(task: KanTask) {
    return this.http.put(this.baseUrl, task, this.usersService.getOptions());
  }
}
