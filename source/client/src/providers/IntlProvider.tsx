//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import {
  MessageFormatElement,
  IntlProvider as Provider,
  createIntl
} from 'react-intl';

import ja from '../translations/compiled/ja.json';

const translations: { [key: string]: Record<string, MessageFormatElement[]> } = {
  ja
};

const intl = createIntl({
  defaultLocale: 'ja',
  locale: window.navigator.language.substring(0, 2),
  messages: translations[window.navigator.language.substring(0, 2)]
});

function IntlProvider(props: Readonly<React.PropsWithChildren<unknown>>) {

  const { children } = props;

  return (
    <Provider {...intl}>
      {children}
    </Provider>
  );

}

export default IntlProvider;
