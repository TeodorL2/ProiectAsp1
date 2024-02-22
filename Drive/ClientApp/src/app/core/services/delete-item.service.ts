import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class DeleteItemService {
  private readonly route: string = "storage";

  constructor(private readonly apiService: ApiService) { }

  delete(path: string) {
    return this.apiService.delete(this.route + path);
  }
}
