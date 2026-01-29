//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { css } from '@emotion/react';
import {
  FluentProvider as Provider,
  Theme,
  createLightTheme,
  teamsDarkTheme,
  teamsHighContrastTheme,
  teamsLightTheme
} from '@fluentui/react-components';
import { app } from '@microsoft/teams-js';

const customThemePalette = {
  10: '#050201',
  20: '#23130F',
  30: '#3B1C17',
  40: '#4F231D',
  50: '#642A23',
  60: '#7A322A',
  70: '#903930',
  80: '#A84036',
  90: '#BF483D',
  100: '#D74F44',
  110: '#EC5B4E',
  120: '#F27363',
  130: '#F88A79',
  140: '#FC9F90',
  150: '#FFB5A7',
  160: '#FFCAC0'
};

const customTheme = createLightTheme(customThemePalette);

interface ThemeContextState {
  theme: Theme
}

const ThemeContext = React.createContext<ThemeContextState | undefined>(undefined);

export const useTheme = (): ThemeContextState => {
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

  const [ theme, setTheme ] = React.useState<Theme>(app.isInitialized() ? teamsLightTheme : customTheme);

  const handleThemeChange = (value: string) => {
    switch (value) {
      case 'dark':
        setTheme(teamsDarkTheme);
        break;
      case 'contrast':
        setTheme(teamsHighContrastTheme);
        break;
      default:
        setTheme(teamsLightTheme);
        break;
    }
  };

  const value = React.useMemo<ThemeContextState>(() => ({ theme }), [
    theme
  ]);

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
