import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { AuthenticationService } from '../../core/services/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  standalone: true,
  imports: [ReactiveFormsModule]
})

export class RegisterComponent {
  registerForm = this.formBuilder.group({
    userName: ['', Validators.required],
    password: ['', Validators.required],
  });

  constructor(private readonly formBuilder: FormBuilder,
    private readonly authenticationService: AuthenticationService) {
  }

  register() {
    this.authenticationService.register(this.registerForm.value)
      .subscribe((data: any) => console.log(data));
  }
}
