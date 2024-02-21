import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDirComponent } from './create-dir.component';

describe('CreateDirComponent', () => {
  let component: CreateDirComponent;
  let fixture: ComponentFixture<CreateDirComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateDirComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateDirComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
