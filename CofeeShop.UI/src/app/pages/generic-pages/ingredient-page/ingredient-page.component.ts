import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-ingredient-page',
  templateUrl: './ingredient-page.component.html',
})
export class IngredientPageComponent {
  @Input()
  public ingredientType: string = 'Ingredient'
  
  constructor() {}
}
