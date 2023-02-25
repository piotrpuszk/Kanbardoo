import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { BoardsService } from '../_services/boards.service';
import { UsersService } from '../_services/users.service';

@Component({
  selector: 'app-new-board',
  templateUrl: './new-board.component.html',
  styleUrls: ['./new-board.component.scss'],
})
export class NewBoardComponent {
  public form!: FormGroup;
  private sub = new Subscription();
  public pending = false;

  constructor(
    private formBuilder: FormBuilder,
    private boardsService: BoardsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      boardName: [
        '',
        [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(256),
        ],
      ],
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  public create() {
    this.form.markAllAsTouched();

    if (!this.form.valid) return;

    this.pending = true;
    this.boardsService
      .create({
        name: this.form.controls['boardName'].value,
      })
      .subscribe((e) => {
        this.router.navigate(['/dashboard']);
        this.pending = false;
      });
  }

  public isBoardNameTouched() {
    return this.form.controls['boardName'].touched;
  }

  public isBoardNameDirty() {
    return this.form.controls['boardName'].dirty;
  }

  public isBoardNameValid() {
    return this.form.controls['boardName'].valid;
  }
}
