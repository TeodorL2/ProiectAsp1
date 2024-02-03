import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    let token = localStorage.getItem('JwtToken');
    let headers = new HttpHeaders();
    headers.set('Authorization', 'Beaere ' + token);
    http.get<WeatherForecast[]>(baseUrl + 'users/check-auth', { headers }).subscribe(result => {
     console.log(result)
    }, error => console.error(error));
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
