import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiConfigService {
  private readonly baseUrl = 'http://localhost:5202/api';

  get apiUrl(): string {
    return this.baseUrl;
  }

  getEndpoint(path: string): string {
    return `${this.baseUrl}/${path}`;
  }

  get companiesUrl(): string {
    return this.getEndpoint('companies');
  }

  get loginUrl(): string {
    return this.getEndpoint('login');
  }
}
