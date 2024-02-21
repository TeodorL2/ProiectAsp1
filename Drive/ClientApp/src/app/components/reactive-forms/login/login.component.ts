import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
  ],
  standalone: true
})
export class LoginComponent {
  loginForm = this.formBuilder.group({
    userName: ['', Validators.required],
    password: ['', Validators.required],
  });

  constructor(private readonly formBuilder: FormBuilder, private readonly authenticationService: AuthenticationService, private router: Router) {

  }

  login() {
    this.authenticationService.login(this.loginForm.value).subscribe(
      (data: any) => {
        this.authenticationService.addTokenToLocalStorage(data.token);
        this.router.navigate([this.loginForm.get('userName')?.value]);
      },
      (error: any) => console.error(error.error.message)
    );
  }
}
