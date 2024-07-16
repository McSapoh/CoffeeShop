import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProductTableComponent } from './components/product-table/product-table.component';
import { NavbarComponent } from './components/default-page-components/navbar/navbar.component';
import { SidebarComponent } from './components/default-page-components/sidebar/sidebar.component';
import { TitleComponent } from './components/default-page-components/title/title.component';
import { ProductPageComponent } from './pages/generic-pages/product-page/product-page.component';
import { CoffeesPageComponent } from './pages/product-pages/coffees-page/coffees-page.component';
import { DessertsPageComponent } from './pages/product-pages/desserts-page/desserts-page.component';


import { DataTablesModule } from 'angular-datatables';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MatTableModule } from '@angular/material/table'
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule } from '@angular/material/dialog'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';


import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component'
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { WrapperComponent } from './components/default-page-components/wrapper/wrapper.component';
import { SocialAuthComponent } from './components/auth/social-auth/social-auth.component'
import { AuthWrapperComponent } from './components/auth/auth-wrapper/auth-wrapper.component';
import { ProductPopupComponent } from './components/popups/product-popup/product-popup.component';
import { ToastrModule } from 'ngx-toastr';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { InputComponent } from './components/default-page-components/input/input.component';

@NgModule({
  declarations: [
    AppComponent,
    ProductTableComponent,
    NavbarComponent,
    SidebarComponent,
    TitleComponent,
    ProductPageComponent,
    CoffeesPageComponent,
    DessertsPageComponent,
    LoginPageComponent,
    RegisterPageComponent,
    WrapperComponent,
    SocialAuthComponent,
    AuthWrapperComponent,
    ProductPopupComponent,
    InputComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    DataTablesModule,
    BrowserAnimationsModule,
    MatTableModule, 
    MatPaginatorModule,
    MatDialogModule,
    MatInputModule,
    ReactiveFormsModule,
    FormsModule,
    ToastrModule.forRoot()
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }


/*
"node_modules/admin-lte/plugins/datatables/jquery.dataTables.min.js",
"node_modules/admin-lte/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js",
"node_modules/admin-lte/plugins/datatables-responsive/js/dataTables.responsive.min.js",
"node_modules/admin-lte/plugins/datatables-responsive/js/responsive.bootstrap4.min.js",
"node_modules/admin-lte/plugins/datatables-buttons/js/dataTables.buttons.min.js",
"node_modules/admin-lte/plugins/datatables-buttons/js/buttons.bootstrap4.min.js",
"node_modules/admin-lte/plugins/jszip/jszip.min.js",
"node_modules/admin-lte/plugins/pdfmake/pdfmake.min.js",
"node_modules/admin-lte/plugins/pdfmake/vfs_fonts.js",
"node_modules/admin-lte/plugins/datatables-buttons/js/buttons.html5.min.js",
"node_modules/admin-lte/plugins/datatables-buttons/js/buttons.print.min.js",
"node_modules/admin-lte/plugins/datatables-buttons/js/buttons.colVis.min.js",
*/
