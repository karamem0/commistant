//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { EventHandler } from '../../../types/Event';

import Presenter from './ScheduleDropdown.presenter';

interface ScheduleDropdownProps {
  options?: Record<string, string>,
  value?: string,
  onBlur?: EventHandler,
  onChange?: EventHandler<string>
}

function ScheduleDropdown(props: ScheduleDropdownProps, ref: React.Ref<HTMLButtonElement>) {

  const {
    options,
    value,
    onBlur,
    onChange
  } = props;

  return (
    <Presenter
      ref={ref}
      options={options}
      value={value}
      onBlur={onBlur}
      onChange={onChange} />
  );

}

export default React.forwardRef(ScheduleDropdown);
