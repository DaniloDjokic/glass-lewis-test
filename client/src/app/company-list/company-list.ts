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
import { MatSelectModule } from '@angular/material/select';
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
    MatSnackBarModule,
    MatSelectModule
  ],
  templateUrl: './company-list.html',
  styleUrl: './company-list.css'
})
export class CompanyListComponent implements OnInit {
  companies: Company[] = [];
  filteredCompanies: Company[] = [];
  displayedColumns: string[] = ['id', 'name', 'exchange', 'ticker', 'isin', 'websiteUrl', 'actions'];
  showForm = false;
  isEditing = false;
  currentCompany: Company = this.initializeCompany();

  // Search properties
  searchType: string = 'all';
  searchValue: string = '';
  showSearchResults = false;

  constructor(private companyService: CompanyService, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.getCompanies();
  }

  getCompanies(): void {
    this.companyService.getCompanies()
      .subscribe({
        next: companies => {
          this.companies = companies;
          this.filteredCompanies = companies;
          this.showSearchResults = false;
        },
        error: (error) => this.handleError(error, 'Failed to load companies')
      });
  }

  onSearch(): void {
    if (!this.searchValue.trim()) {
      this.showAllCompanies();
      return;
    }

    this.showSearchResults = true;

    switch (this.searchType) {
      case 'id':
        this.searchById();
        break;
      case 'isin':
        this.searchByIsin();
        break;
      case 'all':
      default:
        this.searchInAllCompanies();
        break;
    }
  }

  private searchById(): void {
    const id = parseInt(this.searchValue);
    if (isNaN(id)) {
      this.showErrorMessage('Please enter a valid ID number');
      return;
    }

    this.companyService.getCompanyById(id).subscribe({
      next: (company) => {
        this.filteredCompanies = [company];
        this.showSuccessMessage(`Found company with ID: ${id}`);
      },
      error: (error) => {
        this.filteredCompanies = [];
        this.handleError(error, `No company found with ID: ${id}`);
      }
    });
  }

  private searchByIsin(): void {
    const isin = this.searchValue.trim().toUpperCase();

    this.companyService.getCompanyByIsin(isin).subscribe({
      next: (company) => {
        this.filteredCompanies = [company];
        this.showSuccessMessage(`Found company with ISIN: ${isin}`);
      },
      error: (error) => {
        this.filteredCompanies = [];
        this.handleError(error, `No company found with ISIN: ${isin}`);
      }
    });
  }

  private searchInAllCompanies(): void {
    const searchTerm = this.searchValue.toLowerCase().trim();
    this.filteredCompanies = this.companies.filter(company =>
      company.name.toLowerCase().includes(searchTerm) ||
      company.exchange.toLowerCase().includes(searchTerm) ||
      company.ticker.toLowerCase().includes(searchTerm) ||
      company.isin.toLowerCase().includes(searchTerm) ||
      company.id.toString().includes(searchTerm) ||
      (company.websiteUrl && company.websiteUrl.toLowerCase().includes(searchTerm))
    );

    if (this.filteredCompanies.length === 0) {
      this.showErrorMessage(`No companies found matching: ${this.searchValue}`);
    } else {
      this.showSuccessMessage(`Found ${this.filteredCompanies.length} company(ies) matching: ${this.searchValue}`);
    }
  }

  onClearSearch(): void {
    this.searchValue = '';
    this.searchType = 'all';
    this.showAllCompanies();
  }

  private showAllCompanies(): void {
    this.filteredCompanies = this.companies;
    this.showSearchResults = false;
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
            // Update filtered results if currently showing search results
            if (this.showSearchResults) {
              const filteredIndex = this.filteredCompanies.findIndex(c => c.id === this.currentCompany.id);
              if (filteredIndex !== -1) {
                this.filteredCompanies[filteredIndex] = { ...this.currentCompany };
                this.filteredCompanies = [...this.filteredCompanies];
              }
            } else {
              this.filteredCompanies = [...this.companies];
            }
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
            if (!this.showSearchResults) {
              this.filteredCompanies = [...this.companies];
            }
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
          if (!this.showSearchResults) {
            this.filteredCompanies = [...this.companies];
          }
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
        if (!this.showSearchResults) {
          this.filteredCompanies = companies;
        }
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
