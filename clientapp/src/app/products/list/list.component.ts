import { Component, OnInit } from '@angular/core';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  page: number = 1;
  totpage: any;
  products: any;

  constructor(
    private productsService: ProductsService
  ) { 
    this.productList(this.page);
  }

  ngOnInit(): void {
  }

  productList(page: any) {
    this.productsService.sendProductRequest(page).subscribe( res => {
      this.totpage = res.totpage;
      this.products = res.products;
    })
  }

  lastPage(event: any) {
    event.preventDefault();    
    this.page = this.totpage;
    this.productList(this.page);
    return;    
  }

  nextPage(event: any) {
    event.preventDefault();    
    if (this.page == this.totpage) {
      return;
    }
    this.page = this.page + 1;
    this.productList(this.page);
    return;    
  }

  prevPage(event: any) {
    event.preventDefault();    
    if (this.page == 1) {
      return;
    }
    this.page = this.page - 1;
    this.productList(this.page);
    return;    

  }

  firstPage(event: any) {
    event.preventDefault();    
    this.page = 1;
    this.productList(this.page);
    return;    
  }

}
