import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DessertsPageComponent } from './pages/product-pages/desserts-page/desserts-page.component';
import { CoffeesPageComponent } from './pages/product-pages/coffees-page/coffees-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { AuthGuard } from './auth.guard';
import { AuthCallbackComponent } from './components/auth/auth-callback/auth-callback.component';

const routes: Routes = [
  { path: '', component: CoffeesPageComponent, canActivate: [AuthGuard] },
  { path: 'Coffees', component: CoffeesPageComponent, canActivate: [AuthGuard] },
  { path: 'Desserts', component: DessertsPageComponent, canActivate: [AuthGuard] },
  { path: 'Login', component: LoginPageComponent },
  { path: 'Register', component: RegisterPageComponent },
  { path: 'auth/callback', component: AuthCallbackComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
