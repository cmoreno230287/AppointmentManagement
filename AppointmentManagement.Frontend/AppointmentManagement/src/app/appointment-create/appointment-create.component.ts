import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-appointment',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './create-appointment.component.html',
  styles: [
    `.form-container { max-width: 400px; margin: auto; padding: 20px; }
     .error { color: red; font-size: 12px; }`
  ]
})
export class CreateAppointmentComponent implements OnInit {
  appointmentForm: FormGroup;
  users$: Observable<{ id: number; username: string; }[]> | undefined;
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.appointmentForm = this.fb.group({
      userId: ['', Validators.required],
      note: ['', [Validators.required, Validators.minLength(5)]],
      appointmentDate: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.fetchUsers();
  }

  fetchUsers(): void {
    this.users$ = this.http.get<{ id: number; username: string }[]>('https://localhost:7110/api/v1/users')
      .pipe(
        catchError(err => {
          this.error.set('Failed to load users');
          throw err;
        })
      );
  }

  createAppointment(): void {
    if (this.appointmentForm.invalid) return;
    
    this.loading.set(true);
    this.error.set(null);
    
    const formData = this.appointmentForm.value;
    
    this.http.post<{ data: any, message: string, success: boolean }>('https://localhost:7110/api/v1/appointments', formData)
      .subscribe({
        next: (response) => {
          if (response.success) {
            alert('Appointment created successfully!');
            this.appointmentForm.reset();
          } else {
            this.error.set(response.message || 'Failed to create appointment');
          }
        },
        error: () => {
          this.error.set('Error creating appointment. Try again.');
        },
        complete: () => this.loading.set(false)
      });
  }
}
