import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DepartmentService } from '../../Services/department-service';
import { CourseService } from '../../Services/course-service';
import { IDepartment } from '../../Models/idepartment';
import { ICourse } from '../../Models/icourse';
import { IAddCourse } from '../../Models/icourse-add';
import { IUpdateCourse } from '../../Models/icourse-update';

@Component({
  selector: 'app-department-details',
  imports: [FormsModule, RouterLink],
  templateUrl: './department-details.html',
  styleUrl: './department-details.css',
})
export class DepartmentDetails implements OnInit {
  private route = inject(ActivatedRoute);
  private departmentService = inject(DepartmentService);
  private courseService = inject(CourseService);

  department = signal<IDepartment | null>(null);
  courses = signal<ICourse[]>([]);
  allCourses = signal<ICourse[]>([]);
  errorMessage = signal('');
  successMessage = signal('');

  departmentId = 0;
  selectedCourseToAttachId = 0;

  courseModel: IAddCourse & IUpdateCourse = {
    id: 0,
    name: '',
    duration: 1,
    topicId: 1,
  };

  isEditMode = false;

  ngOnInit(): void {
    this.departmentId = Number(this.route.snapshot.paramMap.get('id'));
    this.courseModel.id = 0;
    this.loadDepartment();
    this.loadDepartmentCourses();
    this.loadAllCourses();
  }

  loadDepartment(): void {
    this.departmentService.getDepartmentById(this.departmentId).subscribe({
      next: (res) => this.department.set(res),
      error: (err) => this.errorMessage.set(err.error || 'Failed to load department'),
    });
  }

  loadDepartmentCourses(): void {
    this.courseService.getCoursesByDepartmentId(this.departmentId).subscribe({
      next: (res) => {
        this.courses.set(res);
        if (!this.selectedCourseToAttachId && res.length > 0) {
          this.selectedCourseToAttachId = res[0].id;
        }
      },
      error: (err) => this.errorMessage.set(err.error || 'Failed to load department courses'),
    });
  }

  loadAllCourses(): void {
    this.courseService.getAllCourses().subscribe({
      next: (res) => {
        this.allCourses.set(res);
        if (!this.selectedCourseToAttachId && res.length > 0) {
          this.selectedCourseToAttachId = res[0].id;
        }
      },
      error: (err) => this.errorMessage.set(err.error || 'Failed to load courses'),
    });
  }

  attachCourse(): void {
    if (!this.selectedCourseToAttachId) {
      this.errorMessage.set('Choose a course first');
      return;
    }

    this.clearMessages();
    this.courseService.addCourseToDepartment({
      departmentId: this.departmentId,
      courseId: this.selectedCourseToAttachId,
    }).subscribe({
      next: (res) => {
        this.successMessage.set(res);
        this.loadDepartmentCourses();
      },
      error: (err) => this.errorMessage.set(err.error || 'Failed to add course to department'),
    });
  }

  removeCourse(courseId: number): void {
    this.clearMessages();
    this.courseService.deleteCourseFromDepartment({
      departmentId: this.departmentId,
      courseId,
    }).subscribe({
      next: (res) => {
        this.successMessage.set(res);
        this.loadDepartmentCourses();
      },
      error: (err) => this.errorMessage.set(err.error || 'Failed to remove course from department'),
    });
  }

  editCourse(course: ICourse): void {
    this.courseModel = {
      id: course.id,
      name: course.name,
      duration: course.duration,
      topicId: this.courseModel.topicId,
    };
    this.isEditMode = true;
    this.clearMessages();
  }

  submitCourse(): void {
    if (this.isEditMode) {
      this.courseService.updateCourse(this.courseModel.id, this.courseModel).subscribe({
        next: () => {
          this.successMessage.set('Course updated successfully');
          this.resetCourseForm();
          this.loadDepartmentCourses();
          this.loadAllCourses();
        },
        error: (err) => this.errorMessage.set(err.error || 'Failed to update course'),
      });
      return;
    }

    this.courseService.addCourse(this.courseModel).subscribe({
      next: (res) => {
        this.successMessage.set('Course created successfully');
        this.courseModel = {
          id: 0,
          name: '',
          duration: 1,
          topicId: 1,
        };
        this.selectedCourseToAttachId = res.id;
        this.loadAllCourses();
      },
      error: (err) => this.errorMessage.set(err.error || 'Failed to create course'),
    });
  }

  resetCourseForm(): void {
    this.courseModel = {
      id: 0,
      name: '',
      duration: 1,
      topicId: 1,
    };
    this.isEditMode = false;
  }

  private clearMessages(): void {
    this.errorMessage.set('');
    this.successMessage.set('');
  }
}
