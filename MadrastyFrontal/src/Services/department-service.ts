import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IDepartment } from '../Models/idepartment';
import { IAddDepartment } from '../Models/idepartment-add';
import { IUpdateDepartment } from '../Models/idepartment-update';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private httpClient = inject(HttpClient);
  private url = 'https://localhost:7055/api/Department/';

  getAllDepartments(): Observable<IDepartment[]> {
    return this.httpClient.get<IDepartment[]>(this.url + 'GetAllDepartments');
  }

  getDepartmentById(id: number): Observable<IDepartment> {
    return this.httpClient.get<IDepartment>(this.url + `GetDepartmentById/${id}`);
  }

  addDepartment(payload: IAddDepartment): Observable<IDepartment> {
    return this.httpClient.post<IDepartment>(this.url + 'AddDepartment', payload);
  }

  updateDepartment(id: number, payload: IUpdateDepartment): Observable<IDepartment> {
    return this.httpClient.put<IDepartment>(this.url + `UpdateDepartment/${id}`, payload);
  }

  deleteDepartment(id: number): Observable<string> {
    return this.httpClient.delete(this.url + `DeleteDepartment/${id}`, { responseType: 'text' });
  }
}
