import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MailService {

  constructor(private http:HttpClient) { }




  sendmail(data:any){
    console.log("data",data);
    return this.http.post("https://localhost:7213/api/Email/SendMail",data)
  }

}
