import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SaleRecord } from '../models/sale-record.model';
import { SalesService } from '../services/sales.service';

@Component({
  selector: 'app-sales-history',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './sales-history.component.html',
  styleUrl: './sales-history.component.css'
})
export class SalesHistoryComponent implements OnInit {
  sales: SaleRecord[] = [];
  loading = false;
  errorMessage = '';

  constructor(private salesService: SalesService) {}

  ngOnInit(): void {
    this.loading = true;
    this.salesService.getAll().subscribe({
      next: sales => {
        this.sales = sales;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load sales history. Is the API running?';
        this.loading = false;
      }
    });
  }
}
