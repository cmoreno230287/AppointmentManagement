import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';

export interface User {
  id: number;
  username: string;
  email: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7110/api/v1/users';
  constructor(private http: HttpClient) { }

  getAppointments():  Observable<{ data:User[]; message: string; success: boolean }> {
      return this.http.get<{ data: User[]; message: string; success: boolean }>(this.apiUrl)
        .pipe(
          catchError(error => {
            console.error('Error fetching appointments', error);
            throw error;
          })
        );
    }
}
