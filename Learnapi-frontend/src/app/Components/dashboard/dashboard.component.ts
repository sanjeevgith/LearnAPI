import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormControl,
} from '@angular/forms';
import { DashboardService } from 'src/app/Services/dashboard.service';
import { MailService } from 'src/app/Services/mail.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  customercreate!: FormGroup;
  editcustomercreate!: FormGroup;
  isDisabled: boolean = true;
  statusactive! :boolean; 

  compare_list=[true,false]
  constructor(
    private dashboardservice: DashboardService,
    private fb: FormBuilder,
    private emailservice:MailService
  ) {}

  ngOnInit(): void {
    this.customercreate = this.fb.group({
      code: ['', Validators.required],
      name: ['', [Validators.required]],
      email: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      creditlimit: ['', [Validators.required]],
      isActive: [true, [Validators.required]],
      statusname: [''],
      taxcode: ['', [Validators.required]],
    });
    this.editcustomercreate = this.fb.group({
      code: ['',[Validators.required]],
      name: ['', [Validators.required]],
      email: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      creditlimit: ['', [Validators.required]],
      isActive: ['', [Validators.required]],
      statusname: ['', [Validators.required]],
      taxcode: ['', [Validators.required]],
    });

    this.loadallcustomer();
  }

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
      window.location.reload()
    });
  }

  submitres: any;
  submit() {
    if (this.customercreate.valid) {
      this.dashboardservice
        .addcustomer(this.customercreate.value)
        .subscribe((res) => {
          this.submitres = res;
          window.location.reload()
          console.log('submitted', this.submitres);
          if (this.submitres.responseCode === 400) {
            alert('Please Select Unique ID');
          }
        });
    }
    else{
      alert("All Fields Are Required");
    }
  }

  code: any;
  name: any;
  email: any;
  phone: any;
  creditlimit: any;
  isActive: any ; 
  statusname: any;
  taxcode: any;
  finalrescode: any;
  getcutbycode(code: any) {
    this.dashboardservice.getcustomerbycode(code).subscribe((res) => {
      this.finalrescode = res;
      this.code = this.finalrescode.code;
      this.name = this.finalrescode.name;
      this.email = this.finalrescode.email;
      this.phone = this.finalrescode.phone;
      this.creditlimit = this.finalrescode.creditlimit;
      this.isActive = this.finalrescode.isActive;
      // console.log(this.isActive);
      
      this.statusname = this.finalrescode.statusname;
      this.taxcode = this.finalrescode.taxcode;

      // console.log(this.finalrescode);
    });
  }

  postres: any;
  edit() {
    // console.log("patch this.statusactive", this.statusactive);
    // console.log("edit form values==",this.editcustomercreate.value);
      this.editcustomercreate.patchValue({
        isActive: this.statusactive,
      })
      
      // console.log("edit form values 2==",this.editcustomercreate.value);
    this.dashboardservice
      .edit(this.finalrescode.code, this.editcustomercreate.value)
      .subscribe((res) => {
        this.postres = res;
        window.location.reload()
        console.log(this.postres);
      });
  }


  closemodel() {
    this.loadallcustomer();
  }




  // statusactive! :boolean; 
  compareChange(event:any){
    var compare = event.target.value;
    console.log(compare);
    if(compare === "false"){
    this.statusactive = false; 
    }
    else{
    this.statusactive = true; 
    }
  }





  emailres:any
  sendmail(){
  let data ={
    "toEmail":this.email,
    "subject": "test",
    "body": "string"
  }
   this.emailservice.sendmail(data).subscribe(res=>{
    this.emailres=res;
    console.log(this.emailres);
    
  })
  }









}
