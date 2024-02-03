import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-child',
  templateUrl: './child.component.html',
  styleUrls: ['./child.component.css']
})
export class ChildComponent {
  @Input() title = 'initial Value';
  @Output() event = new EventEmitter();

  constructor() {
  }

  sendDataToParent() {
    this.event.emit('hi parent!');
  }

}
