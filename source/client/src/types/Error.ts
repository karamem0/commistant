//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

export class ArgumentNullError extends Error {

  constructor(name?: string) {
    super();
    this.name = 'ArgumentNullError';
    this.message = `${name ?? 'The argument'} is null or undefined.`;
  }

}

export class DependencyNullError extends Error {

  constructor(name?: string) {
    super();
    this.name = 'DependencyNullError';
    this.message = `${name ?? 'The dependency'} is null or undefined.`;
  }

}
