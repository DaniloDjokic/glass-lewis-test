import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Company } from '../models.company';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = 'https://localhost:5202/api/companies';
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  private mockCompanies: Company[] = [
    { id: 1, name: 'Company A', exchange: 'NYSE', ticker: 'CMPA', isin: 'US1234567890', websiteUrl: 'https://companya.com' },
    { id: 2, name: 'Company B', exchange: 'NASDAQ', ticker: 'CMPB', isin: 'US0987654321', websiteUrl: 'https://companyb.com' },
    { id: 3, name: 'Company C', exchange: 'LSE', ticker: 'CMPC', isin: 'GB1234567890', websiteUrl: 'https://companyc.co.uk' }
  ];

  constructor(private http: HttpClient) { }

  getCompanies(): Observable<Company[]> {
    return of([...this.mockCompanies]);
    // return this.http.get<Company[]>(this.apiUrl);
  }

  getCompanyById(id: number): Observable<Company> {
    const company = this.mockCompanies.find(p => p.id === id);
    return of(company!);
    // const url = `${this.apiUrl}/${id}`;
    // return this.http.get<Company>(url);
  }

  getCompanyByIsin(isin: string): Observable<Company> {
    const company = this.mockCompanies.find(p => p.isin === isin);
    return of(company!);
    // const url = `${this.apiUrl}/${isin}`;
    // return this.http.get<Company>(url);
  }

  addCompany(company: Company): Observable<Company> {
    const maxId = this.mockCompanies.length > 0 ? Math.max(...this.mockCompanies.map(c => c.id)) : 0;
    const newCompany = { ...company, id: maxId + 1 };

    this.mockCompanies.push(newCompany);

    return of({ ...newCompany }); // Return a copy
    // return this.http.post<Company>(this.apiUrl, company, this.httpOptions);
  }

  updateCompany(company: Company): Observable<Company> {
    const index = this.mockCompanies.findIndex(p => p.id === company.id);

    this.mockCompanies[index] = { ...company };

    return of({ ...company }); // Return a copy
    // const url = `${this.apiUrl}/${company.id}`;
    // return this.http.put<Company>(url, company, this.httpOptions);
  }
}
