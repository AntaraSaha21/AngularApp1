import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductVersionsComponent } from './product-versions.component';

describe('ProductVersionsComponent', () => {
  let component: ProductVersionsComponent;
  let fixture: ComponentFixture<ProductVersionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProductVersionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductVersionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
