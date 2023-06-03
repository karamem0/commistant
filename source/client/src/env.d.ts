//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

declare module 'ress';

interface ImportMetaEnv {
  readonly VITE_TELEMETRY_CONNECTION_STRING: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
