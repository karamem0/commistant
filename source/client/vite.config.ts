import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';
import env from 'vite-plugin-env-compatible';

export default defineConfig({
  build: {
    outDir: 'dist',
    sourcemap: true
  },
  plugins: [
    react({
      jsxImportSource: '@emotion/react',
      babel: {
        plugins: [
          '@emotion/babel-plugin'
        ]
      }
    }),
    env({
      prefix: 'APP'
    })
  ]
});