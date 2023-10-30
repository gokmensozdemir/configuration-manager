import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class ConfigurationService {
  private baseUrl = 'http://localhost:8080/api';

  constructor(private http: HttpClient) {}

  getItems(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/configurations`);
  }

  createItem(item: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/configurations`, item);
  }

  updateItem(item: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/configurations/${item.id}`, item);
  }
}
