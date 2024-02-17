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

interface Error500PageProps {
  error?: string
}

function Error500Page(props: Error500PageProps) {

  const { error } = props;

  const { theme } = useTheme();

  return (
    <div
      css={css`
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        min-height: 100vh;
        background-color: ${theme.colorNeutralBackground3};
      `}>
      <div
        css={css`
          display: flex;
          flex-direction: column;
          grid-gap: 0.5rem;
          align-items: center;
          justify-content: center;
        `}>
        <Text
          as="h1"
          css={css`
            font-size: 3rem;
            line-height: calc(3rem * 1.25);
            color: ${theme.colorNeutralForegroundDisabled};
          `}>
          <FormattedMessage {...messages.Error500Title} />
        </Text>
        <Text
          css={css`
            color: ${theme.colorNeutralForegroundDisabled};
          `}>
          <FormattedMessage {...messages.Error500Description} />
        </Text>
        <Text
          css={css`
            font-size: 0.75rem;
            line-height: calc(0.75rem * 1.25);
            color: ${theme.colorNeutralForegroundDisabled};
          `}>
          {error}
        </Text>
      </div>
    </div>
  );

}

export default React.memo(Error500Page);
