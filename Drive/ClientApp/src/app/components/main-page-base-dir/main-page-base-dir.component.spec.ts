import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainPageBaseDirComponent } from './main-page-base-dir.component';

describe('MainPageBaseDirComponent', () => {
  let component: MainPageBaseDirComponent;
  let fixture: ComponentFixture<MainPageBaseDirComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MainPageBaseDirComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainPageBaseDirComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
