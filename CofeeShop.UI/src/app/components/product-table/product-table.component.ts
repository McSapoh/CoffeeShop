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



@Component({
  selector: 'app-product-table',
  templateUrl: './product-table.component.html'
})
export class ProductTableComponent {
  title = 'CofeeShop.UI';
  products: DisplayProductDTO[] = []
  defaultImageUrl = environment.apiUrl + '/images'
  path = 'https://localhost:44300/api/images/DefaultCoffeeImage.png'

  // datatable
  displayTable: boolean = false;
  dtOptions: DataTables.Settings = {};


  // material
  displayedColumns: string[] = ['name', 'description', 'imagePath', 'sizes', 'isConfirmed', 'edit']
  dataSource = new MatTableDataSource(this.products)
  @ViewChild(MatPaginator) paginator!: MatPaginator
  pageSizes = [3, 5, 10];
  totalRecords: number = 0;


  @Input()
  public productType: string = ''


  constructor(private service: ProductService, private dialogRef: MatDialog) {}

  ngOnInit(): void {
    this.totalRecords = 0;

    this.dataSource.paginator = this.paginator
    /*this.service.getProducts(this.productType)
      .subscribe(
        (result: DisplayProductDTO[]) => {
          this.products = result
          this.displayTable = true;


          this.dataSource = new MatTableDataSource(this.products)


          this.paginator.pageSizeOptions = this.pageSizes
          this.paginator.length = this.totalRecordsData
          this.dataSource.paginator = this.paginator
        }
      )*/

    this.refreshData()
  }

  refreshData(currentPage?: number, pageSize?: number): void{
    if (currentPage == undefined || pageSize == undefined) {
      currentPage = 1
      pageSize = this.pageSizes[0]
    }
    this.service.getPagination(this.productType, currentPage, pageSize).subscribe(
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

  ngAfterViewInit() {
    this.paginator.page.subscribe(page => {
      this.onPagination(page)
    })
  }
  
  onPagination(page: PageEvent) {
    this.refreshData(page.pageIndex + 1, page.pageSize)
  }
}
