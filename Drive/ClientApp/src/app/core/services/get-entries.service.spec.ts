import { TestBed } from '@angular/core/testing';

import { GetEntriesService } from './get-entries.service';

describe('GetEntriesService', () => {
  let service: GetEntriesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetEntriesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
