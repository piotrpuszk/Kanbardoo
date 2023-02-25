import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { UsersService } from 'src/app/_services/users.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss'],
})
export class SignUpComponent implements OnInit, OnDestroy {
  public form!: FormGroup;
  private sub = new Subscription();

  constructor(private formBuilder: FormBuilder, private usersService: UsersService) {}

  ngOnInit(): void {
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
      ],
      repeatPassword: [''],
    });

    this.sub.add(this.form.controls['repeatPassword'].valueChanges.subscribe((e) => {
      if(this.form.controls['password'].value != e) {
        this.form.controls['repeatPassword'].setErrors({'notMatchesPassword': true});
      }
    }));

    this.sub.add(this.form.controls['password'].valueChanges.subscribe((e) => {
      this.form.controls['repeatPassword'].updateValueAndValidity();
    }));
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  public signUp() {
    this.form.markAllAsTouched();

    if(!this.form.valid) return;

    this.usersService.signUp({ 
      userName: this.form.controls['username'].value,
      password: this.form.controls['password'].value
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

  public isRepeatPasswordValid() {
    return !this.form.controls['repeatPassword'].errors;
  }

  public isRepeatPasswordTouched() {
    return this.form.controls['password'].touched;
  }

  public isRepeatPasswordDirty() {
    return this.form.controls['password'].dirty;
  }
}
