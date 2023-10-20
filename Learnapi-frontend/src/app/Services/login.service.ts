import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoginService {


  URL="https://localhost:7213/api/"
  constructor(private http:HttpClient) { }

  loginuser(data:any){
    return this.http.post(this.URL+'Authorize/GenerateToken',data);
  }


  isLoggedIn()
  {
      let token = localStorage.getItem("token");
      if(token ==undefined || token===''||token==null){
        return false;
      }else{
        return true;
      }
  }

}
