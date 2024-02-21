import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Router } from '@angular/router';
import { CreateBaseDirService } from '../../core/services/create-base-dir.service';
import { BaseDirCrUpRequestDto } from '../../data/interfaces/base-dir-cr-up-request-dto';

@Component({
  selector: 'app-create-base-dir',
  templateUrl: './create-base-dir.component.html',
  styleUrls: ['./create-base-dir.component.css'],
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
  ],
  standalone: true
})
export class CreateBaseDirComponent {
  constructor(private readonly formBuilder: FormBuilder, private router: Router, private createBaseDirService: CreateBaseDirService) { }

  createBaseDirForm = this.formBuilder.group({
    DirectoryName: ['', Validators.required],
    IsPublic: ['true']
  });


  create() {
    let dto: BaseDirCrUpRequestDto = {
      DirectoryName: this.createBaseDirForm.get('DirectoryName')?.value?.toString() || '',
      IsPublic : this.createBaseDirForm.get('IsPublic')?.value === 'true'
    };

    this.createBaseDirService.create(dto).subscribe(
      (data: any) => {
        localStorage.removeItem("createBaseDirMenu");
      },
      (error: any) => {
        console.error(error);
      },
      () => {
        
      }
    );
  }

}
