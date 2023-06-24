import { Component, OnInit } from '@angular/core';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-catalog',
  templateUrl: './catalog.component.html',
  styleUrls: ['./catalog.component.css']
})
export class CatalogComponent implements OnInit {

  page: number = 1;
  totpage: any;
  products: any;

  constructor(
    private productsService: ProductsService
  ) { 
    this.productCatalogs(this.page);    
  }

  ngOnInit(): void {
  }

  productCatalogs(page: any) {
    this.productsService.sendProductRequest(page).subscribe( res => {
      this.totpage = res.totpage;
      this.products = res.products;
    })
  }

  lastPage(event: any) {
    event.preventDefault();    
    this.page = this.totpage;
    this.productCatalogs(this.page);
    return;    
  }

  nextPage(event: any) {
    event.preventDefault();    
    if (this.page == this.totpage) {
      return;
    }
    this.page = this.page + 1;
    this.productCatalogs(this.page);
    return;    
  }

  prevPage(event: any) {
    event.preventDefault();    
    if (this.page == 1) {
      return;
    }
    this.page = this.page - 1;
    this.productCatalogs(this.page);
    return;    

  }

  firstPage(event: any) {
    event.preventDefault();    
    this.page = 1;
    this.productCatalogs(this.page);
    return;    
  }

}
