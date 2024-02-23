import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UploadFilesService {
  private readonly apiUrl: string;
  private readonly route: string = "storage/upload-files";
  constructor(private readonly apiService: ApiService, private readonly httpClient: HttpClient) {
    this.apiUrl = "https://localhost:7180/api/";
  }

  create(fileCollection: any) {

    return this.httpClient.post<any>(this.apiUrl + this.route, fileCollection);
  }
}
