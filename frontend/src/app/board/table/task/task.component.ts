import { Component, Input, OnDestroy } from '@angular/core';
import { DragulaService } from 'ng2-dragula';
import { delay, of, Subscription, take } from 'rxjs';
import { KanTask } from 'src/app/_models/kan-task';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss']
})
export class TaskComponent implements OnDestroy {
  @Input() task!: KanTask;
  private dragging = false;
  private subs = new Subscription();

  constructor(private dragula: DragulaService) {
    this.subs.add(this.dragula.drag().subscribe(e => {
      this.dragging = true;
    }));

    this.subs.add(this.dragula.dragend().subscribe(e => {
      of(1).pipe(take(1), delay(100)).subscribe(e => {
        this.dragging = false;
      });
    }));
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  public isDragging() {
    return this.dragging;
  }
}
