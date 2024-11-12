import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-versions',
  templateUrl: './product-versions.component.html',
  styleUrl: './product-versions.component.css'
})
export class ProductVersionsComponent {
  constructor(private router: Router) { }
  RedirectV1(): void {
    this.router.navigate(['/productV1']);
  }
  RedirectV2(): void {
    this.router.navigate(['/productV2']);
  }
}
