import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from 'src/app/Services/register.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  user!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private register: RegisterService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.user = this.fb.group({
      code: ['', Validators.required],
      name: ['', [Validators.required]],
      email: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      isactive: [true, [Validators.required]],
      password: ['', [Validators.required]],
      role: ['', [Validators.required]],
    });
  }

  userres: any;
  submit() {
    console.log(this.user.value);
    if (this.user.valid) {
      this.register.postuser(this.user.value).subscribe((res) => {
        this.userres = res;
        console.log('userres', this.userres);
        if (this.userres.responseCode === 400) {
          alert('Please Select Unique ID');
        } else {
          this.router.navigate(['login']);
        }
      });
    } else {
      alert('Please fill all fields');
    }
  }
}
