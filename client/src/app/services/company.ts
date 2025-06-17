import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Company } from '../models.company';
import { ApiConfigService } from './api-config-service';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) { }

  getCompanies(): Observable<Company[]> {
    return this.http.get<Company[]>(this.apiConfig.companiesUrl);
  }

  getCompanyById(id: number): Observable<Company> {
    const url = `${this.apiConfig.companiesUrl}/${id}`;
    return this.http.get<Company>(url);
  }

  getCompanyByIsin(isin: string): Observable<Company> {
    const url = `${this.apiConfig.companiesUrl}/isin/${isin}`;
    return this.http.get<Company>(url);
  }

  addCompany(company: Company): Observable<Company> {
    return this.http.post<Company>(this.apiConfig.companiesUrl, company, this.httpOptions);
  }

  updateCompany(company: Company): Observable<Company> {
    const url = `${this.apiConfig.companiesUrl}/${company.id}`;
    return this.http.put<Company>(url, company, this.httpOptions);
  }
}
