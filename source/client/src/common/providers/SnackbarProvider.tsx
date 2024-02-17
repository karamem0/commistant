//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import Snackbar from '../components/Snackbar';
import { SnackbarType } from '../types/Snackbar';

interface SnackbarProps {
  text?: string,
  type?: SnackbarType
}

interface SnackbarContextProps {
  snackbar?: SnackbarProps,
  setSnackbar: React.Dispatch<React.SetStateAction<SnackbarProps | undefined>>
}

const SnackbarContext = React.createContext<SnackbarContextProps | undefined>(undefined);

export const useSnackbar = (): SnackbarContextProps => {
  const props = React.useContext(SnackbarContext);
  if (!props) {
    throw new Error('The context is not initialzed: SnackbarContext');
  }
  return props;
};

interface SnackbarProviderProps {
  children?: React.ReactNode
}

function SnackbarProvider(props: SnackbarProviderProps) {

  const { children } = props;

  const [ snackbar, setSnackbar ] = React.useState<SnackbarProps>();
  const value = React.useMemo(() => ({
    snackbar,
    setSnackbar
  }), [
    snackbar
  ]);

  return (
    <SnackbarContext.Provider value={value}>
      <Snackbar />
      {children}
    </SnackbarContext.Provider>
  );

}

export default SnackbarProvider;
