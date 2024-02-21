import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { AuthenticationService } from '../../../core/services/authentication.service';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    CommonModule,
  ],
})
export class RegisterComponent {
  registerForm = this.formBuilder.group({
    userName: ['', Validators.required],
    password: ['', Validators.required],
  });

  accountAlreadyUsed: string = 'sirul initial';
  infoBox = false;

  constructor(private readonly formBuilder: FormBuilder,
    private readonly authenticationService: AuthenticationService,
    private router: Router) {
  }

  register() {
    this.authenticationService.registerUser(this.registerForm.value)
      .subscribe((data: any) => {
        this.authenticationService.addTokenToLocalStorage(data.token);
        this.router.navigate([this.registerForm.get('userName')?.value]);
      },
        (err: any) => {
          if (err instanceof HttpErrorResponse)
            if (err.status === 400) {
              var msg = err.error.message;
            this.accountAlreadyUsed = "account already exists";
            this.infoBox = true;
            console.log("mesaj: " + msg);
            }
        });
  }

  hideInfoBox() {
    this.infoBox = false;
  }
}
