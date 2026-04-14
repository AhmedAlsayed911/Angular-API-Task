import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CourseService } from '../../Services/course-service';
import { ICourse } from '../../Models/icourse';
import { IAddCourse } from '../../Models/icourse-add';
import { IUpdateCourse } from '../../Models/icourse-update';
import { TopicService } from '../../Services/topic-service';
import { ITopic } from '../../Models/itopic';

@Component({
  selector: 'app-course',
  imports: [FormsModule],
  templateUrl: './course.html',
  styleUrl: './course.css',
})
export class Course implements OnInit {
  private courseService = inject(CourseService);
  private topicService = inject(TopicService);

  courses = signal<ICourse[]>([]);
  topics = signal<ITopic[]>([]);
  errorMessage = signal('');

  courseModel: IAddCourse & IUpdateCourse = {
    id: 0,
    name: '',
    duration: 1,
    topicId: 1,
  };
  isEditMode = false;

  ngOnInit(): void {
    this.loadCourses();
    this.loadTopics();
  }

  loadTopics(): void {
    this.topicService.getAllTopics().subscribe({
      next: (res) => this.topics.set(res),
      error: (err) => this.errorMessage.set(err.error || 'Failed to load topics'),
    });
  }

  loadCourses(): void {
    this.courseService.getAllCourses().subscribe({
      next: (res) => this.courses.set(res),
      error: (err) => this.errorMessage.set(err.error || 'Failed to load courses'),
    });
  }

  submit(): void {
    if (this.isEditMode) {
      this.courseService
        .updateCourse(this.courseModel.id, this.courseModel)
        .subscribe({
          next: () => {
            this.resetForm();
            this.loadCourses();
          },
          error: (err) => this.errorMessage.set(err.error || 'Failed to update course'),
        });
      return;
    }

    this.courseService
      .addCourse(this.courseModel)
      .subscribe({
        next: () => {
          this.resetForm();
          this.loadCourses();
        },
        error: (err) => this.errorMessage.set(err.error || 'Failed to add course'),
      });
  }

  edit(item: ICourse): void {
    this.courseModel = {
      id: item.id,
      name: item.name,
      duration: item.duration,
      topicId: item.topicId,
    };
    this.isEditMode = true;
    this.errorMessage.set('');
  }

  delete(id: number): void {
    this.courseService.deleteCourse(id).subscribe({
      next: () => this.loadCourses(),
      error: (err) => this.errorMessage.set(err.error || 'Failed to delete course'),
    });
  }

  resetForm(): void {
    this.courseModel = {
      id: 0,
      name: '',
      duration: 1,
      topicId: 1,
    };
    this.isEditMode = false;
    this.errorMessage.set('');
  }
}
