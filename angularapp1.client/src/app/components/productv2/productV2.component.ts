import { Component } from '@angular/core';
import { GetProducts } from '../../GetProducts';
import { Product } from '../../product';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { OnInit } from '@angular/core';
import { ProductService } from '../../product.service';

@Component({
  selector: 'app-productV2',
  templateUrl: './productV2.component.html',
  styleUrl: './productV2.component.css'
})
export class ProductV2Component implements OnInit {
  data: any;
  private apiUrl = 'https://localhost:7285/api/v2/Products';
  private version = 'v2';
  Products: Product[] = []
  GetProducts: GetProducts[] = []
  constructor(private ProductService: ProductService, private http: HttpClient) { }
  ngOnInit(): void {
    this.getProducts();

    console.log(this.getProducts());
  }

  getData(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
  getProducts(): void {
    this.ProductService.getProducts(this.version)
      .subscribe(Producte => {
        this.GetProducts = Producte;
        console.log(this.GetProducts)
      });
  }
  add(productName: string, productQuantity: number, productEan: string): void {
    const name = productName.trim();
    const quantity = productQuantity;
    const ean = productEan.trim();

    if (name == "" && ean == "" && quantity == 0) { return; }

    const newProduct: Product = { name, quantity, ean };
    this.ProductService.addProduct(newProduct, this.version)
      .subscribe((product: Product) => {
        this.Products.push(product);
        this.getProducts();
      }
      )
  }
  delete(productId?: number): void {
    if (productId != undefined) {
      this.ProductService.deleteProduct(productId, this.version).subscribe(_ => this.getProducts());
    }
  }
  update(id: number | undefined, productName: string, productQuantity: number, productEan: string): void {
    if (id === undefined) {
      console.error('Product ID is undefined');
      return; // Exit if ID is not defined
    }

    const name = productName.trim();
    const quantity = productQuantity;
    const ean = productEan.trim();

    if (name == "" && ean == "" && quantity == 0) { return; }

    const newProduct: GetProducts = { id, name, quantity, ean };
    console.log(newProduct)
    this.ProductService.updateProduct(newProduct, this.version)
      .subscribe((updatedProduct: GetProducts) => {
        //const index = this.GetProducts.findIndex(product => product.id === updatedProduct.id);
        //if (index !== -1) {
        //  this.GetProducts[index] = updatedProduct; // Update the existing product
        //}
        this.getProducts();
      });
  }
}
