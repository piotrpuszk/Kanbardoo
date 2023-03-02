import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardSettingsModalComponent } from './board-settings-modal.component';

describe('BoardSettingsModalComponent', () => {
  let component: BoardSettingsModalComponent;
  let fixture: ComponentFixture<BoardSettingsModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BoardSettingsModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoardSettingsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
