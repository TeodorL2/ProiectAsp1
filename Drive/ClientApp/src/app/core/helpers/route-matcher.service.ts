import { Injectable } from '@angular/core';
import { UrlMatchResult, UrlSegment } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RouteMatcherService {

  constructor() { }

  static isBaseDir(segments: UrlSegment[]): any {
  if (segments.length === 2)
    return { consumed: segments, posParams: {} };
  else
    return null;
  }

  static addBaseDirRoute(segments: UrlSegment[]): UrlMatchResult | null {
  
  if (segments.length !== 1) {
    return null;
  }

  const queryParams = segments[0].parameters;
  const hasQueryParam = queryParams && Object.keys(queryParams).length === 1;

  if (!hasQueryParam) {
    return null;
  }

  const paramValue = queryParams['createBaseDir'];
  if (paramValue !== 'true') {
    return null;
  }

    return {
      consumed: segments,
      posParams: {
        param: new UrlSegment(paramValue, {})
      }
    };
}

}
