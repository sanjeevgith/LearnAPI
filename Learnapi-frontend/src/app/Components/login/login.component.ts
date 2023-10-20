import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/Services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginuser!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private loginservice: LoginService,
    private router: Router
  ) {}

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
    if (this.loginuser.valid) {
      this.loginservice.loginuser(this.loginuser.value).subscribe((res) => {
        this.loginres = res;
        console.log(this.loginres);
        localStorage.setItem('token', this.loginres.token);
        this.router.navigate(['dashboard']);
      });
    }
    else{
      alert("Please fill all fields")
    }
  }
}
