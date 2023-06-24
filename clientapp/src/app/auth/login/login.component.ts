import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { windowToggle } from 'rxjs';

declare var $:any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginMessage: any;  
  loginData: any;

  constructor(
    private loginService: LoginService

  ) { }

  ngOnInit(): void {
  }

  submitLoginForm(loginForm:NgForm) {
    if(loginForm.valid)
    {
       this.loginData = loginForm.value;
       this.loginService.sendLoginRequest(this.loginData).subscribe(res => {
          if(res.statuscode == 200) {
            this.loginMessage = res.message;
            if (res.isactivated == 0) {
              window.setTimeout(() => {
                this.loginMessage  = null;
              }, 3000);
  
              return;
            }
            sessionStorage.setItem("USERID", res.id);
            sessionStorage.setItem("TOKEN", res.token);
            sessionStorage.setItem("USERPIC", res.profilepic);

            if (res.qrcodeurl != "") {
              $("#loginClose").click();
              $("#mfashow").click();
              return;
            }
            sessionStorage.setItem("USERNAME", res.username);

            window.setTimeout(() => {
              this.loginMessage  = null;
              window.location.reload();
            }, 3000);

          } else {
            this.loginMessage = res.message;
            setTimeout(() => {
              this.loginMessage  = null;
            }, 3000);

          }
      });
    }
  }

}
