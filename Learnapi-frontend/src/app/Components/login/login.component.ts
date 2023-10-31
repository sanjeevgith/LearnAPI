import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { LoginService } from 'src/app/Services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginuser!: FormGroup;
  typeSelected: string;
  constructor(
    private fb: FormBuilder,
    private loginservice: LoginService,
    private router: Router,
    private spinnerService :NgxSpinnerService
  ) {
    this.typeSelected = 'ball-fussion';
  }

  finalres: any;
  ngOnInit(): void {
    localStorage.clear();
    this.loginuser = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required]],
    });
  }

  loginres: any;
  submit() {
    this.spinnerService.show();
    if (this.loginuser.valid) {
      this.loginservice.loginuser(this.loginuser.value).subscribe((res) => {
        this.loginres = res;
        console.log(this.loginres);
          this.spinnerService.hide();
        localStorage.setItem('token', this.loginres.token);
        this.router.navigate(['dashboard']);
      });
    }
    else{
      alert("Please fill all fields")
    }
  }
}
