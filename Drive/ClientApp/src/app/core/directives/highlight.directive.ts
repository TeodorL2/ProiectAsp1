import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appHighlight]',
  standalone: true
})
export class HighlightDirective {

  constructor(private htmlElement: ElementRef) { }

  @HostListener('mouseenter') OnMouseEnter() {
    this.htmlElement.nativeElement.style.borderRadius = "10px";
    this.htmlElement.nativeElement.style.borderStyle = "solid";
    this.htmlElement.nativeElement.style.borderWidth = "3px";
    this.htmlElement.nativeElement.style.borderColor = "#000099";
  }

  @HostListener('mouseleave') OnMouseLeave() {
    this.htmlElement.nativeElement.style.borderStyle = "none";
  }
}
