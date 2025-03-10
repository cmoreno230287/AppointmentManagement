import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';

export interface Appointment {
  id: number;
  appointmentDate: string;
  appointmentTime: string;
  status: string;
  notes: string;
}

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private apiUrl = 'https://localhost:7110/api/v1/appointments';

  constructor(private http: HttpClient) {}

  getAppointments():  Observable<{ data:Appointment[]; message: string; success: boolean }> {
    return this.http.get<{ data: Appointment[]; message: string; success: boolean }>(this.apiUrl)
      .pipe(
        catchError(error => {
          console.error('Error fetching appointments', error);
          throw error;
        })
      );
  }
}
