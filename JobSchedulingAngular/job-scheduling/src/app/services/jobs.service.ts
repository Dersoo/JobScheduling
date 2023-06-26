import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IConfiguration } from '../models/configuration';

@Injectable({
  providedIn: 'root'
})
export class JobsService {

  constructor(private http: HttpClient) { 

  }

  baseUri = "https://localhost:7066/api/Jobs/";

  getConfiguration() : Observable<IConfiguration> {
    return this.http.get<IConfiguration>(this.baseUri + "GetSchedule");
  }

  setConfiguration(configuration: IConfiguration) : Observable<IConfiguration> {
    return this.http.post<IConfiguration>(this.baseUri + "ChangeSchedule", configuration);
  }
}
