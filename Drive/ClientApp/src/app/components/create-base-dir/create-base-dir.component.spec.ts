import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBaseDirComponent } from './create-base-dir.component';

describe('CreateBaseDirComponent', () => {
  let component: CreateBaseDirComponent;
  let fixture: ComponentFixture<CreateBaseDirComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateBaseDirComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateBaseDirComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
