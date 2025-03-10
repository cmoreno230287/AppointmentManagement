import { Component, OnInit, signal } from '@angular/core';
import { AgGridModule } from 'ag-grid-angular';
import { ColDef, ClientSideRowModelModule } from 'ag-grid-community';
import { Module } from 'ag-grid-community';
import { AppointmentService } from '../services/appointment.service';


@Component({
  selector: 'app-appointments-list',
  standalone: true,
  imports: [AgGridModule],
  templateUrl: './appointments-list.component.html',
  styles: [
    `.container { padding: 20px; }
     .error { color: red; font-weight: bold; }`
  ],
  providers: [AppointmentService]
})
export class AppointmentsListComponent implements OnInit {
  modules: Module[] = [ClientSideRowModelModule];
  appointments = signal<any[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  colDefs: ColDef[] = [
    { field: "id", headerName: "ID", width: 80, sortable: true, hide: true },
    { field: "appointmentDate", headerName: "Date", width: 150, sortable: true, filter: "agDateColumnFilter" },
    { field: "appointmentTime", headerName: "Time", width: 150, sortable: true },
    { field: "status", headerName: "Status", width: 120, sortable: true, filter: "agTextColumnFilter" },
    { field: "notes", headerName: "Notes", width: 250, filter: "agTextColumnFilter" }
  ];

  defaultColDef: ColDef = {
    resizable: true,
    filter: true,
    sortable: true
  };

  constructor(private appointmentService: AppointmentService) {
  }
  

  ngOnInit(): void {
    this.fetchAppointments();
  }

  fetchAppointments(): void {
    this.loading.set(true);
    this.error.set(null);

    this.appointmentService.getAppointments().subscribe({
      next: (response) => {
        if (response.success) {
          this.appointments.set(response.data);
        } else {
          this.error.set(response.message || 'Failed to fetch appointments');
        }
      },
      error: (err) => {
        this.error.set('Failed to load appointments');
        console.error(err);
      },
      complete: () => {
        this.loading.set(false)
      }
    });
  }
  /*fetchAppointments(): void {
    this.loading.set(true);
    this.http.get<{ data: any[], message: string, success: boolean }>('https://localhost:7110/api/v1/appointments')
      .subscribe({
        next: (response) => {
          if (response.success) {
            this.appointments.set(response.data);
          } else {
            this.error.set(response.message || 'Failed to fetch appointments');
          }
        },
        error: () => {
          this.error.set('Error fetching appointments. Please try again.');
        },
        complete: () => this.loading.set(false)
      });
  }*/
}
