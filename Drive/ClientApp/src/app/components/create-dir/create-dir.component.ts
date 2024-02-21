import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Router } from '@angular/router';
import { CreateBaseDirService } from '../../core/services/create-base-dir.service';
import { CreateDirService } from '../../core/services/create-dir.service';
import { CreateDirRequestDto } from '../../data/interfaces/create-dir-request-dto';

@Component({
  selector: 'app-create-dir',
  templateUrl: './create-dir.component.html',
  styleUrls: ['./create-dir.component.css'],
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
  ],
  standalone: true
})
export class CreateDirComponent {
  constructor(private readonly formBuilder: FormBuilder, private router: Router, private createDirService: CreateDirService) { }

  createDirForm = this.formBuilder.group({
    DirName: ['', Validators.required]
  });


  create() {
    let path = this.router.url;
    path = path.substring(1);

    let dto: CreateDirRequestDto = {
      DirName: this.createDirForm.get('DirName')?.value?.toString() || '',
      pathToCreateAt: path
    };

    this.createDirService.create(dto).subscribe(
      (data: any) => {
        localStorage.removeItem("createDirMenu");
      },
      (error: any) => {
        console.error(error);
      },
      () => {

      }
    );
  }
}
