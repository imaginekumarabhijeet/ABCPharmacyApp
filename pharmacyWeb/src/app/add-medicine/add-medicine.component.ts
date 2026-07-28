import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Medicine } from '../models/medicine.model';
import { MedicineService } from '../services/medicine.service';

@Component({
  selector: 'app-add-medicine',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-medicine.component.html',
  styleUrl: './add-medicine.component.css'
})
export class AddMedicineComponent {
  @Output() added = new EventEmitter<Medicine>();
  @Output() cancelled = new EventEmitter<void>();

  submitting = false;
  errorMessage = '';
  form: FormGroup;

  constructor(private fb: FormBuilder, private medicineService: MedicineService) {
    this.form = this.fb.group({
      fullName: ['', [Validators.required, Validators.maxLength(200)]],
      notes: [''],
      expiryDate: ['', Validators.required],
      quantity: [0, [Validators.required, Validators.min(0)]],
      price: [0, [Validators.required, Validators.min(0)]],
      brand: ['', [Validators.required, Validators.maxLength(100)]]
    });
  }

  get f() {
    return this.form.controls;
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    this.submitting = true;
    this.errorMessage = '';

    this.medicineService
      .create({
        fullName: value.fullName!.trim(),
        notes: value.notes?.trim() ?? '',
        expiryDate: value.expiryDate!,
        quantity: Number(value.quantity),
        price: Math.round(Number(value.price) * 100) / 100,
        brand: value.brand!.trim()
      })
      .subscribe({
        next: medicine => {
          this.submitting = false;
          this.form.reset({ fullName: '', notes: '', expiryDate: '', quantity: 0, price: 0, brand: '' });
          this.added.emit(medicine);
        },
        error: err => {
          this.submitting = false;
          this.errorMessage = err?.error?.message ?? 'Failed to add medicine. Please check the form and try again.';
        }
      });
  }

  cancel(): void {
    this.cancelled.emit();
  }
}
