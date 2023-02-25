import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UsersService } from 'src/app/_services/users.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {

    constructor(public usersService: UsersService, private router: Router) {}

    public signOut() {
      this.usersService.signOut();
      this.router.navigate(["/sign-in"]);
    }
}
