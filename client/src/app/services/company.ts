import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Company } from '../models.company';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = 'http://localhost:5202/api/companies';
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getCompanies(): Observable<Company[]> {
    return this.http.get<Company[]>(this.apiUrl);
  }

  getCompanyById(id: number): Observable<Company> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Company>(url);
  }

  getCompanyByIsin(isin: string): Observable<Company> {
    const url = `${this.apiUrl}/isin/${isin}`;
    return this.http.get<Company>(url);
  }

  addCompany(company: Company): Observable<Company> {
    return this.http.post<Company>(this.apiUrl, company, this.httpOptions);
  }

  updateCompany(company: Company): Observable<Company> {
    const url = `${this.apiUrl}/${company.id}`;
    return this.http.put<Company>(url, company, this.httpOptions);
  }
}
