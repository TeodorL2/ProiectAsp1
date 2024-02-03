import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from './core/services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  loginForm = this.formBuilder.group({
    userName: ['', Validators.required],
    password: ['', Validators.required],
  });

  constructor(private readonly formBuilder: FormBuilder, private readonly authenticationService: AuthenticationService) {

  }

  login() {

    this.authenticationService.login(this.loginForm.value).subscribe((data: any) => console.log(data));

  }
}
