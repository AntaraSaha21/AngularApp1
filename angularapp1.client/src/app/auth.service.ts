import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginUrl = 'https://localhost:7285/api/Auth';

  constructor(private http: HttpClient, private router: Router) { }
  login(username: string, password: string) {
    return this.http.post<any>(`${this.loginUrl}/login`, { username, password })
      .subscribe(response => {
        localStorage.setItem('token', response.token);
        this.router.navigate(['/productVersion']);
      });
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  public get loggedIn(): boolean {
    return localStorage.getItem('token') !== null;
  }

}
