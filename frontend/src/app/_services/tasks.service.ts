import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
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
  private taskStatusesCached: KanTaskStatus[] = [];
  private taskStatusesPending = false;

  constructor(private usersService: UsersService, private http: HttpClient) { }

  public getTaskStatuses() {
    if(this.taskStatusesCached.length > 0) return of<Result<KanTaskStatus[]>>({
      isSuccess: true,
      content: this.taskStatusesCached,
      errors: []
    });

    return this.http.get<Result<KanTaskStatus[]>>(this.taskStatusesUrl, this.usersService.getOptions())
    .pipe(tap(e => this.taskStatusesCached = e.content));
  }

  public add(newTask: NewKanTask) {
    return this.http.post(this.baseUrl, newTask, this.usersService.getOptions());
  }

  public update(task: KanTask) {
    return this.http.put(this.baseUrl, task, this.usersService.getOptions());
  }
}
