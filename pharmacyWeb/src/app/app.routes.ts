import { Routes } from '@angular/router';
import { MedicineListComponent } from './medicine-list/medicine-list.component';
import { SalesHistoryComponent } from './sales-history/sales-history.component';

export const routes: Routes = [
  { path: '', redirectTo: 'medicines', pathMatch: 'full' },
  { path: 'medicines', component: MedicineListComponent },
  { path: 'sales', component: SalesHistoryComponent },
  { path: '**', redirectTo: 'medicines' }
];
