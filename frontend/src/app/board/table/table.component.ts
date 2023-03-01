import { Component, Input } from '@angular/core';
import { KanTable } from 'src/app/_models/kan-table';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class TableComponent {
  @Input() table!: KanTable;
}
