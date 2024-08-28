//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { Event } from '../../../types/Event';
import Presenter from './HomePage.presenter';

function HomePage() {

  const handleLinkClick = React.useCallback((_?: Event, data?: string) => {
    switch (data) {
      case 'GitHub':
        window.open('https://github.com/karamem0/commistant', '_blank');
        break;
      case 'TermsOfUse':
        window.open('https://github.com/karamem0/commistant/blob/main/TERMS_OF_USE.md', '_blank');
        break;
      case 'PrivacyPolicy':
        window.open('https://github.com/karamem0/commistant/blob/main/PRIVACY.md', '_blank');
        break;
      default:
        break;
    }
  }, []);

  return (
    <Presenter onLinkClick={handleLinkClick} />
  );

}

export default HomePage;
