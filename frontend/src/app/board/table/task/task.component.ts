import { Component, Input } from '@angular/core';
import { KanTask } from 'src/app/_models/kan-task';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss']
})
export class TaskComponent {
  @Input() task!: KanTask;
}
