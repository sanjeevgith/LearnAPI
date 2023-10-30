import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  URL="https://learapi.bsite.net/api/"
 // URL="https://localhost:7213/api/"
  constructor(private http:HttpClient) { }



  postuser(data:any){
    return this.http.post(this.URL+'Authorize/RegisterUser',data);
  }

}
