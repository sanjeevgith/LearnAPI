import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginService } from '../Services/login.service';

@Injectable()
export class InterInterceptor implements HttpInterceptor {

  constructor(private login:LoginService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let newReq = request;
        let token = this.login.getToken()

       // console.log("intercepter", token);
        if (token != null) {
            //console.log("tokeen is not null");
            newReq = newReq.clone({
                setHeaders: {
                  Authorization: `bearer ${token}`
                }
            })
           // console.log(newReq);
            
        }
        return next.handle(newReq)
  }
}
