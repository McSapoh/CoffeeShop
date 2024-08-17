import { Component, Input, ViewChild, AfterViewInit } from '@angular/core';
import { DisplayProductDTO } from 'src/app/models/products/display-product';
import { ProductService } from 'src/app/services/product/product.service';
import {catchError, map, startWith, Subject, switchMap} from 'rxjs';
import { MatTableDataSource } from '@angular/material/table'
import { MatPaginator, PageEvent } from '@angular/material/paginator'
import { PaginatedResult } from 'src/app/models/pagination/pagination';
import { environment } from 'src/environments/environment.development';
import { MatDialog } from '@angular/material/dialog';
import { ProductPopupComponent } from '../popups/product-popup/product-popup.component';
import { Size } from "../../models/size"
import Swal from 'sweetalert2';
import { HttpErrorResponse } from '@angular/common/http';



@Component({
  selector: 'app-product-table',
  templateUrl: './product-table.component.html'
})
export class ProductTableComponent {
  title = 'CofeeShop.UI';
  products: DisplayProductDTO[] = []
  defaultImageUrl = environment.apiUrl + '/images'
  currentPage?: number
  pageSize?: number
  
  // datatable
  displayTable: boolean = false;
  dtOptions: DataTables.Settings = {};


  // material
  displayedColumns: string[] = ['name', 'description', 'imagePath', 'sizes', 'edit']
  dataSource = new MatTableDataSource(this.products)
  @ViewChild(MatPaginator) paginator!: MatPaginator
  pageSizes = [10, 25, 50];
  totalRecords: number = 0;


  @Input()
  public productType: string = ''


  constructor(private service: ProductService, private dialogRef: MatDialog) {}

  ngOnInit(): void {
    this.totalRecords = 0;

    this.dataSource.paginator = this.paginator
    this.service.refreshTable$.subscribe(() => {
      this.refreshData(this.currentPage, this.pageSize);
    });
    /*this.service.getProducts(this.productType)
      .subscribe(
        (result: DisplayProductDTO[]) => {
          this.products = result
          this.displayTable = true;


          this.dataSource = new MatTableDataSource(this.products)


          this.paginator.this.pageSizeOptions = this.this.pageSizes
          this.paginator.length = this.totalRecordsData
          this.dataSource.paginator = this.paginator
        }
      )*/

    this.refreshData()
  }

  refreshData(currentPage?: number, pageSize?: number): void{
    this.currentPage = currentPage
    this.pageSize = pageSize
    if (this.currentPage == undefined || this.pageSize == undefined) {
      this.currentPage = 1
      this.pageSize = this.pageSizes[0]
    }
    this.service.getPagination(this.productType, this.currentPage, this.pageSize).subscribe(
      (result: PaginatedResult<DisplayProductDTO[]>) => {
        console.log(result);

        this.products = result.result

        
        this.displayTable = true;


        this.dataSource = new MatTableDataSource(this.products)

        this.totalRecords = result.pagination.TotalCount
      }
    )


    /*
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 5,
      lengthMenu: [5, 50, 100],
      paging: true,
      searching: true,
      ordering: true,
      info: true,
    }


    this.service.getProducts(this.productType)
      .subscribe(
        (result: DisplayProductDTO[]) => {
          this.products = result
          this.displayTable = true;


          this.dataSource = new MatTableDataSource(this.products)
          this.dataSource.paginator = this.paginator
        }
      )
    */
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
        this.service.delete(this.productType, id).subscribe((res: string) => {
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
    let getProductResult: DisplayProductDTO = new DisplayProductDTO()
    this.service.getProduct(this.productType, id).subscribe(res => {
      getProductResult = res
      console.log('openEditPopup sub', res);
      this.dialogRef.open(ProductPopupComponent, {
         height: 'auto',   //width: 'inherit', 
        data: {product: getProductResult, productType: this.productType}})
    })
    // this.dialogRef.open(ProductPopupComponent, {data: {productType: this.productType, id: id }})
  }
  openCreatePopup() {
    console.log();
    let getProductResult: DisplayProductDTO = new DisplayProductDTO()
    console.log('openCreatePopup sub', getProductResult);
    this.dialogRef.open(ProductPopupComponent, {
      height: 'auto',   //width: 'inherit', 
      data: {product: getProductResult, productType: this.productType}
    })
    // this.dialogRef.open(ProductPopupComponent, {data: {productType: this.productType, id: id }})
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
