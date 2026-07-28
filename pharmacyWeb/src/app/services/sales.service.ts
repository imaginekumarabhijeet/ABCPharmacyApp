import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SaleRecord } from '../models/sale-record.model';

@Injectable({ providedIn: 'root' })
export class SalesService {
  private readonly baseUrl = '/api/sales';

  constructor(private http: HttpClient) {}

  getAll(): Observable<SaleRecord[]> {
    return this.http.get<SaleRecord[]>(this.baseUrl);
  }
}
