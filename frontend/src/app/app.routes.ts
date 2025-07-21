// src/app/app.routes.ts
import { Routes } from '@angular/router';
import { EmployeeListComponent } from './components/employee-list/employee-list.component';

export const routes: Routes = [
  { path: '', component: EmployeeListComponent }  // 👈 this route will match "/"
];
