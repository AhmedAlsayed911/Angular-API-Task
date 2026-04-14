import { Routes } from '@angular/router';
import { Student } from '../Components/student/student';
import { Course } from '../Components/course/course';
import { DepartmentDetails } from '../Components/department-details/department-details';
import { Login } from '../Components/login/login';
import { Register } from '../Components/register/register';
import { authGuard } from '../Guards/auth.guard';
import { Department } from '../Components/department/department';
import { StudentGrades } from '../Components/student-grades/student-grades';

export const routes: Routes = [
	{ path: '', pathMatch: 'full', redirectTo: 'students' },
	{ path: 'login', component: Login },
	{ path: 'register', component: Register },
	{ path: 'students', component: Student, canActivate: [authGuard] },
	{ path: 'students/:id/grades', component: StudentGrades, canActivate: [authGuard] },
	{ path: 'courses', component: Course, canActivate: [authGuard] },
	{ path: 'departments', component: Department, canActivate: [authGuard] },
	{ path: 'departments/:id', component: DepartmentDetails, canActivate: [authGuard] },
	{ path: '**', redirectTo: 'students' },
];
