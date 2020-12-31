/// <reference path="../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { PhotoEditorComponent } from './photo-editor.component';

let component: PhotoEditorComponent;
let fixture: ComponentFixture<PhotoEditorComponent>;

describe('photo-editor component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ PhotoEditorComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(PhotoEditorComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});