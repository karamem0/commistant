//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { app, authentication } from '@microsoft/teams-js';
import axios from 'axios';
import { useError } from 'react-use';

interface TeamsContextProps {
  context?: app.Context
}

const TeamsContext = React.createContext<TeamsContextProps | undefined>(undefined);

export const useTeams = (): TeamsContextProps => {
  const value = React.useContext(TeamsContext);
  if (!value) {
    throw new Error('The context is not initialzed: TeamsContext');
  }
  return value;
};

interface TeamsProviderProps {
  children?: React.ReactNode
}

function TeamsProvider(props: Readonly<TeamsProviderProps>) {

  const { children } = props;

  const [ value, setValue ] = React.useState<TeamsContextProps>({});

  const dispatchError = useError();

  React.useEffect(() => {
    (async () => {
      try {
        await authentication.getAuthToken();
        setValue({ context: await app.getContext() });
        axios.interceptors.request.use(async (request) => {
          const authToken = await authentication.getAuthToken();
          request.baseURL = import.meta.env.VITE_FUNCTION_APP_URL;
          request.headers.Authorization = `Bearer ${authToken}`;
          return request;
        });
      } catch (e) {
        dispatchError(e as Error);
      }
    })();
  }, [
    dispatchError
  ]);

  return (
    <TeamsContext.Provider value={value}>
      {children}
    </TeamsContext.Provider>
  );

}

export default TeamsProvider;
