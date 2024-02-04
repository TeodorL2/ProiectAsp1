import { Injectable } from '@angular/core';
import { ApiService } from "./api.service";

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  private readonly route = "directory";

  constructor(private readonly apiService: ApiService) {
  }

  getEntries(path: string) {
    return this.apiService.get(`${this.route}/${path}`);
  }

}
