import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { Entry } from '../../data/interfaces/entry';
import { GetEntriesService } from '../../core/services/get-entries.service';
import { NavigationEnd, Router } from '@angular/router';
import { Location } from '@angular/common';
import { DateFormattingPipe } from '../../core/pipes/date-formatting.pipe';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { CreateBaseDirComponent } from '../create-base-dir/create-base-dir.component';
import { CreateDirComponent } from '../create-dir/create-dir.component';
import { HighlightDirective } from '../../core/directives/highlight.directive';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    DateFormattingPipe,
    RouterModule,
    CreateBaseDirComponent,
    CreateDirComponent,
    HighlightDirective
  ]
})
export class MainPageComponent implements OnInit {
  
  items: Entry[] = [];
  pathToAsk: string = "";
  currentFolder: string = "";
  constructor(private location: Location, private readonly getEntriesService: GetEntriesService, private router: Router,
              private route: ActivatedRoute) { }

  getEntries() {
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

  ngOnInit(): void {
    this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd)
    ).subscribe((event: any) => {
      /*
      if (event.urlAfterRedirects === '**') {

        this.getEntries();
      } */
      this.getEntries();
    });
    this.getEntries();
  }

  getPathParts(): string[] {
    return this.location.path().split('/');
  }

  isUserDir(): boolean {
    return this.getPathParts().length === 2;
  }

  isBaseDir(): boolean {
    return this.getPathParts().length === 3;
  }

  createBaseDirMenu(): boolean {
    return localStorage.getItem('createBaseDirMenu') !== null;
  } 

  createBaseDir() {
    localStorage.setItem('createBaseDirMenu', "true");
  }

  createDirMenu(): boolean {
    return localStorage.getItem('createDirMenu') !== null;
  }

  createDir() {
    localStorage.setItem('createDirMenu', "true");
  }

  download() {
    // TODO
  }

  navToDir(dirName: string) {
    this.router.navigate([this.router.url + "/" + dirName]);
  }

  navigatePrevDir() {
    var pathParts = this.router.url.split('/');
    pathParts.pop();
    this.router.navigate([pathParts.join('/')]);
  }
}
