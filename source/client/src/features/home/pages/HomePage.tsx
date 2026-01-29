//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { useIntl } from 'react-intl';
import messages from '../messages';

import Presenter from './HomePage.presenter';

function HomePage() {

  const intl = useIntl();

  const handleLinkToGitHub = React.useCallback(() => {
    window.open(intl.formatMessage(messages.GitHubLink), '_blank', 'noreferrer');
  }, [
    intl
  ]);

  const handleLinkToPrivacyPolicy = React.useCallback(() => {
    window.open(intl.formatMessage(messages.PrivacyPolicyLink), '_blank', 'noreferrer');
  }, [
    intl
  ]);

  const handleLinkToTermsOfUse = React.useCallback(() => {
    window.open(intl.formatMessage(messages.TermsOfUseLink), '_blank', 'noreferrer');
  }, [
    intl
  ]);

  return (
    <Presenter
      onLinkToGitHub={handleLinkToGitHub}
      onLinkToPrivacyPolicy={handleLinkToPrivacyPolicy}
      onLinkToTermsOfUse={handleLinkToTermsOfUse} />
  );

}

export default HomePage;
