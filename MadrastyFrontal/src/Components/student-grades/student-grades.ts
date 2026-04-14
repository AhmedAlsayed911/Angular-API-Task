import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { StudentService } from '../../Services/student-service';
import { IStudent } from '../../Models/istudent';

@Component({
  selector: 'app-student-grades',
  imports: [RouterLink],
  templateUrl: './student-grades.html',
  styleUrl: './student-grades.css',
})
export class StudentGrades implements OnInit {
  private route = inject(ActivatedRoute);
  private studentService = inject(StudentService);

  student = signal<IStudent | null>(null);
  errorMessage = signal('');

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id || Number.isNaN(id)) {
      this.errorMessage.set('Invalid student id.');
      return;
    }

    this.studentService.GetStudentById(id).subscribe({
      next: (res) => this.student.set(res),
      error: (err) => this.errorMessage.set(err.error || 'Failed to load student grades'),
    });
  }
}
