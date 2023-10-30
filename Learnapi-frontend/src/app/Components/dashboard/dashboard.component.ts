import { HttpClient } from '@angular/common/http';
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
  mail!: FormGroup;
  isDisabled: boolean = true;
  statusactive!: boolean;

  compare_list = [true, false];
  constructor(
    private dashboardservice: DashboardService,
    private fb: FormBuilder,
    private emailservice: MailService,
    private http: HttpClient
  ) {}

  finres: any;
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
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
      email: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      creditlimit: ['', [Validators.required]],
      isActive: ['', [Validators.required]],
      statusname: ['', [Validators.required]],
      taxcode: ['', [Validators.required]],
    });
    this.mail = this.fb.group({
      toEmail: ['', [Validators.required]],
      subject: ['', [Validators.required]],
      body: ['', [Validators.required]],
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
      window.location.reload();
    });
  }

  submitres: any;
  submit() {
    if (this.customercreate.valid) {
      this.dashboardservice
        .addcustomer(this.customercreate.value)
        .subscribe((res) => {
          this.submitres = res;
          window.location.reload();
          console.log('submitted', this.submitres);
          if (this.submitres.responseCode === 400) {
            alert('Please Select Unique ID');
          }
        });
    } else {
      alert('All Fields Are Required');
    }
  }


  demo:any

  code: any;
  name: any;
  email: any;
  phone: any;
  creditlimit: any;
  isActive: any;
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
     console.log(this.isActive);
     this.demo = this.isActive

      
  

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
    });

    // console.log("edit form values 2==",this.editcustomercreate.value);
    this.dashboardservice
      .edit(this.finalrescode.code, this.editcustomercreate.value)
      .subscribe((res) => {
        this.postres = res;
        window.location.reload();
        console.log(this.postres);
      });
  }

  closemodel() {
    this.loadallcustomer();
  }

  // statusactive! :boolean;
  compareChange(event: any) {
    var compare = event.target.value;
    console.log(compare);
    if (compare === 'false') {
      this.statusactive = false;
    } else {
      this.statusactive = true;
    }
  }

  emailres: any;
  sendmail() {
    // var subject = (<HTMLInputElement>document.getElementById('subject')).value;
    // var body = (<HTMLInputElement>document.getElementById('body')).value;
    // let data = {
    //   toEmail: this.email,
    //   subject: subject,
    //   body: body,
    // };
    this.mail.patchValue({
      toEmail: this.email,
    });
    console.log(this.mail.value);
    this.emailservice.sendmail(this.mail.value).subscribe((res) => {
      this.emailres = res;
      console.log(this.emailres);
      (<HTMLInputElement>document.getElementById('body')).value="";
      (<HTMLInputElement>document.getElementById('subject')).value="";
      
    });
  }



  //image upload (single file)
  selectedFile!: File;
  onFileSelected(event:any) {
    this.selectedFile = event.target.files[0];
  }

  uploadImage() {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('formFile', this.selectedFile, this.selectedFile.name); 
     // console.log(formData);
      
      var imginput = (<HTMLInputElement>document.getElementById('imginput')).value;
      //console.log("https://localhost:7213/api/Product/UploadImage?productcode=" + imginput, formData);
      this.http.put("https://localhost:7213/api/Product/UploadImage?productcode=" + imginput, formData).subscribe(res => {
        var finalres = res;
        console.log(finalres);
      });
    }
  }


  //multi image uplaod
  selectedFilesMulti!: FileList;
  onMultiFilesSelected(event: any) {
    this.selectedFilesMulti = event.target.files;
    
    for (let i = 0; i < this.selectedFilesMulti.length; i++) {
      const file = this.selectedFilesMulti[i];
      //  console.log(file.size);
      //  console.log(3 * 1024 * 1024);
      //  console.log(this.selectedFilesMulti.length);
      if (file.size > 3 * 1024 * 1024) {
        // File is too large
        alert('File is too large. Maximum One File size is 3 megabytes');
        if(this.selectedFilesMulti.length > 10){
          alert('Maximum 10 File select at a time');
          return;
        }
        return;
      }
    }

  }

  
  MultiuploadImages() {
    if (this.selectedFilesMulti && this.selectedFilesMulti.length > 0) {
      const formData = new FormData();
      for (let i = 0; i < this.selectedFilesMulti.length; i++) {
        formData.append('filecollection', this.selectedFilesMulti[i], this.selectedFilesMulti[i].name);
      }
      var imginput = (<HTMLInputElement>document.getElementById('multiimginput')).value;
      this.http.put(`https://localhost:7213/api/Product/MultiUploadImage?productcode=${imginput}`, formData).subscribe(
        (response) => {
          console.log('Images uploaded successfully', response);
        },
        (error) => {
          console.error('Error uploading images', error);
        }
      );
    }
  }




}
