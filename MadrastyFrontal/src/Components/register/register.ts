import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../Services/auth-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private authService = inject(AuthService);
  private router = inject(Router);

  firstName = '';
  lastName = '';
  userName = '';
  email = '';
  password = '';
  message = signal('');
  errorMessage = signal('');

  register(): void {
    this.errorMessage.set('');
    this.message.set('');

    this.authService
      .register({
        firstName: this.firstName,
        lastName: this.lastName,
        userName: this.userName,
        email: this.email,
        password: this.password,
      })
      .subscribe({
        next: (res) => {
          this.message.set(res);
          this.router.navigate(['/login']);
        },
        error: (err) => {
          const errorText = typeof err.error === 'string' ? err.error : 'Register failed';
          this.errorMessage.set(errorText);
        },
      });
  }
}
