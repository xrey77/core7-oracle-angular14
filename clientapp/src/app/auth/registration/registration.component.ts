import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../../services/registration.service';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  registrationData: any;
  registrationMessage: any; 

  constructor(
    private registrationService: RegistrationService,
  ) { }

  ngOnInit(): void {
  }

  submitRegistrationForm(registrationForm:NgForm) {
    if(registrationForm.valid)
    {
       this.registrationData = registrationForm.value;

       this.registrationService.sendRegistrationRequest(this.registrationData).subscribe(res => {
        if(res.statuscode == 200) {
          this.registrationMessage = res.message;
          return;
        } else {
          this.registrationMessage = res.message;
          setTimeout(() => {
            this.registrationMessage = null;            
          }, 3000);

        }

      });     
    }
  }
}
