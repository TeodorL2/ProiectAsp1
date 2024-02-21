import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class GetEntriesService {

  private readonly route: string = "storage";
  constructor(private readonly apiService: ApiService) {
    
  }

  getEntries(path: string) {
    return this.apiService.get(this.route + path);
  }
}
