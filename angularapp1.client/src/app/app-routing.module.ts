import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductV1Component } from './components/products/productV1.component';
import { LoginComponent } from './components/login/login.component';
import { ProductV2Component } from './components/productv2/productV2.component';
import { ProductVersionsComponent } from './components/product-versions/product-versions.component';
import { AuthGuard } from './auth.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'productV1', component: ProductV1Component, canActivate: [AuthGuard] },
  { path: 'productV2', component: ProductV2Component, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'productVersion', component: ProductVersionsComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
