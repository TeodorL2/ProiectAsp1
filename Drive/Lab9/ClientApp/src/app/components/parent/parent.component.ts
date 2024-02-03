import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-parent',
  templateUrl: './parent.component.html',
  styleUrls: ['./parent.component.css']
})
export class ParentComponent implements OnInit {
  titleFromParent = 'title from parent 2';
  constructor() { }

  ngOnInit(): void {
  }

  listenToChildResponse($event: any){
    console.log($event);
  }
}
