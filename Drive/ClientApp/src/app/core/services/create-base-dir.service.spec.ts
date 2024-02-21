import { TestBed } from '@angular/core/testing';

import { CreateBaseDirService } from './create-base-dir.service';

describe('CreateBaseDirService', () => {
  let service: CreateBaseDirService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateBaseDirService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
