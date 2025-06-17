import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiConfigService {
  private baseUrl = 'API_URL_PLACEHOLDER';

  get authUrl(): string {
    return `${this.baseUrl}/api/login`
  }

  get companiesUrl(): string {
    return `${this.baseUrl}/api/companies`
  }
}
