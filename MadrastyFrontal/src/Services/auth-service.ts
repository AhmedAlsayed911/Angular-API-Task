import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';

interface LoginPayload {
  email: string;
  password: string;
}

interface RegisterPayload {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private httpClient = inject(HttpClient);
  private url = 'https://localhost:7055/api/Account/';
  private tokenKey = 'madrasty_token';
  private roleClaim = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
  private nameClaim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';

  isLoggedIn = signal<boolean>(!!localStorage.getItem(this.tokenKey));
  currentRoles = signal<string[]>(this.extractRoles(localStorage.getItem(this.tokenKey)));
  currentUserName = signal<string>(this.extractUserName(localStorage.getItem(this.tokenKey)));
  isAdminOrInstructor = computed(() => this.hasRole('Admin') || this.hasRole('Instructor'));

  login(payload: LoginPayload): Observable<string> {
    return this.httpClient.post(this.url + 'Login', payload, { responseType: 'text' }).pipe(
      tap((token) => {
        localStorage.setItem(this.tokenKey, token);
        this.isLoggedIn.set(true);
        this.currentRoles.set(this.extractRoles(token));
        this.currentUserName.set(this.extractUserName(token));
      })
    );
  }

  register(payload: RegisterPayload): Observable<string> {
    return this.httpClient.post(this.url + 'Register', payload, { responseType: 'text' });
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.isLoggedIn.set(false);
    this.currentRoles.set([]);
    this.currentUserName.set('');
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  hasRole(role: string): boolean {
    return this.currentRoles().some((r) => r.toLowerCase() === role.toLowerCase());
  }

  private decodeToken(token: string | null): Record<string, unknown> | null {
    if (!token) {
      return null;
    }

    const tokenParts = token.split('.');
    if (tokenParts.length < 2) {
      return null;
    }

    try {
      const base64 = tokenParts[1].replace(/-/g, '+').replace(/_/g, '/');
      const normalized = base64.padEnd(Math.ceil(base64.length / 4) * 4, '=');
      const payload = atob(normalized);
      return JSON.parse(payload) as Record<string, unknown>;
    } catch {
      return null;
    }
  }

  private extractRoles(token: string | null): string[] {
    const payload = this.decodeToken(token);
    if (!payload) {
      return [];
    }

    const roleValue = payload[this.roleClaim];
    if (Array.isArray(roleValue)) {
      return roleValue.filter((r): r is string => typeof r === 'string');
    }

    if (typeof roleValue === 'string') {
      return [roleValue];
    }

    return [];
  }

  private extractUserName(token: string | null): string {
    const payload = this.decodeToken(token);
    if (!payload) {
      return '';
    }

    const name = payload[this.nameClaim];
    return typeof name === 'string' ? name : '';
  }
}
