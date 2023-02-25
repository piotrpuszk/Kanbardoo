import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UsersService } from 'src/app/_services/users.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss'],
})
export class SignInComponent implements OnInit, OnDestroy {
  public form!: FormGroup;
  private sub = new Subscription();

  constructor(
    private formBuilder: FormBuilder,
    private usersService: UsersService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.sub.add(this.usersService.loggedUser$.subscribe(e => {
      if(e.id !== 0) {
        this.router.navigate(["/dashboard"]);
      }
    }));

    this.form = this.formBuilder.group({
      username: [
        '',
        [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(24),
        ],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(1024),
        ],
      ]
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  public signIn() {
    this.form.markAllAsTouched();

    if (!this.form.valid) return;

    this.usersService.signIn({
      userName: this.form.controls['username'].value,
      password: this.form.controls['password'].value,
    });
  }

  public isUserNameTouched() {
    return this.form.controls['username'].touched;
  }

  public isUserNameDirty() {
    return this.form.controls['username'].dirty;
  }

  public isUserNameValid() {
    return this.form.controls['username'].valid;
  }

  public isPasswordTouched() {
    return this.form.controls['password'].touched;
  }

  public isPasswordDirty() {
    return this.form.controls['password'].dirty;
  }

  public isPasswordValid() {
    return this.form.controls['password'].valid;
  }
}
