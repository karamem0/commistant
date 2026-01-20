//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

/// <reference types="vite/client" />

declare module 'ress';

interface ImportMeta {
  readonly env: ImportMetaEnv
}

interface ImportMetaEnv {
  readonly VITE_FUNCTION_APP_URL: string,
  readonly VITE_TELEMETRY_CONNECTION_STRING: string
}
