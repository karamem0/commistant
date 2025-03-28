//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

interface TeamsInitializerProps {
  loading?: boolean
}

function TeamsInitializer(props: Readonly<React.PropsWithChildren<TeamsInitializerProps>>) {

  const {
    children,
    loading
  } = props;

  return loading ? null : children;

}

export default React.memo(TeamsInitializer);
