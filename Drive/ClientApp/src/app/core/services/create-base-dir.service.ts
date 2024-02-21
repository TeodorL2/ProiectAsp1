import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class CreateBaseDirService {

  private readonly route: string = "storage/create-base-dir";

  constructor(private readonly apiService: ApiService) {

  }

  create(baseDirToCreate: any) {
    return this.apiService.post(this.route, baseDirToCreate);
  }
}
