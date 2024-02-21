import { Injectable } from '@angular/core';
import { ApiService } from "./api.service";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private readonly route = 'users';

  constructor(private readonly apiService: ApiService) {
  }

  registerUser(user: any) {
    return this.apiService.post(this.route + '/create-user', user);
  }

  login(user: any) {
    return this.apiService.post(this.route + '/login', user);
  }

  addTokenToLocalStorage(token: string) {
    localStorage.setItem("jwt", token);
  }

  isLoggedIn(): boolean {
    return localStorage.getItem("jwt") == null;
  }
}
