//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import fs from 'fs';

import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  'build': {
    'outDir': 'build',
    'sourcemap': true
  },
  'optimizeDeps': {
    'esbuildOptions': {
      'define': {
        'global': 'globalThis'
      }
    }
  },
  'plugins': [
    react({
      'jsxImportSource': '@emotion/react',
      'babel': {
        'plugins': [
          '@emotion',
          [
            'formatjs',
            {
              'ast': true,
              'idInterpolationPattern': '[sha512:contenthash:base64:6]'
            }
          ]
        ]
      }
    })
  ],
  'server': {
    'https': {
      'cert': fs.readFileSync('./cert/localhost.crt'),
      'key': fs.readFileSync('./cert/localhost.key')
    },
    'proxy': {
      '/api': {
        'changeOrigin': true,
        'secure': false,
        'target': 'https://localhost:5001'
      }
    }
  }
});
