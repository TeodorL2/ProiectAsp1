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
import { DeleteItemService } from '../../core/services/delete-item.service';
import { HttpClient } from '@angular/common/http';
import { UploadFilesDto } from '../../data/interfaces/upload-files-dto';
import { UploadFilesService } from '../../core/services/upload-files.service';

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
    private route: ActivatedRoute, private deleteItemService: DeleteItemService, private httpClient: HttpClient,
    private uploadFilesService: UploadFilesService) { }

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

  showDeleteConsentMenu: boolean = false;
  deleteThisConsent() {
    this.showDeleteConsentMenu = true;
  }

  deleteThis() {
    var pathParts = this.router.url.split('/');
    this.deleteItemService.delete(pathParts.join('/')).subscribe(
      (data: any) => { console.log("deleted successfully")},
      (error: any) => console.error(error)

    );
    this.showDeleteConsentMenu = false;
    // this.router.navigate([this.router.url]);
  }

  hideDeleteThisCOnsent() {
    this.showDeleteConsentMenu = false;
  }


  // Rename
  showRenameMenu: boolean = false;

  renameMenu() {
    this.showRenameMenu = true;
  }
  rename() {

  }

  // Upload files
  uploadFilesMenu: boolean = false;
  showUploadFilesMenu() {
    this.uploadFilesMenu = true;
  }
  hideUploadFilesMenu() {
    this.uploadFilesMenu = false;
  }

  selectedFiles: File[] = [];

  onFileSelected(event: any) {
    this.selectedFiles = event.target.files;
  }

  uploadFiles() {
    const formData = new FormData();

    formData.append('Path', this.router.url.substr(1));

    for (let i = 0; i < this.selectedFiles.length; ++i) {
      formData.append('Files', this.selectedFiles[i]);
    }

    this.uploadFilesService.create(formData).subscribe(
      (data: any) => {
        this.hideUploadFilesMenu();
        this.router.navigate([this.router.url]);
      },
      (error: any) => {
        console.error(error);
      }
    );
  }

}
