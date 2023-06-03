//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { app, authentication } from '@microsoft/teams-js';

import axios from 'axios';

interface TeamsContextProps {
  context?: app.Context,
  authToken?: string
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

function TeamsProvider(props: TeamsProviderProps) {

  const { children } = props;

  const [ value, setValue ] = React.useState<TeamsContextProps>({});

  React.useEffect(() => {
    (async () => {
      await app.initialize();
      const context = await app.getContext();
      const authToken = await authentication.getAuthToken();
      setValue({ context, authToken });
      axios.interceptors.request.use(async (request) => {
        request.headers.Authorization = `Bearer ${authToken}`;
        return request;
      });
    })();
  }, []);

  return (
    <TeamsContext.Provider value={value}>
      {children}
    </TeamsContext.Provider>
  );

}

export default TeamsProvider;
