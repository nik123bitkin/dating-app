/// <reference path="../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { MemberCardComponent } from './member-card.component';

let component: MemberCardComponent;
let fixture: ComponentFixture<MemberCardComponent>;

describe('member-card component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ MemberCardComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(MemberCardComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});