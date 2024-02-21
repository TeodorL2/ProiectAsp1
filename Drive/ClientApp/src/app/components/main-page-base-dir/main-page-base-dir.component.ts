import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { Entry } from '../../data/interfaces/entry';
import { GetEntriesService } from '../../core/services/get-entries.service';
import { Location } from '@angular/common';
import { DateFormattingPipe } from '../../core/pipes/date-formatting.pipe';

@Component({
  selector: 'app-main-page-base-dir',
  templateUrl: './main-page-base-dir.component.html',
  styleUrls: ['./main-page-base-dir.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    DateFormattingPipe
  ]
})
export class MainPageBaseDirComponent implements OnInit {

  items: Entry[] = [];
  pathToAsk: string = "";
  currentFolder: string = "";

  constructor(private location: Location, private readonly getEntriesService: GetEntriesService) { }

  ngOnInit(): void {
    this.pathToAsk = this.location.path();
    var pathParts = this.pathToAsk.split('/');
    this.currentFolder = pathParts[pathParts.length - 1];
    this.getEntriesService.getEntries(this.pathToAsk).subscribe(
      (data: any) => {
        this.items = data;
      },
      (error) => {
        console.error('Error during fetching entries: ' + error.error.message);
      }
    );
  }

  download() {
    // TODO
  }

}
