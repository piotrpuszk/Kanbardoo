import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitationsModalComponent } from './invitations-modal.component';

describe('InvitationsModalComponent', () => {
  let component: InvitationsModalComponent;
  let fixture: ComponentFixture<InvitationsModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvitationsModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvitationsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
