import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

// Angular Material Modules
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { Company } from '../models.company';
import { CompanyService } from '../services/company';

@Component({
  selector: 'app-company-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './company-list.html',
  styleUrl: './company-list.css'
})
export class CompanyListComponent implements OnInit {
  companies: Company[] = [];
  displayedColumns: string[] = ['id', 'name', 'exchange', 'ticker', 'isin', 'websiteUrl', 'actions'];
  showForm = false;
  isEditing = false;
  currentCompany: Company = this.initializeCompany();

  constructor(private companyService: CompanyService) { }

  ngOnInit(): void {
    this.getCompanies();
  }

  getCompanies(): void {
    this.companyService.getCompanies()
      .subscribe(companies => this.companies = companies);
  }

  onAdd(): void {
    this.showForm = true;
    this.isEditing = false;
    this.currentCompany = this.initializeCompany();
  }

  onEdit(company: Company): void {
    this.showForm = true;
    this.isEditing = true;
    this.currentCompany = { ...company };
  }

  onSave(): void {
    if (this.isEditing) {
      this.companyService.updateCompany(this.currentCompany).subscribe({
        next: (updatedCompany) => {
          const index = this.companies.findIndex(c => c.id === this.currentCompany.id);
          if (index !== -1) {
            this.companies[index] = { ...this.currentCompany };
            this.companies = [...this.companies];
          }
          this.cancelForm();
        },
        error: (error) => console.error('Update error:', error)
      });
    } else {
      this.companyService.addCompany(this.currentCompany).subscribe({
        next: (newCompany) => {
          // Force re-render by creating a new array reference
          this.companies = [...this.companies, newCompany];
          this.cancelForm();
        },
        error: (error) => console.error('Add error:', error)
      });
    }
  }

  cancelForm(): void {
    this.showForm = false;
    this.currentCompany = this.initializeCompany();
  }

  private initializeCompany(): Company {
    return {
      id: 0,
      name: '',
      exchange: '',
      ticker: '',
      isin: '',
      websiteUrl: ''
    };
  }
}
