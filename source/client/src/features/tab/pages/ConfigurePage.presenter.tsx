//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { FormattedMessage } from 'react-intl';

import { Text } from '@fluentui/react-components';

import { css } from '@emotion/react';

import { useTheme } from '../../../providers/ThemeProvider';
import messages from '../messages';

function ConfigurePage() {

  const { theme } = useTheme();

  return (
    <div
      css={css`
        min-height: 100vh;
        background-color: ${theme.colorNeutralBackground1};
      `}>
      <Text>
        <FormattedMessage {...messages.AppDescription} />
      </Text>
    </div>
  );

}

export default React.memo(ConfigurePage);
