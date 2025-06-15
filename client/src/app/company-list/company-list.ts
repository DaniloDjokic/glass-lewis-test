import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Company } from '../models.company';
import { CompanyService } from '../services/company';
import { HttpErrorResponse } from '@angular/common/http';

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
    MatInputModule,
    MatSnackBarModule
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

  constructor(private companyService: CompanyService, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.getCompanies();
  }

  getCompanies(): void {
    this.companyService.getCompanies()
      .subscribe({
        next: companies => this.companies = companies,
        error: (error) => this.handleError(error, 'Failed to load companies')
      });
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
          this.showSuccessMessage('Company updated successfully');
          this.cancelForm();
        },
        error: (error) => this.handleError(error, 'Failed to update company')
      });
    } else {
      this.companyService.addCompany(this.currentCompany).subscribe({
        next: (newCompany) => {
          if (newCompany && newCompany.id) {
            this.companies = [...this.companies, newCompany];
            this.showSuccessMessage('Company created successfully');
            this.cancelForm();
          } else {
            this.fetchAndAddNewCompany();
          }
        },
        error: (error) => this.handleError(error, 'Failed to create company')
      });
    }
  }

  private fetchAndAddNewCompany(): void {
    if (this.currentCompany.isin) {
      this.companyService.getCompanyByIsin(this.currentCompany.isin).subscribe({
        next: (company) => {
          this.companies = [...this.companies, company];
          this.showSuccessMessage('Company created successfully');
          this.cancelForm();
        },
        error: (error) => {
          this.handleError(error, 'Error fetching created company');
          this.refreshCompanyList();
        }
      });
    } else {
      this.refreshCompanyList();
    }
  }

  private refreshCompanyList(): void {
    this.companyService.getCompanies().subscribe({
      next: (companies) => {
        this.companies = companies;
        this.showSuccessMessage('Company created successfully');
        this.cancelForm();
      },
      error: (error) => this.handleError(error, 'Error refreshing company list')
    });
  }

  private handleError(error: HttpErrorResponse, defaultMessage: string): void {
    let errorMessage = defaultMessage;

    if (error.error) {
      if (typeof error.error === 'string') {
        errorMessage = error.error;
      } else if (error.error.message) {
        errorMessage = error.error.message;
      } else if (error.error.title) {
        errorMessage = error.error.title;
      } else if (error.error.errors) {
        const validationErrors = Object.values(error.error.errors).flat();
        errorMessage = validationErrors.join(', ');
      }
    } else if (error.message) {
      errorMessage = error.message;
    }

    if (error.status) {
      errorMessage = `${errorMessage} (Status: ${error.status})`;
    }

    this.showErrorMessage(errorMessage);
  }

  private showErrorMessage(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 6000,
      panelClass: ['error-snackbar'],
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }

  private showSuccessMessage(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      panelClass: ['success-snackbar'],
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
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
