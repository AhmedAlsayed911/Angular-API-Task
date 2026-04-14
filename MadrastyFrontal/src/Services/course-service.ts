import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ICourse } from '../Models/icourse';
import { IAddCourse } from '../Models/icourse-add';
import { IUpdateCourse } from '../Models/icourse-update';
import { IDepartmentCourseAction } from '../Models/idepartment-course-action';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private httpClient = inject(HttpClient);
  private url = 'https://localhost:7055/api/Course/';

  getAllCourses(): Observable<ICourse[]> {
    return this.httpClient.get<ICourse[]>(this.url + 'GetAllCourses');
  }

  addCourse(payload: IAddCourse): Observable<ICourse> {
    return this.httpClient.post<ICourse>(this.url + 'AddCourse', payload);
  }

  updateCourse(id: number, payload: IUpdateCourse): Observable<ICourse> {
    return this.httpClient.put<ICourse>(this.url + `UpdateCourse/${id}`, payload);
  }

  deleteCourse(id: number): Observable<string> {
    return this.httpClient.delete(this.url + `DeleteCourse/${id}`, { responseType: 'text' });
  }

  getCoursesByDepartmentId(departmentId: number): Observable<ICourse[]> {
    return this.httpClient.get<ICourse[]>(this.url + 'GetCoursesByDepartmentId', {
      params: { deptId: departmentId },
    });
  }

  addCourseToDepartment(payload: IDepartmentCourseAction): Observable<string> {
    return this.httpClient.post(this.url + 'AddCourseToDepartment', null, {
      params: {
        id: payload.departmentId,
        courseId: payload.courseId,
      },
      responseType: 'text',
    });
  }

  deleteCourseFromDepartment(payload: IDepartmentCourseAction): Observable<string> {
    return this.httpClient.delete(this.url + 'DeleteCourseFromDepartment', {
      params: {
        id: payload.departmentId,
        courseId: payload.courseId,
      },
      responseType: 'text',
    });
  }

  addCourseToStudent(studentId: number, courseId: number): Observable<string> {
    return this.httpClient.post(this.url + 'AddCourseToStudent', null, {
      params: {
        studentId,
        courseId,
      },
      responseType: 'text',
    });
  }
}
