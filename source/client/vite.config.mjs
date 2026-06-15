//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import fs from 'fs';

import babel from '@rolldown/plugin-babel';
import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';

export default defineConfig({
  'build': {
    'outDir': 'build',
    'rollupOptions': {
      onLog(logLevel, rolldownLog, defaultHandler) {
        if (rolldownLog.code !== 'INVALID_ANNOTATION') {
          defaultHandler(logLevel, rolldownLog);
        }
      }
    },
    'sourcemap': true
  },
  'plugins': [
    babel({
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
    }),
    react({
      'jsxImportSource': '@emotion/react'
    })
  ],
  'server': {
    'https': {
      'cert': fs.readFileSync('./cert/localhost.crt'),
      'key': fs.readFileSync('./cert/localhost.key')
    },
    'port': process.env.PORT ? Number(process.env.PORT) : 5173,
    'proxy': {
      '/api': {
        'changeOrigin': true,
        'secure': false,
        'target': 'https://localhost:5001'
      }
    }
  }
});
