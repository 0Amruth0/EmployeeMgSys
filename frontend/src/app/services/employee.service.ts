// src/app/services/employee.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../models/employee.model'; // ✅ make sure this is the one and only model

@Injectable({ providedIn: 'any' })
export class EmployeeService {
  private baseUrl = 'https://webapp-azurelearning-003.azurewebsites.net/api/Employees';

  constructor(private http: HttpClient) {}

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.baseUrl); // ✅ returns correct model type
  }
  addEmployee(employee: Employee): Observable<any> {
    return this.http.post(this.baseUrl, employee);
  }

}
