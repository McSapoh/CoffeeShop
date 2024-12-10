import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DessertsPageComponent } from './pages/product-pages/desserts-page/desserts-page.component';
import { CoffeesPageComponent } from './pages/product-pages/coffees-page/coffees-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { AuthGuard } from './auth.guard';
import { AuthCallbackComponent } from './components/auth/auth-callback/auth-callback.component';
import { SandwichesPageComponent } from './pages/product-pages/sandwiches-page/sandwiches-page.component';
import { TeasPageComponent } from './pages/product-pages/teas-page/teas-page.component';
import { SnacksPageComponent } from './pages/product-pages/snacks-page/snacks-page.component';
import { AlcoholPageComponent } from './pages/ingredient-pages/alcohol-page/alcohol-page.component';

const routes: Routes = [
  { path: '', component: CoffeesPageComponent, canActivate: [AuthGuard] },
  // products
  { path: 'Coffees', component: CoffeesPageComponent, canActivate: [AuthGuard], title: 'Coffees'},
  { path: 'Desserts', component: DessertsPageComponent, canActivate: [AuthGuard], title: 'Desserts' },
  { path: 'Sandwiches', component: SandwichesPageComponent, canActivate: [AuthGuard], title: 'Sandwiches' },
  { path: 'Snacks', component: SnacksPageComponent, canActivate: [AuthGuard], title: 'Snacks' },
  { path: 'Tea', component: TeasPageComponent, canActivate: [AuthGuard], title: 'Tea' },
  // ingredients
  { path: 'Alcohol', component: AlcoholPageComponent, canActivate: [AuthGuard], title: 'Alcohols' },


  { path: 'Login', component: LoginPageComponent },
  { path: 'Register', component: RegisterPageComponent },
  { path: 'auth/callback', component: AuthCallbackComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
