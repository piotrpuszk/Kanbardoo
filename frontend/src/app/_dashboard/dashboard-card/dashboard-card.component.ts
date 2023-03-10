import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { KanBoard } from 'src/app/_models/kan-board';

@Component({
  selector: 'app-dashboard-card',
  templateUrl: './dashboard-card.component.html',
  styleUrls: ['./dashboard-card.component.scss']
})
export class DashboardCardComponent {
  @Input() public board!: KanBoard;

  constructor(private router: Router) {}

  navigateToBoard() {
    this.router.navigate([`/boards/${this.board.id}`]);
  }
}
