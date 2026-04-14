import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { StudentService } from '../../Services/student-service';
import { AuthService } from '../../Services/auth-service';
import { CourseService } from '../../Services/course-service';
import { DepartmentService } from '../../Services/department-service';
import { IStudent } from '../../Models/istudent';
import { IAddStudent } from '../../Models/istudent-add';
import { IUpdateStudent } from '../../Models/istudent-update';
import { IStudentGradeUpdate } from '../../Models/istudent-grade-update';
import { IStudentGrade } from '../../Models/istudent-grade';
import { ICourse } from '../../Models/icourse';
import { IDepartment } from '../../Models/idepartment';

@Component({
  selector: 'app-student',
  imports: [FormsModule, RouterLink],
  templateUrl: './student.html',
  styleUrl: './student.css',
})
export class Student implements OnInit {
  private studentService = inject(StudentService);
  private courseService = inject(CourseService);
  private departmentService = inject(DepartmentService);
  authService = inject(AuthService);
  Students = signal<IStudent[]>([]);
  courses = signal<ICourse[]>([]);
  departments = signal<IDepartment[]>([]);
  errorMessage = signal('');
  isPrivilegedUser = computed(() => this.authService.isAdminOrInstructor());

  studentModel: IAddStudent & IUpdateStudent = {
    id: 0,
    name: '',
    address: '',
    age: 1,
    departmentId: 1,
  };
  isEditMode = false;

  gradeModel: IStudentGradeUpdate = {
    studentId: 0,
    courseId: 0,
    grade: 1,
  };
  selectedStudentName = signal('');
  selectedStudentCourses = signal<IStudentGrade[]>([]);
  isAssignGradeModalOpen = signal(false);
  isAssignCourseModalOpen = signal(false);

  courseAssignmentModel = {
    studentId: 0,
    courseId: 0,
  };

  ngOnInit(): void {
    if (this.isPrivilegedUser()) {
      this.loadStudents();
      this.loadCourses();
      this.loadDepartments();
    }
  }

  loadCourses(): void {
    this.courseService.getAllCourses().subscribe({
      next: (res) => this.courses.set(res),
      error: (err) => this.errorMessage.set(err.error || 'Failed to load courses'),
    });
  }

  loadStudents(): void {
    this.studentService.GetAllStudents().subscribe({
      next: (res) => this.Students.set(res),
      error: (err) => this.errorMessage.set(err.error || 'Failed to load students'),
    });
  }

  loadDepartments(): void {
    this.departmentService.getAllDepartments().subscribe({
      next: (res) => this.departments.set(res),
      error: (err) => this.errorMessage.set(err.error || 'Failed to load departments'),
    });
  }

  submit(): void {
    if (this.isEditMode) {
      this.studentService
        .UpdateStudent(this.studentModel.id, this.studentModel)
        .subscribe({
          next: () => {
            this.resetForm();
            this.loadStudents();
          },
          error: (err) => this.errorMessage.set(err.error || 'Failed to update student'),
        });
      return;
    }

    this.studentService
      .AddStudent(this.studentModel)
      .subscribe({
        next: () => {
          this.resetForm();
          this.loadStudents();
        },
        error: (err) => this.errorMessage.set(err.error || 'Failed to add student'),
      });
  }

  edit(item: IStudent): void {
    this.studentModel = {
      id: item.id,
      name: item.fullName,
      address: item.address,
      age: item.age,
      departmentId: item.departmentId,
    };
    this.isEditMode = true;
    this.errorMessage.set('');
  }

  delete(id: number): void {
    this.studentService.DeleteStudent(id).subscribe({
      next: () => this.loadStudents(),
      error: (err) => this.errorMessage.set(err.error || 'Failed to delete student'),
    });
  }

  openAssignGradeModal(student: IStudent): void {
    const courses = student.grades ?? [];

    this.gradeModel = {
      studentId: student.id,
      courseId: courses.length ? courses[0].courseId : 0,
      grade: courses.length ? courses[0].grade : 1,
    };

    this.selectedStudentName.set(student.fullName);
    this.selectedStudentCourses.set(courses);
    this.isAssignGradeModalOpen.set(true);
    this.errorMessage.set('');
  }

  openAssignCourseModal(student: IStudent): void {
    this.courseAssignmentModel = {
      studentId: student.id,
      courseId: this.courses().length ? this.courses()[0].id : 0,
    };
    this.isAssignCourseModalOpen.set(true);
    this.errorMessage.set('');
  }

  closeAssignGradeModal(): void {
    this.isAssignGradeModalOpen.set(false);
    this.selectedStudentName.set('');
    this.selectedStudentCourses.set([]);
  }

  closeAssignCourseModal(): void {
    this.isAssignCourseModalOpen.set(false);
    this.courseAssignmentModel = {
      studentId: 0,
      courseId: 0,
    };
  }

  submitGrade(): void {
    if (!this.gradeModel.courseId) {
      this.errorMessage.set('This student has no courses to grade yet.');
      return;
    }

    this.studentService.UpdateStudentGrade(this.gradeModel).subscribe({
      next: () => {
        this.closeAssignGradeModal();
        this.loadStudents();
        this.errorMessage.set('Grade updated successfully.');
      },
      error: (err) => this.errorMessage.set(err.error || 'Failed to update grade'),
    });
  }

  submitCourseAssignment(): void {
    if (!this.courseAssignmentModel.courseId) {
      this.errorMessage.set('Please choose a course first.');
      return;
    }

    this.courseService.addCourseToStudent(this.courseAssignmentModel.studentId, this.courseAssignmentModel.courseId).subscribe({
      next: () => {
        this.closeAssignCourseModal();
        this.loadStudents();
        this.errorMessage.set('Course assigned successfully.');
      },
      error: (err) => this.errorMessage.set(err.error || 'Failed to assign course'),
    });
  }

  resetForm(): void {
    this.studentModel = {
      id: 0,
      name: '',
      address: '',
      age: 1,
      departmentId: 1,
    };
    this.isEditMode = false;
    this.errorMessage.set('');
  }

}
