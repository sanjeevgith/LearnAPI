import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MailService {

  constructor(private http:HttpClient) { }


  URL="https://learapi.bsite.net/api/"


  sendmail(data:any){
    console.log("data",data);
    return this.http.post(this.URL+"Email/SendMail",data)
  }

}
