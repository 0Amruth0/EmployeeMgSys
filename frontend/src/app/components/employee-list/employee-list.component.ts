import { Component } from '@angular/core';
import { EmployeeService } from '../../services/employee.service';
import { Employee } from '../../models/employee.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent {
  employees: Employee[] = [];
  showList = false;

  newEmployee: Employee = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    department: '',
    position: '',
    salary: 0,
    hireDate: new Date(),
    isActive: true,
    createdAt: new Date(),
    updatedAt: new Date()
  };

  message: string = '';

  constructor(private employeeService: EmployeeService) {}

  fetchEmployees(): void {
    this.employeeService.getEmployees().subscribe({
      next: data => {
        this.employees = data;
        this.showList = true;
        this.message = '';
      },
      error: err => {
        this.message = 'Error fetching employees.';
      }
    });
  }

  addEmployee(): void {
    this.newEmployee.createdAt = new Date();
    this.newEmployee.updatedAt = new Date();
    this.newEmployee.isActive = true;

    this.employeeService.addEmployee(this.newEmployee).subscribe({
      next: () => {
        this.message = 'Employee added successfully!';
        this.resetForm();
      },
      error: err => {
        this.message = 'Failed to add employee.';
      }
    });
  }

  private resetForm(): void {
    this.newEmployee = {
      id: 0,
      firstName: '',
      lastName: '',
      email: '',
      department: '',
      position: '',
      salary: 0,
      hireDate: new Date(),
      isActive: true,
      createdAt: new Date(),
      updatedAt: new Date()
    };
  }
}
