import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class CreateDirService {
  private readonly route: string = "storage/create-dir";

  constructor(private readonly apiService: ApiService) {

  }

  create(dirToCreate: any) {
    return this.apiService.post(this.route, dirToCreate);
  }
}
