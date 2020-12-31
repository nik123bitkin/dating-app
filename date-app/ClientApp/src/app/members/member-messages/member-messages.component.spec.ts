/// <reference path="../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { MemberMessagesComponent } from './member-messages.component';

let component: MemberMessagesComponent;
let fixture: ComponentFixture<MemberMessagesComponent>;

describe('member-messages component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ MemberMessagesComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(MemberMessagesComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});