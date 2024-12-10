import { Component, Input, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table'
import { MatPaginator, PageEvent } from '@angular/material/paginator'
import { PaginatedResult } from 'src/app/models/pagination/pagination';
import { environment } from 'src/environments/environment.development';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { HttpErrorResponse } from '@angular/common/http';
import { DisplayIngredientDTO } from 'src/app/models/ingredients/display-ingredient';
import { IngredientService } from 'src/app/services/ingredient/ingredient.service';
import { IngredientPopupComponent } from '../popups/ingredient-popup/ingredient-popup.component';

@Component({
  selector: 'app-ingredient-table',
  templateUrl: './ingredient-table.component.html',
})
export class IngredientTableComponent {
  itle = 'CofeeShop.UI';
  products: DisplayIngredientDTO[] = []
  defaultImageUrl = environment.apiUrl + '/images'
  currentPage?: number
  pageSize?: number
  
  // datatable
  displayTable: boolean = false;
  dtOptions: DataTables.Settings = {};


  // material
  displayedColumns: string[] = ['name', 'price', 'edit']
  dataSource = new MatTableDataSource(this.products)
  @ViewChild(MatPaginator) paginator!: MatPaginator
  pageSizes = [10, 25, 50];
  totalRecords: number = 0;


  @Input()
  public ingredientType: string = ''


  constructor(private service: IngredientService, private dialogRef: MatDialog) {}

  ngOnInit(): void {
    this.totalRecords = 0;

    this.dataSource.paginator = this.paginator
    this.service.refreshTable$.subscribe(() => {
      this.refreshData(this.currentPage, this.pageSize);
    });
    this.refreshData()
  }

  refreshData(currentPage?: number, pageSize?: number): void{
    this.currentPage = currentPage
    this.pageSize = pageSize
    if (this.currentPage == undefined || this.pageSize == undefined) {
      this.currentPage = 1
      this.pageSize = this.pageSizes[0]
    }
    this.service.getPagination(this.ingredientType, this.currentPage, this.pageSize).subscribe(
      (result: PaginatedResult<DisplayIngredientDTO[]>) => {
        console.log(result);

        this.products = result.result

        
        this.displayTable = true;


        this.dataSource = new MatTableDataSource(this.products)

        this.totalRecords = result.pagination.TotalCount
      }
    )
  }
  delete(id: number): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'You won\'t be able to revert this!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, cancel!',
    }).then((result) => {
      if (result.isConfirmed) {
        this.service.delete(this.ingredientType, id).subscribe((res: string) => {
          this.service.triggerRefreshTable();
          Swal.fire('Deleted!', 'Your item has been deleted.', 'success');
          
        this.refreshData(this.currentPage, this.pageSize)
        }, ((error: HttpErrorResponse) => {
          if (error.status == 404) {
            Swal.fire('Cancelled', 'Element with this id is not found', 'info');
          } else if (error.status == 409) {
            Swal.fire('Cancelled', 'Item is already deleted', 'info');
          }else if (error.status == 500) {
            Swal.fire('Cancelled', 'Error while deleting', 'info');
          } else {
            Swal.fire('Cancelled', error.message, 'info');
          }
          console.log(error);
        })
      )
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        Swal.fire('Cancelled', 'Your item is safe :)', 'info');
      }
    });
  }
  openEditPopup(id: number) {
    console.log(id);
    let getProductResult: DisplayIngredientDTO = new DisplayIngredientDTO()
    this.service.getIngredient(this.ingredientType, id).subscribe(res => {
      getProductResult = res
      console.log('openEditPopup sub', res);
      this.dialogRef.open(IngredientPopupComponent, {
        maxHeight: '100vh',   //width: 'inherit', 
        data: {product: getProductResult, ingredientType: this.ingredientType}})
    })
  }
  openCreatePopup() {
    console.log();
    let getProductResult: DisplayIngredientDTO = new DisplayIngredientDTO()
    console.log('openCreatePopup sub', getProductResult);
    this.dialogRef.open(IngredientPopupComponent, {
      height: 'auto',   //width: 'inherit', 
      data: {product: getProductResult, ingredientType: this.ingredientType}
    })
  }
  ngAfterViewInit() {
    this.paginator.page.subscribe(page => {
      this.onPagination(page)
    })
  }
  
  onPagination(page: PageEvent) {
    this.refreshData(page.pageIndex + 1, page.pageSize)
  }
}
