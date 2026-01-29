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
  Button,
  Image,
  Link,
  Text
} from '@fluentui/react-components';
import { SiGithub } from 'react-icons/si';
import { FormattedMessage, useIntl } from 'react-intl';
import { useTheme } from '../../../providers/ThemeProvider';
import { EventHandler } from '../../../types/Event';
import messages from '../messages';

interface HomePageProps {
  onLinkToGitHub?: EventHandler,
  onLinkToPrivacyPolicy?: EventHandler,
  onLinkToTermsOfUse?: EventHandler
}

function HomePage(props: Readonly<HomePageProps>) {

  const {
    onLinkToGitHub,
    onLinkToPrivacyPolicy,
    onLinkToTermsOfUse
  } = props;

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
      <meta
        content={intl.formatMessage(messages.AppTitle)}
        property="og:title" />
      <meta
        content="website"
        property="og:type" />
      <meta
        content={`${location.origin}/assets/screenshots/001.png`}
        property="og:image" />
      <meta
        content={location.origin}
        property="og:url" />
      <meta
        content={intl.formatMessage(messages.AppDescription)}
        property="og:description" />
      <title>
        {intl.formatMessage(messages.AppTitle)}
      </title>
      <div
        css={css`
          display: flex;
          flex-flow: column;
          background-color: ${theme.colorNeutralBackground3};
        `}>
        <header
          css={css`
            display: grid;
            grid-template-rows: auto;
            grid-template-columns: auto;
            align-items: center;
            justify-content: end;
            width: 100%;
            height: 2rem;
            padding: 0 1rem;
          `}>
          <Button
            appearance="transparent"
            as="a"
            icon={(
              <SiGithub
                css={css`
                  width: 1rem;
                  height: 1rem;
                `} />
            )}
            onClick={onLinkToGitHub}>
            <FormattedMessage {...messages.GitHub} />
          </Button>
        </header>
        <section
          css={css`
            display: grid;
            grid-template-rows: auto;
            grid-template-columns: auto;
            align-items: center;
            justify-content: center;
            width: 100%;
            padding: 2rem;
            background-color: ${theme.colorBrandBackground};
            @media (width >= 960px) {
              padding: 4rem 2rem;
            }
          `}>
          <div
            css={css`
              display: grid;
              grid-template-rows: auto;
              grid-template-columns: auto;
              align-items: center;
              justify-content: center;
              @media (width >= 960px) {
                grid-template-rows: auto;
                grid-template-columns: 1fr 1fr;
              }
            `}>
            <div
              css={css`
                display: grid;
                align-items: center;
                justify-content: center;
              `}>
              <Text
                as="h1"
                css={css`
                  font-size: 3rem;
                  font-weight: 700;
                  line-height: calc(3rem * 1.25);
                  color: #fff;
                  text-align: center;
                `}>
                <FormattedMessage {...messages.AppTitle} />
              </Text>
              <Text
                css={css`
                  color: ${theme.colorNeutralForegroundInverted};
                  text-align: center;
                `}>
                <FormattedMessage {...messages.AppDescription} />
              </Text>
            </div>
            <Image
              alt={intl.formatMessage(messages.AppTitle)}
              fit="contain"
              src="/assets/screenshots/001.png"
              css={css`
                height: auto;
              `} />
          </div>
        </section>
        <section
          css={css`
            display: flex;
            flex-flow: column;
            gap: 1rem;
            align-items: center;
            justify-content: center;
            padding: 2rem;
          `}>
          <Text
            as="h2"
            content=""
            css={css`
              font-size: 2rem;
              font-weight: 700;
              line-height: calc(2rem * 1.25);
              text-align: center;
            `}>
            <FormattedMessage {...messages.FeaturesTitle} />
          </Text>
          <div
            css={css`
              display: flex;
              flex-flow: column;
              gap: 0.5rem;
              align-items: center;
              justify-content: center;
            `}>
            <Text>
              <FormattedMessage {...messages.FeaturesDescription1} />
            </Text>
            <Text>
              <FormattedMessage {...messages.FeaturesDescription2} />
            </Text>
          </div>
          <Text
            as="h2"
            css={css`
              font-size: 2rem;
              font-weight: 700;
              line-height: calc(2rem * 1.25);
              text-align: center;
            `}>
            <FormattedMessage {...messages.ScenesTitle} />
          </Text>
          <div
            css={css`
              display: flex;
              flex-flow: column;
              gap: 0.5rem;
              align-items: center;
              justify-content: center;
            `}>
            <Text>
              <FormattedMessage {...messages.ScenesDescription1} />
            </Text>
            <Text>
              <FormattedMessage {...messages.ScenesDescription2} />
            </Text>
            <Text>
              <FormattedMessage {...messages.ScenesDescription3} />
            </Text>
          </div>
        </section>
        <footer
          css={css`
            display: flex;
            flex-flow: row;
            align-items: center;
            justify-content: center;
            padding: 2rem 0;
          `}>
          <Link
            as="button"
            onClick={onLinkToTermsOfUse}>
            <FormattedMessage {...messages.TermsOfUse} />
          </Link>
          <Text
            css={css`
              padding: 0 0.25rem;
            `}>
            |
          </Text>
          <Link
            as="button"
            onClick={onLinkToPrivacyPolicy}>
            <FormattedMessage {...messages.PrivacyPolicy} />
          </Link>
        </footer>
      </div>
    </React.Fragment>
  );

}

export default React.memo(HomePage);
