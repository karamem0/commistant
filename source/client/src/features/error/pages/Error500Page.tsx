//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import Presenter from './Error500Page.presenter';
import { useAppInsightsContext } from '@microsoft/applicationinsights-react-js';

interface Error500PageProps {
  error?: Error
}

function Error500Page(props: Readonly<Error500PageProps>) {

  const { error } = props;

  const { trackException } = useAppInsightsContext();

  React.useEffect(() => {
    trackException({ exception: error });
  }, [
    error,
    trackException
  ]);

  return (
    <Presenter error={error?.message} />
  );

}

export default Error500Page;
