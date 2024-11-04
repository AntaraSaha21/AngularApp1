import { Component, OnInit } from '@angular/core';
import { Product } from '../../product';
import { ProductService } from '../../product.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { GetProducts } from '../../GetProducts';

@Component({
  selector: 'app-Products',
  templateUrl: './Products.component.html',
  styleUrl: './Products.component.css'
})
export class ProductsComponent implements OnInit {
  data: any;
  private apiUrl = 'https://localhost:7285/api/Products'; 
  Products: Product[] = []
  GetProducts: GetProducts[]=[]
  constructor(private ProductService: ProductService, private http: HttpClient) { }
  ngOnInit(): void {
    this.getProducts();
    
    console.log(this.getProducts());
  }
  
  getData(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
  getProducts(): void {
    this.ProductService.getProducts()
      .subscribe(Producte => {
        this.GetProducts = Producte;
        console.log(this.GetProducts)
      });
  }
  add(productName: string, productQuantity: number, productEan: string): void {
    const name = productName.trim();
    const quantity = productQuantity;
    const ean = productEan.trim();

    if (name=="" && ean=="" && quantity == 0) { return; }

    const newProduct: Product = { name, quantity, ean };
    this.ProductService.addProduct(newProduct)
      .subscribe((product: Product) => {
          this.Products.push(product);
          this.getProducts();
        }
      )
  }
  delete(productId?: number): void {
    if (productId != undefined) {
      this.ProductService.deleteProduct(productId).subscribe(_=>this.getProducts());
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

    if (name=="" && ean=="" && quantity == 0) { return; }

    const newProduct: GetProducts = { id, name, quantity, ean };
    console.log(newProduct)
    this.ProductService.updateProduct(newProduct)
      .subscribe((updatedProduct: GetProducts) => {
        //const index = this.GetProducts.findIndex(product => product.id === updatedProduct.id);
        //if (index !== -1) {
        //  this.GetProducts[index] = updatedProduct; // Update the existing product
        //}
        this.getProducts();
      });
  }
}
