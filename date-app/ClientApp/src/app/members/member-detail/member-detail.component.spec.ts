/// <reference path="../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { MemberDetailComponent } from './member-detail.component';

let component: MemberDetailComponent;
let fixture: ComponentFixture<MemberDetailComponent>;

describe('member-detail component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ MemberDetailComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(MemberDetailComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});