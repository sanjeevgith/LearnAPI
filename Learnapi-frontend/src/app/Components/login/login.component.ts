import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit{





constructor(private http:HttpClient) {
 
  
}

finalres:any
  ngOnInit(): void {
    this.http.get("https://localhost:7213/api/Customer/GetAll").subscribe(res=>{
      this.finalres = res;
      console.log(this.finalres);
      
    })
  }

}
