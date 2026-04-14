import { IStudentGrade } from './istudent-grade';

export interface IStudent {
    id: number;
    fullName: string;
    address: string;
    age: number;
    departmentId: number;
    departmentName: string;
    grades: IStudentGrade[];
}
