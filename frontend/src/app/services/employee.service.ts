// src/app/services/employee.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Employee, EmployeePost } from '../models/employee.model'; // ✅ make sure this is the one and only model

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private baseUrl = 'https://fnapp-azurelearning-007.azurewebsites.net/api/employees?code=_XFWkFjcidLx-GZlnysjHtP9w353GuRJLIpBp4icwJUoAzFuY85DGg==';
  constructor(private http: HttpClient) {}

  getEmployees(): Observable<Employee[]> {
    return this.http.get<any>(this.baseUrl).pipe(
      map( response => response.Value as Employee[])
    );
  }
  addEmployee(employee: EmployeePost): Observable<any> {
    return this.http.post(this.baseUrl, employee);
  }

}