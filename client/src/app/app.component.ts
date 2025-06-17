import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, Router } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Glass Lewis Company Management';
  isAuthenticated = false;
  isLoading = true;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    const storedToken = localStorage.getItem('auth_token');
    if (storedToken) {
      console.log('Clearing potentially stale token on app construction');
      localStorage.removeItem('auth_token');
    }
  }

  ngOnInit() {
    this.authService.token$.subscribe(token => {
      this.isAuthenticated = !!token;
      this.isLoading = false;

      if (token) {
        if (this.router.url === '/login' || this.router.url === '/') {
          this.router.navigate(['/companies']);
        }
      } else {
        if (this.router.url !== '/login') {
          this.router.navigate(['/login']);
        }
      }
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
