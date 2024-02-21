import { TestBed } from '@angular/core/testing';

import { RouteMatcherService } from './route-matcher.service';

describe('RouteMatcherService', () => {
  let service: RouteMatcherService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RouteMatcherService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
