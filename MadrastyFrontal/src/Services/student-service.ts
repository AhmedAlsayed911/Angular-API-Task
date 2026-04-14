import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IStudent } from '../Models/istudent';
import { IAddStudent } from '../Models/istudent-add';
import { IUpdateStudent } from '../Models/istudent-update';
import { IStudentGrade } from '../Models/istudent-grade';
import { IStudentGradeUpdate } from '../Models/istudent-grade-update';

@Injectable({
  providedIn: 'root',
})
export class StudentService {

  private httpClient = inject(HttpClient);
  private url = 'https://localhost:7055/api/Student/';

  GetAllStudents(): Observable<IStudent[]> {
    return this.httpClient.get<IStudent[]>(this.url + 'GetAllStudents');
  }

  GetStudentById(id: number): Observable<IStudent> {
    return this.httpClient.get<IStudent>(this.url + `GetStudentById/${id}`);
  }

  AddStudent(payload: IAddStudent): Observable<IStudent> {
    return this.httpClient.post<IStudent>(this.url + 'AddStudent', payload);
  }

  UpdateStudent(id: number, payload: IUpdateStudent): Observable<IStudent> {
    return this.httpClient.put<IStudent>(this.url + `UpdateStudent/${id}`, payload);
  }

  GetStudentGrades(id: number): Observable<IStudentGrade[]> {
    return this.httpClient.get<IStudentGrade[]>(this.url + `GetStudentGrades/${id}`);
  }

  UpdateStudentGrade(payload: IStudentGradeUpdate): Observable<string> {
    return this.httpClient.put(this.url + 'UpdateStudentGrade', payload, { responseType: 'text' });
  }

  DeleteStudent(id: number): Observable<string> {
    return this.httpClient.delete(this.url + `DeleteStudent/${id}`, { responseType: 'text' });
  }
}
