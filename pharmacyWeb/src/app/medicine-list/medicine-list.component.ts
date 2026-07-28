import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { AddMedicineComponent } from '../add-medicine/add-medicine.component';
import { Medicine } from '../models/medicine.model';
import { MedicineService } from '../services/medicine.service';

const EXPIRY_WARNING_DAYS = 30;
const LOW_STOCK_THRESHOLD = 10;

@Component({
  selector: 'app-medicine-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, AddMedicineComponent],
  templateUrl: './medicine-list.component.html',
  styleUrl: './medicine-list.component.css'
})
export class MedicineListComponent implements OnInit, OnDestroy {
  medicines: Medicine[] = [];
  searchTerm = '';
  loading = false;
  errorMessage = '';
  showAddForm = false;

  sellQuantity: Record<string, number> = {};
  sellError: Record<string, string> = {};
  sellSuccess: Record<string, string> = {};
  selling: Record<string, boolean> = {};

  private searchSubject = new Subject<string>();
  private searchSubscription?: Subscription;

  constructor(private medicineService: MedicineService) {}

  ngOnInit(): void {
    this.load();
    this.searchSubscription = this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe(term => this.load(term));
  }

  ngOnDestroy(): void {
    this.searchSubscription?.unsubscribe();
  }

  onSearchChange(term: string): void {
    this.searchTerm = term;
    this.searchSubject.next(term);
  }

  load(search: string = this.searchTerm): void {
    this.loading = true;
    this.errorMessage = '';
    this.medicineService.getAll(search).subscribe({
      next: medicines => {
        this.medicines = medicines;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load medicines. Is the API running?';
        this.loading = false;
      }
    });
  }

  onMedicineAdded(): void {
    this.showAddForm = false;
    this.load();
  }

  daysUntilExpiry(medicine: Medicine): number {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const expiry = new Date(medicine.expiryDate);
    return Math.round((expiry.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
  }

  isExpiringSoon(medicine: Medicine): boolean {
    return this.daysUntilExpiry(medicine) < EXPIRY_WARNING_DAYS;
  }

  isLowStock(medicine: Medicine): boolean {
    return medicine.quantity < LOW_STOCK_THRESHOLD;
  }

  sell(medicine: Medicine): void {
    const quantity = this.sellQuantity[medicine.id] || 1;
    this.sellError[medicine.id] = '';
    this.sellSuccess[medicine.id] = '';
    this.selling[medicine.id] = true;

    this.medicineService.sell(medicine.id, quantity).subscribe({
      next: sale => {
        medicine.quantity -= sale.quantitySold;
        this.sellSuccess[medicine.id] = `Sold ${sale.quantitySold} unit(s).`;
        this.sellQuantity[medicine.id] = 1;
        this.selling[medicine.id] = false;
      },
      error: err => {
        this.sellError[medicine.id] = err?.error?.message ?? 'Failed to record sale.';
        this.selling[medicine.id] = false;
      }
    });
  }
}
