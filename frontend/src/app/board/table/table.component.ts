import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DragulaService } from 'ng2-dragula';
import { delay, of, Subscription, take } from 'rxjs';
import { KanTable } from 'src/app/_models/kan-table';
import { getDefaultTask, KanTask } from 'src/app/_models/kan-task';
import { TablesService } from 'src/app/_services/tables.service';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent {
  @Input() table!: KanTable;
  public tableNameEdit: {
    enabled: boolean;
    pending: boolean;
    form?: FormGroup;
  } = {
    enabled: false,
    pending: false,
    form: undefined,
  };
  public tasks: KanTask[] = [];
  public getDefaultTask = getDefaultTask;
  private subs = new Subscription();

  constructor(
    private formBuilder: FormBuilder,
    private tablesService: TablesService
  ) {
  }

  public startTableNameEdit() {
    this.tableNameEdit.enabled = true;
    this.tableNameEdit.pending = false;
    this.tableNameEdit.form = this.formBuilder.group({
      name: [
        this.table.name,
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(256),
        ],
      ],
    });
  }

  public stopTableNameEdit() {
    this.tableNameEdit.enabled = false;
    this.tableNameEdit.pending = false;
    this.tableNameEdit.form = undefined;
  }

  public submitNewTableName() {
    this.table.name = this.tableNameEdit.form?.controls['name'].value;
    this.tableNameEdit.pending = true;

    this.tablesService.update(this.table).subscribe((e) => {
      this.stopTableNameEdit();
    });
  }
}
