//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { useSnackbar } from '../providers/SnackbarProvider';

import Presenter from './Snackbar.presenter';

function Snackbar() {

  const { snackbar, setSnackbar } = useSnackbar();

  const handleDismiss = React.useCallback(() => {
    setSnackbar(undefined);
  }, [
    setSnackbar
  ]);

  return (
    <Presenter
      text={snackbar?.text}
      type={snackbar?.type}
      onDismiss={handleDismiss} />
  );

}

export default Snackbar;
