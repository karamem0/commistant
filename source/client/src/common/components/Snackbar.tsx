//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { useInterval } from 'react-use';

import { useSnackbar } from '../providers/SnackbarProvider';

import Presenter from './Snackbar.presenter';

function Snackbar() {

  const { snackbar, setSnackbar } = useSnackbar();

  const handleDismiss = React.useCallback(() => {
    setSnackbar(undefined);
  }, [
    setSnackbar
  ]);

  useInterval(() => {
    setSnackbar(undefined);
  }, snackbar?.text ? 5000 : undefined);

  return (
    <Presenter
      text={snackbar?.text}
      type={snackbar?.type}
      onDismiss={handleDismiss} />
  );

}

export default Snackbar;
