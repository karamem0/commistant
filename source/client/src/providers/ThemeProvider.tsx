//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import {
  FluentProvider as Provider,
  Theme,
  teamsDarkTheme,
  teamsHighContrastTheme,
  teamsLightTheme
} from '@fluentui/react-components';
import { app } from '@microsoft/teams-js';
import { css } from '@emotion/react';

interface ThemeContextProps {
  theme: Theme
}

const ThemeContext = React.createContext<ThemeContextProps | undefined>(undefined);

export const useTheme = (): ThemeContextProps => {
  const value = React.useContext(ThemeContext);
  if (!value) {
    throw new Error('The context is not initialzed: ThemeContext');
  }
  return value;
};

interface ThemeProviderProps {
  children?: React.ReactNode
}

function ThemeProvider(props: Readonly<ThemeProviderProps>) {

  const { children } = props;

  const [ value, setValue ] = React.useState<ThemeContextProps>({ theme: teamsLightTheme });

  const handleThemeChange = (value: string) => {
    switch (value) {
      case 'dark':
        setValue({ theme: teamsDarkTheme });
        break;
      case 'contrast':
        setValue({ theme: teamsHighContrastTheme });
        break;
      default:
        setValue({ theme: teamsLightTheme });
        break;
    }
  };

  React.useEffect(() => {
    if (app.isInitialized()) {
      (async () => {
        const context = await app.getContext();
        handleThemeChange(context.app.theme);
        app.registerOnThemeChangeHandler(handleThemeChange);
      })();
    }
  }, []);

  return (
    <ThemeContext.Provider value={value}>
      <Provider theme={value.theme}>
        <div
          css={css`
            font-size: ${value.theme.fontSizeBase400};
            line-height: calc(${value.theme.fontSizeBase400} * 1.25);
          `}>
          {children}
        </div>
      </Provider>
    </ThemeContext.Provider>
  );

}

export default ThemeProvider;
