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

import Presenter from './TeamsInitializer.presenter';

interface TeamsInitializerProps {
  children?: (inTeams?: boolean) => React.ReactNode
}

function TeamsInitializer(props: Readonly<TeamsInitializerProps>) {

  const { children } = props;

  const [ inTeams, setInTeams ] = React.useState<boolean>();
  const [ loading, setLoading ] = React.useState<boolean>();

  React.useEffect(() => {
    (async () => {
      try {
        setLoading(true);
        await app.initialize();
        await authentication.getAuthToken();
        axios.interceptors.request.use(async (request) => {
          const authToken = await authentication.getAuthToken();
          request.baseURL = import.meta.env.VITE_FUNCTION_APP_URL;
          request.headers.Authorization = `Bearer ${authToken}`;
          return request;
        });
        setInTeams(true);
      } catch {
        setInTeams(false);
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  return (
    <Presenter loading={loading}>
      {inTeams == null ? null : children?.(inTeams)}
    </Presenter>
  );

}

export default TeamsInitializer;
