//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { Dropdown, Option } from '@fluentui/react-components';
import { EventHandler } from '../../../types/Event';

interface ScheduleDropdownProps {
  disabled?: boolean,
  options?: Record<string, string>,
  value?: string,
  onBlur?: EventHandler,
  onChange?: EventHandler<string>
}

function ScheduleDropdown(props: Readonly<ScheduleDropdownProps>, ref: React.Ref<HTMLButtonElement>) {

  const {
    disabled,
    options,
    value,
    onBlur,
    onChange
  } = props;

  return (
    <Dropdown
      ref={ref}
      defaultSelectedOptions={value ? [ value ] : undefined}
      defaultValue={value && options ? options[value] : undefined}
      disabled={disabled}
      onBlur={onBlur}
      onOptionSelect={(e, data) => onChange?.(e, data.optionValue)}>
      {
        options && Object.keys(options).sort((a, b) => Number(a) - Number(b)).map((item) => (
          <Option
            key={item}
            value={item}>
            {options[item]}
          </Option>
        ))
      }
    </Dropdown>
  );

}

export default React.memo(React.forwardRef(ScheduleDropdown));
