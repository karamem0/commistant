//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { FormattedMessage, useIntl } from 'react-intl';
import { Text } from '@fluentui/react-components';
import { css } from '@emotion/react';
import messages from '../messages';
import { useTheme } from '../../../providers/ThemeProvider';

function ConfigurePage() {

  const intl = useIntl();
  const { theme } = useTheme();

  return (
    <React.Fragment>
      <meta
        content={intl.formatMessage(messages.AppCreator)}
        name="author" />
      <meta
        content={intl.formatMessage(messages.AppDescription)}
        name="description" />
      <title>
        {intl.formatMessage(messages.AppTitle)}
      </title>
      <div
        css={css`
          min-height: 100vh;
          background-color: ${theme.colorNeutralBackground1};
        `}>
        <Text>
          <FormattedMessage {...messages.AppDescription} />
        </Text>
      </div>
    </React.Fragment>
  );

}

export default React.memo(ConfigurePage);
