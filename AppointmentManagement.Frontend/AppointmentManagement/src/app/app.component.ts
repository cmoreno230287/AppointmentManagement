import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppointmentsListComponent } from './appointments-list/appointments-list.component';
//import { CreateAppointmentComponent } from './appointment-create/appointment-create.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AppointmentsListComponent/*, CreateAppointmentComponent*/],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'AgGridTest';
}
