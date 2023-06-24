import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AboutusComponent } from './aboutus/aboutus.component';
import { LocateusComponent } from './locateus/locateus.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { HeaderComponent } from './layout/header/header.component';
import { FooterComponent } from './layout/footer/footer.component';
import { ListComponent } from './products/list/list.component';
import { CatalogComponent } from './products/catalog/catalog.component';
import { SearchComponent } from './products/search/search.component';
import { LoginComponent } from './auth/login/login.component';
import { RegistrationComponent } from './auth/registration/registration.component';
import { MfaComponent } from './auth/mfa/mfa.component';

@NgModule({
  declarations: [
    AppComponent,
    AboutusComponent,
    LocateusComponent,
    HomeComponent,
    ProfileComponent,
    HeaderComponent,
    FooterComponent,
    ListComponent,
    CatalogComponent,
    SearchComponent,
    LoginComponent,
    RegistrationComponent,
    MfaComponent
  ],
  imports: [
    FormsModule,
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'aboutus', component: AboutusComponent },
      { path: 'locateus', component: LocateusComponent },
      { path: 'profile', component: ProfileComponent },
      { path: 'productlist', component: ListComponent },
      { path: 'productcatalog', component: CatalogComponent },
      { path: 'productsearch', component: SearchComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
