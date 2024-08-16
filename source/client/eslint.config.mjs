//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import path from 'path';
import { fileURLToPath } from 'url';

import { fixupConfigRules, fixupPluginRules } from '@eslint/compat';
import { FlatCompat } from '@eslint/eslintrc';
import js from '@eslint/js';
import hooks from 'eslint-plugin-hooks';
import sonarjs from 'eslint-plugin-sonarjs';
import globals from 'globals';

const compat = new FlatCompat({
  baseDirectory: path.dirname(fileURLToPath(import.meta.url)),
  recommendedConfig: js.configs.recommended,
  allConfig: js.configs.all
});

export default [
  ...fixupConfigRules(compat.extends(
    'eslint:recommended',
    'plugin:react/recommended',
    'plugin:react-hooks/recommended',
    'plugin:@typescript-eslint/recommended',
    'plugin:sonarjs/recommended-legacy',
    'standard'
  )),
  {
    'plugins': {
      hooks,
      'sonarjs': fixupPluginRules(sonarjs)
    },
    'languageOptions': {
      'globals': {
        ...globals.browser,
        ...globals.jest
      }
    },
    'settings': {
      'react': {
        'version': 'detect'
      }
    },
    'rules': {
      'array-bracket-spacing': [
        'error',
        'always',
        {
          'arraysInArrays': false
        }
      ],
      'arrow-parens': [
        'error',
        'always'
      ],
      'arrow-spacing': 'error',
      'dot-notation': [
        'error',
        {
          'allowPattern': '^[a-z]+(_[a-z]+)+$'
        }
      ],
      'hooks/sort': [
        'error',
        {
          'groups': [
            'useContext',
            'useReducer',
            'useState',
            'useMemo',
            'useRef',
            'useCallback',
            'useEffect'
          ]
        }
      ],
      'key-spacing': [
        'error',
        {
          'afterColon': true
        }
      ],
      'linebreak-style': [
        'error',
        'unix'
      ],
      'multiline-ternary': [
        'error',
        'never'
      ],
      'no-alert': 'error',
      'no-console': [
        'warn',
        {
          'allow': [
            'error'
          ]
        }
      ],
      'no-unused-vars': 'off',
      'no-use-before-define': 'off',
      'no-var': 'error',
      'operator-linebreak': [
        'error',
        'after'
      ],
      'padded-blocks': 'off',
      'quote-props': [
        'error',
        'consistent'
      ],
      'quotes': [
        'error', 'single'
      ],
      'semi': [
        'error', 'always'
      ],
      'sort-imports': 'off',
      'space-before-function-paren': [
        'error',
        {
          'anonymous': 'never',
          'named': 'never',
          'asyncArrow': 'always'
        }
      ],
      '@typescript-eslint/array-type': [
        'error',
        {
          'default': 'array'
        }
      ],
      '@typescript-eslint/member-delimiter-style': [
        'error',
        {
          'multiline': {
            'delimiter': 'comma',
            'requireLast': false
          },
          'singleline': {
            'delimiter': 'comma',
            'requireLast': false
          }
        }
      ],
      '@typescript-eslint/no-unused-vars': 'error',
      '@typescript-eslint/no-use-before-define': 'error',
      'import/order': [
        'error',
        {
          'pathGroups': [
            {
              'pattern': 'react',
              'group': 'builtin',
              'position': 'before'
            },
            {
              'pattern': 'react-dom/**',
              'group': 'builtin',
              'position': 'before'
            },
            {
              'pattern': 'react**',
              'group': 'builtin',
              'position': 'before'
            },
            {
              'pattern': '@automapper/**',
              'group': 'builtin',
              'position': 'after'
            },
            {
              'pattern': '@fluentui/**',
              'group': 'builtin',
              'position': 'after'
            },
            {
              'pattern': '@microsoft/**',
              'group': 'builtin',
              'position': 'after'
            },
            {
              'pattern': '@azure/**',
              'group': 'builtin',
              'position': 'after'
            }
          ],
          'pathGroupsExcludedImportTypes': [
            'react',
            'react-dom/**',
            'react**',
            '@automapper/**',
            '@fluentui/**',
            '@microsoft/**',
            '@azure/**'
          ],
          'alphabetize': {
            'order': 'asc'
          },
          'newlines-between': 'always'
        }
      ],
      'react/jsx-closing-bracket-location': [
        'error',
        'after-props'
      ],
      'react/jsx-first-prop-new-line': [
        'error',
        'multiline'
      ],
      'react/jsx-indent': [
        'error',
        2
      ],
      'react/jsx-indent-props': [
        'error',
        2
      ],
      'react/jsx-max-props-per-line': [
        'error',
        {
          'maximum': 1
        }
      ],
      'react/jsx-sort-props': [
        'error',
        {
          'callbacksLast': true,
          'multiline': 'last',
          'reservedFirst': true
        }
      ],
      'react/jsx-tag-spacing': [
        'error',
        {
          'beforeSelfClosing': 'always'
        }
      ],
      'react/no-unknown-property': [
        'error',
        {
          'ignore': [
            'css'
          ]
        }
      ],
      'react/prop-types': 'off',
      'sonarjs/no-collapsible-if': 'warn',
      'sonarjs/no-duplicate-string': 'off',
      'sonarjs/no-small-switch': 'warn',
      'sonarjs/prefer-single-boolean-return': 'off'
    }
  }
];
