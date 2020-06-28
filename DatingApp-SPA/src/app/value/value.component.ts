import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {

  values: any;
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getValues();
  }

  /*Get request from Angular to Api, which returns an observable(json object/stream of data), to see the observable we need to subscribe

  Assigning the respone to values(type any) **/

  getValues(): void {
    this.http.get('http://localhost:5000/api/values').subscribe(response => {this.values = response; } ,
    error => {console.log(error); });
  }

}
