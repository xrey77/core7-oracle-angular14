import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  search: string = '';
  searchForm: any;
  searchKey: any;
  products: any;

  constructor(
    private productsService: ProductsService
  ) { }

  ngOnInit(): void {
  }

  submitSearchForm(searchForm:NgForm) {
    if (searchForm.valid) {
      this.searchKey = searchForm.value;
      this.productsService.sendSearchRequest(this.searchKey).subscribe((res: any) => {
        this.products = res.products;
      })
    }
  }

}
