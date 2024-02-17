//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

export function inTeams(): boolean {
  if (window.parent === window.self &&
      Object.hasOwn(window, 'nativeInterface')) {
    return true;
  }
  if (window.navigator.userAgent.includes('Teams/')) {
    return true;
  }
  if (window.name === 'embedded-page-container') {
    return true;
  }
  if (window.name === 'extension-tab-frame') {
    return true;
  }
  return false;
}
