import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StorageService } from '../../core/services/storage.service';

@Component({
  selector: 'app-entries',
  templateUrl: './entries.component.html',
  styleUrls: ['./entries.component.css']
})
export class EntriesComponent implements OnInit {

  path: string = ''; // Initialize path to an empty string
  entries: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private storageService: StorageService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.path = params.get('path') || ''; // Get the 'path' parameter from the URL
      this.getEntries();
    });
  }

  getEntries() {
    this.storageService.getEntries(this.path)
      .subscribe(
        (data: any[]) => {
          this.entries = data;
        },
        error => {
          console.error('Error fetching entries:', error);
        }
      );
  }

}
