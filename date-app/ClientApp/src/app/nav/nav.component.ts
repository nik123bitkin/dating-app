import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-nav',
    templateUrl: './nav.component.html',
    styleUrls: ['./nav.component.css']
})
/** nav component*/
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string;
  /** nav ctor */
  constructor(public authService: AuthService,
    private alertify: AlertifyService,
    private router: Router) {

  }
  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => {
      this.photoUrl = photoUrl;
    })
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success("Logged in successfully");

    }, error => {
      this.alertify.error(error);
    }, () => {
        this.router.navigate(['/members']);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.currentUser = null;
    this.authService.decodedToken = null;
    this.alertify.message('Logged out');
    this.router.navigate(['/home']);
  }
}
