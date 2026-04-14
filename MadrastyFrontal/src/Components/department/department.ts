import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { IDepartment } from '../../Models/idepartment';
import { IAddDepartment } from '../../Models/idepartment-add';
import { IUpdateDepartment } from '../../Models/idepartment-update';
import { DepartmentService } from '../../Services/department-service';

@Component({
  selector: 'app-department',
  imports: [FormsModule, RouterLink],
  templateUrl: './department.html',
  styleUrl: './department.css',
})
export class Department implements OnInit {
  private departmentService = inject(DepartmentService);

  departments = signal<IDepartment[]>([]);
  errorMessage = signal('');

  departmentModel: IAddDepartment & IUpdateDepartment = {
    id: 0,
    name: '',
    location: '',
  };

  isEditMode = false;

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.departmentService.getAllDepartments().subscribe({
      next: (res) => this.departments.set(res),
      error: (err) => this.errorMessage.set(err.error || 'Failed to load departments'),
    });
  }

  submit(): void {
    if (this.isEditMode) {
      this.departmentService.updateDepartment(this.departmentModel.id, this.departmentModel).subscribe({
        next: () => {
          this.resetForm();
          this.loadDepartments();
        },
        error: (err) => this.errorMessage.set(err.error || 'Failed to update department'),
      });
      return;
    }

    this.departmentService.addDepartment(this.departmentModel).subscribe({
      next: () => {
        this.resetForm();
        this.loadDepartments();
      },
      error: (err) => this.errorMessage.set(err.error || 'Failed to add department'),
    });
  }

  edit(item: IDepartment): void {
    this.departmentModel = {
      id: item.id,
      name: item.name,
      location: item.location,
    };
    this.isEditMode = true;
    this.errorMessage.set('');
  }

  delete(id: number): void {
    this.departmentService.deleteDepartment(id).subscribe({
      next: () => this.loadDepartments(),
      error: (err) => this.errorMessage.set(err.error || 'Failed to delete department'),
    });
  }

  resetForm(): void {
    this.departmentModel = {
      id: 0,
      name: '',
      location: '',
    };
    this.isEditMode = false;
    this.errorMessage.set('');
  }
}
