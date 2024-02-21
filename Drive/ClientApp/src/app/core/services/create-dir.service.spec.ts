import { TestBed } from '@angular/core/testing';

import { CreateDirService } from './create-dir.service';

describe('CreateDirService', () => {
  let service: CreateDirService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateDirService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
