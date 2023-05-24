import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-product-page',
  templateUrl: './product-page.component.html',
})
export class ProductPageComponent {
  @Input()
  public productType: string = 'Product'

  constructor() {}
}
