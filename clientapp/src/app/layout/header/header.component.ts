import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  userName: any;
  profilepic: any;

  constructor(

  ) { 
    this.userName = sessionStorage.getItem("USERNAME");
    this.profilepic = sessionStorage.getItem('USERPIC');
  }

  ngOnInit(): void {
  }


  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }
  logOut(){
    sessionStorage.removeItem("USERNAME");
    sessionStorage.clear();
    window.location.reload();
  }
}
