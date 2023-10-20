import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DashboardService } from 'src/app/Services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  customercreate!: FormGroup;
  isDisabled = true;
  constructor(
    private dashboardservice: DashboardService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    
    this.customercreate = this.fb.group({
      code: ['', Validators.required],
      name: ['', [Validators.required]],
      email: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      creditlimit: ['', [Validators.required]],
      isActive: [true, [Validators.required]],
      statusname: ['', [Validators.required]],
      taxcode: ['', [Validators.required]],
    });

    this.loadallcustomer();
  }
  //  "code": "7",
  //     "name": "bipin kumar",
  //     "email": "bipin@gmail.com",
  //     "phone": "192638819",
  //     "creditlimit": 12990,
  //     "isActive": true,
  //     "statusname": "Active",
  //     "taxcode": 121212

  getallres: any;
  loadallcustomer() {
    this.dashboardservice.getall().subscribe((res) => {
      this.getallres = res;
      console.log(this.getallres);
    });
  }

  delres: any;
  delete(data: any) {
    this.dashboardservice.delete(data).subscribe((res) => {
      this.delres = res;
      this.ngOnInit();
    });
  }

  submit() {

  }

  finalrescode:any
  getcutbycode(code:any){
    this.dashboardservice.getcustomerbycode(code).subscribe(res=>{
      this.finalrescode = res;
      console.log(this.finalrescode);
    })
  }

  postres:any
  edit() {
    this.dashboardservice.edit(this.customercreate.value).subscribe(res=>{
      this.postres = res;
      console.log( this.postres);
      
    })
  }
}
