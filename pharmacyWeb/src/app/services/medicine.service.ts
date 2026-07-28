import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateMedicine, Medicine } from '../models/medicine.model';
import { SaleRecord } from '../models/sale-record.model';

@Injectable({ providedIn: 'root' })
export class MedicineService {
  private readonly baseUrl = '/api/medicines';

  constructor(private http: HttpClient) {}

  getAll(search?: string): Observable<Medicine[]> {
    let params = new HttpParams();
    if (search) {
      params = params.set('search', search);
    }
    return this.http.get<Medicine[]>(this.baseUrl, { params });
  }

  create(medicine: CreateMedicine): Observable<Medicine> {
    return this.http.post<Medicine>(this.baseUrl, medicine);
  }

  sell(id: string, quantity: number): Observable<SaleRecord> {
    return this.http.post<SaleRecord>(`${this.baseUrl}/${id}/sell`, { quantity });
  }
}
