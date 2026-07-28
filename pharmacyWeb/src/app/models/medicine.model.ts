export interface Medicine {
  id: string;
  fullName: string;
  notes: string;
  expiryDate: string;
  quantity: number;
  price: number;
  brand: string;
}

export interface CreateMedicine {
  fullName: string;
  notes?: string;
  expiryDate: string;
  quantity: number;
  price: number;
  brand: string;
}
