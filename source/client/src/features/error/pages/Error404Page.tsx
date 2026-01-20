//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import Presenter from './Error404Page.presenter';

interface Error404PageProps {
  error?: Error
}

function Error404Page(props: Readonly<Error404PageProps>) {

  const { error } = props;

  return (
    <Presenter error={error?.message} />
  );

}

export default Error404Page;
