import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  URL = 'https://localhost:7213/api/Customer/';

  constructor(private http: HttpClient) {}

  getall() {
    return this.http.get(this.URL + 'GetAll');
  }

  delete(code: any) {
    return this.http.delete(this.URL + 'Remove?code=' + code);
  }

  edit(code:any,data: any) {
    return this.http.put(this.URL +'Update?code=' + code , data);
  }

  addcustomer(data: any) {
    return this.http.post(this.URL + 'Create', data);
  }

  getcustomerbycode(code:any){
    return this.http.get(this.URL + 'GetByCode?code=' + code)
  }
}
