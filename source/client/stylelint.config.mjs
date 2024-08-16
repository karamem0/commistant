//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

export default {
  'extends': [
    'stylelint-config-standard-scss',
    'stylelint-config-recess-order'
  ],
  'overrides': [
    {
      'customSyntax': 'postcss-styled-syntax',
      'files': [
        'src/**/*.{jsx,tsx}'
      ],
      'rules': {
        'at-rule-empty-line-before': 'never',
        'declaration-empty-line-before': 'never',
        'rule-empty-line-before': 'never'
      }
    }
  ]
};
