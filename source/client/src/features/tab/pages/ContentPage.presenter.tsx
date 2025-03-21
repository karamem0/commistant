//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import {
  Button,
  Field,
  Input,
  Subtitle2,
  Textarea
} from '@fluentui/react-components';
import { Controller, useForm } from 'react-hook-form';
import { FormattedMessage, useIntl } from 'react-intl';
import { CommandSettingsFormState } from '../../../types/Form';
import { EventHandler } from '../../../types/Event';
import ScheduleDropdown from '../components/ScheduleDropdown';
import { css } from '@emotion/react';
import messages from '../messages';
import { useTheme } from '../../../providers/ThemeProvider';

interface ContentPageProps {
  disabled?: boolean,
  value?: CommandSettingsFormState,
  onSubmit?: EventHandler<CommandSettingsFormState>
}

function ContentPage(props: Readonly<ContentPageProps>) {

  const {
    disabled,
    value,
    onSubmit
  } = props;

  const intl = useIntl();
  const { theme } = useTheme();
  const form = useForm<CommandSettingsFormState>({
    defaultValues: value
  });

  React.useEffect(() => {
    form.reset(value);
  }, [
    form,
    value
  ]);

  return value ? (
    <div
      css={css`
        min-height: 100vh;
        padding: 1rem;
        background-color: ${theme.colorNeutralBackground3};
      `}>
      <form
        css={css`
          display: grid;
          grid-template-rows: auto auto;
          grid-template-columns: 1fr;
          gap: 1rem;
        `}
        onSubmit={form.handleSubmit((formState) => onSubmit?.({}, formState))}>
        <div
          css={css`
            display: grid;
            grid-template-rows: auto;
            grid-template-columns: 1fr;
            gap: 1rem;
            align-items: start;
            justify-content: center;
            width: 100%;
            @media (width >= 960px) {
              grid-template-rows: auto;
              grid-template-columns: 1fr 1fr 1fr;
            }
          `}>
          <div>
            <Subtitle2>
              <FormattedMessage {...messages.StartMeeting} />
            </Subtitle2>
            <Controller
              control={form.control}
              defaultValue={value.startMeetingSchedule}
              name="startMeetingSchedule"
              render={({ field }) => (
                <Field label={intl.formatMessage(messages.Schedule)}>
                  <ScheduleDropdown
                    ref={field.ref}
                    disabled={disabled}
                    value={field.value}
                    options={{
                      '-1': intl.formatMessage(messages.None),
                      '0': intl.formatMessage(messages.InTime),
                      '5': intl.formatMessage(messages.MinutesAfter, { value: 5 }),
                      '10': intl.formatMessage(messages.MinutesAfter, { value: 10 }),
                      '15': intl.formatMessage(messages.MinutesAfter, { value: 15 })
                    }}
                    onBlur={field.onBlur}
                    onChange={(_, value) => field.onChange(value || '')} />
                </Field>
              )} />
            <Controller
              control={form.control}
              defaultValue={value.startMeetingMessage}
              name="startMeetingMessage"
              render={({ field }) => (
                <Field label={intl.formatMessage(messages.Message)}>
                  <Textarea
                    {...field}
                    disabled={disabled} />
                </Field>
              )} />
            <Controller
              control={form.control}
              defaultValue={value.startMeetingUrl}
              name="startMeetingUrl"
              render={({ field }) => (
                <Field label={intl.formatMessage(messages.Url)}>
                  <Input
                    {...field}
                    disabled={disabled} />
                </Field>
              )} />
          </div>
          <div>
            <Subtitle2>
              <FormattedMessage {...messages.EndMeeting} />
            </Subtitle2>
            <Controller
              control={form.control}
              defaultValue={value.endMeetingSchedule}
              name="endMeetingSchedule"
              render={({ field }) => (
                <Field label={intl.formatMessage(messages.Schedule)}>
                  <ScheduleDropdown
                    ref={field.ref}
                    disabled={disabled}
                    value={field.value}
                    options={{
                      '-1': intl.formatMessage(messages.None),
                      '0': intl.formatMessage(messages.InTime),
                      '5': intl.formatMessage(messages.MinutesBefore, { value: 5 }),
                      '10': intl.formatMessage(messages.MinutesBefore, { value: 10 }),
                      '15': intl.formatMessage(messages.MinutesBefore, { value: 15 })
                    }}
                    onBlur={field.onBlur}
                    onChange={(_, value) => field.onChange(value || '')} />
                </Field>
              )} />
            <Controller
              control={form.control}
              defaultValue={value.endMeetingMessage}
              name="endMeetingMessage"
              render={({ field }) => (
                <Field label={intl.formatMessage(messages.Message)}>
                  <Textarea
                    {...field}
                    disabled={disabled} />
                </Field>
              )} />
            <Controller
              control={form.control}
              defaultValue={value.endMeetingUrl}
              name="endMeetingUrl"
              render={({ field }) => (
                <Field label={intl.formatMessage(messages.Url)}>
                  <Input
                    {...field}
                    disabled={disabled} />
                </Field>
              )} />
          </div>
          <div>
            <Subtitle2>
              <FormattedMessage {...messages.InMeeting} />
            </Subtitle2>
            <Controller
              control={form.control}
              defaultValue={value.inMeetingSchedule}
              name="inMeetingSchedule"
              render={({ field }) => (
                <Field label={intl.formatMessage(messages.Schedule)}>
                  <ScheduleDropdown
                    ref={field.ref}
                    disabled={disabled}
                    value={field.value}
                    options={{
                      '-1': intl.formatMessage(messages.None),
                      '5': intl.formatMessage(messages.Minutes, { value: 5 }),
                      '10': intl.formatMessage(messages.Minutes, { value: 10 }),
                      '15': intl.formatMessage(messages.Minutes, { value: 15 }),
                      '30': intl.formatMessage(messages.Minutes, { value: 30 }),
                      '60': intl.formatMessage(messages.Minutes, { value: 60 })
                    }}
                    onBlur={field.onBlur}
                    onChange={(_, value) => field.onChange(value || '')} />
                </Field>
              )} />
            <Controller
              control={form.control}
              defaultValue={value.inMeetingMessage}
              name="inMeetingMessage"
              render={({ field }) => (
                <Field label={intl.formatMessage(messages.Message)}>
                  <Textarea
                    {...field}
                    disabled={disabled} />
                </Field>
              )} />
            <Controller
              control={form.control}
              defaultValue={value.inMeetingUrl}
              name="inMeetingUrl"
              render={({ field }) => (
                <Field label={intl.formatMessage(messages.Url)}>
                  <Input
                    {...field}
                    disabled={disabled} />
                </Field>
              )} />
          </div>
        </div>
        <div
          css={css`
            display: flex;
            flex-direction: column;
            align-items: end;
            justify-content: center;
          `}>
          <Button
            appearance="primary"
            aria-label={intl.formatMessage(messages.Save)}
            disabled={disabled || !form.formState.isDirty}
            title={intl.formatMessage(messages.Save)}
            type="submit">
            <FormattedMessage {...messages.Save} />
          </Button>
        </div>
      </form>
    </div>
  ) : null;

}

export default React.memo(ContentPage);
