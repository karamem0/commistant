//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { GitHubLogoIcon } from '@fluentui/react-icons-mdl2';
import {
  teamsTheme,
  Provider,
  Button,
  Header,
  Image,
  Text
} from '@fluentui/react-northstar';

import { css } from '@emotion/react';

export default React.memo(function App () {

  const handleGitHubClick = React.useCallback(() => {
    window.open('https://github.com/karamem0/commistant', '_blank');
  }, []);

  const handlePrivacyClick = React.useCallback(() => {
    window.open('https://github.com/karamem0/commistant/blob/main/PRIVACY.md', '_blank');
  }, []);

  const handleTermsOfUseClick = React.useCallback(() => {
    window.open('https://github.com/karamem0/commistant/blob/main/TERMS_OF_USE.md', '_blank');
  }, []);

  return (
    <Provider theme={teamsTheme}>
      <div
        css={css`
          display: flex;
          flex-flow: column;
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
            content="GitHub"
            icon={<GitHubLogoIcon />}
            text
            onClick={handleGitHubClick} />
        </header>
        <section
          css={css`
            display: grid;
            grid-template-rows: auto;
            grid-template-columns: auto;
            align-items: center;
            justify-content: center;
            width: 100%;
            background-color: #ea5549;
            @media (max-width: 959px) {
              padding: 2rem;
            }
            @media (min-width: 960px) {
              padding: 4rem 2rem;
            }
          `}>
          <div
            css={css`
              display: grid;
              gap: 1rem;
              @media (max-width: 959px) {
                grid-template-rows: auto;
                grid-template-columns: auto;
              }
              @media (min-width: 960px) {
                grid-template-rows: auto;
                grid-template-columns: 1fr 1fr;
              }
              align-items: center;
              justify-content: center;
            `}>
            <div
              css={css`
                display: grid;
                align-items: center;
                justify-content: center;
              `}>
              <Header
                as="h1"
                content={
                  <span
                    css={css`
                      color: #fff;
                  `}>
                    Commistant
                  </span>
                }
                css={css`
                  font-size: 3rem;
                  line-height: 3rem;
                  text-align: center;
                `}>
              </Header>
              <Text
                content="Microsoft Teams 会議によるコミュニティ イベントをサポートするアシスタント ボットです。"
                css={css`
                  font-size: 1rem;
                  line-height: 1rem;
                  color: #fff;
                  text-align: center;
                `} />
            </div>
            <Image
              fluid
              src="/assets/screenshot1.png" />
          </div>
        </section>
        <section
          css={css`
            display: flex;
            flex-flow: column;
            align-items: center;
            justify-content: center;
            padding: 0 2rem 0 2rem;
          `}>
          <Header
            as="h2"
            content="機能"
            css={css`
              font-size: 2rem;
              line-height: 2rem;
              text-align: center;
            `} />
          <Text
            css={css`
              display: flex;
              flex-flow: column;
              gap: 0.5rem;
              align-items: center;
              justify-content: center;
            `}
            size="large">
            <Text content="Commistant は会議の開始時、終了時、または会議中に定型のメッセージ通知を送信します。" />
            <Text content="通知にはテキストおよび QR コードつきの URL を添付することができます。" />
          </Text>
          <Header
            as="h2"
            content="利用シーン"
            css={css`
              font-size: 2rem;
              line-height: 2rem;
              text-align: center;
            `} />
          <Text
            css={css`
              display: flex;
              flex-flow: column;
              gap: 0.5rem;
              align-items: center;
              justify-content: center;
            `}
            size="large">
            <Text content="新型コロナウイルスの流行によりコミュニティ イベントはオンライン開催することが主流になりました。" />
            <Text content="開催者は参加者に対して定型のメッセージを送ることがありますが、定期的にメッセージを送ることは開催者の負担となります。" />
            <Text content="Commistant を使うことで、会議中に slido などの Q&A サービスに誘導することや、会議の最初や最後にアンケートの URL を送ることなどが簡単に実現できるようになります。" />
          </Text>
        </section>
        <footer
          css={css`
            display: flex;
            flex-flow: row;
            align-items: center;
            justify-content: center;
            padding: 2rem 0;
          `}>
          <Button
            content="Terms of Use"
            text
            onClick={handleTermsOfUseClick} />
          <Text content="|" />
          <Button
            content="Privacy Policy"
            text
            onClick={handlePrivacyClick} />
        </footer>
      </div>
    </Provider>
  );

});
