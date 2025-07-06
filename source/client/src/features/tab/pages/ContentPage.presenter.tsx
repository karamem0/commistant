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
import { Controller, useForm, useWatch } from 'react-hook-form';
import { FormattedMessage, useIntl } from 'react-intl';
import { Helmet, HelmetProvider } from 'react-helmet-async';
import { CommandSettingsFormState } from '../../../types/Form';
import { EventHandler } from '../../../types/Event';
import ScheduleDropdown from '../components/ScheduleDropdown';
import { css } from '@emotion/react';
import messages from '../messages';
import { useTheme } from '../../../providers/ThemeProvider';

interface ContentPageProps {
  disabled?: boolean,
  loading?: boolean,
  value?: CommandSettingsFormState,
  onSubmit?: EventHandler<CommandSettingsFormState>
}

function ContentPage(props: Readonly<ContentPageProps>) {

  const {
    disabled,
    loading,
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

  const watch = useWatch({
    control: form.control
  });

  return (
    <React.Fragment>
      <HelmetProvider>
        <Helmet>
          <meta
            content={intl.formatMessage(messages.AppCreator)}
            name="author" />
          <meta
            content={intl.formatMessage(messages.AppDescription)}
            name="description" />
          <title>
            {intl.formatMessage(messages.AppTitle)}
          </title>
        </Helmet>
      </HelmetProvider>
      {
        value ? (
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
                    <FormattedMessage {...messages.MeetingStart} />
                  </Subtitle2>
                  <Controller
                    control={form.control}
                    defaultValue={value.meetingStartSchedule}
                    name="meetingStartSchedule"
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
                    defaultValue={value.meetingStartMessage}
                    name="meetingStartMessage"
                    render={({ field }) => (
                      <Field label={intl.formatMessage(messages.Message)}>
                        <Textarea
                          {...field}
                          disabled={disabled || watch.meetingStartSchedule === '-1'} />
                      </Field>
                    )} />
                  <Controller
                    control={form.control}
                    defaultValue={value.meetingStartUrl}
                    name="meetingStartUrl"
                    render={({ field }) => (
                      <Field label={intl.formatMessage(messages.Url)}>
                        <Input
                          {...field}
                          disabled={disabled || watch.meetingStartSchedule === '-1'} />
                      </Field>
                    )} />
                </div>
                <div>
                  <Subtitle2>
                    <FormattedMessage {...messages.MeetingEnd} />
                  </Subtitle2>
                  <Controller
                    control={form.control}
                    defaultValue={value.meetingEndSchedule}
                    name="meetingEndSchedule"
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
                    defaultValue={value.meetingEndMessage}
                    name="meetingEndMessage"
                    render={({ field }) => (
                      <Field label={intl.formatMessage(messages.Message)}>
                        <Textarea
                          {...field}
                          disabled={disabled || watch.meetingEndSchedule === '-1'} />
                      </Field>
                    )} />
                  <Controller
                    control={form.control}
                    defaultValue={value.meetingEndUrl}
                    name="meetingEndUrl"
                    render={({ field }) => (
                      <Field label={intl.formatMessage(messages.Url)}>
                        <Input
                          {...field}
                          disabled={disabled || watch.meetingEndSchedule === '-1'} />
                      </Field>
                    )} />
                </div>
                <div>
                  <Subtitle2>
                    <FormattedMessage {...messages.MeetingRun} />
                  </Subtitle2>
                  <Controller
                    control={form.control}
                    defaultValue={value.meetingRunSchedule}
                    name="meetingRunSchedule"
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
                    defaultValue={value.meetingRunMessage}
                    name="meetingRunMessage"
                    render={({ field }) => (
                      <Field label={intl.formatMessage(messages.Message)}>
                        <Textarea
                          {...field}
                          disabled={disabled || watch.meetingRunSchedule === '-1'} />
                      </Field>
                    )} />
                  <Controller
                    control={form.control}
                    defaultValue={value.meetingRunUrl}
                    name="meetingRunUrl"
                    render={({ field }) => (
                      <Field label={intl.formatMessage(messages.Url)}>
                        <Input
                          {...field}
                          disabled={disabled || watch.meetingRunSchedule === '-1'} />
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
                  disabled={disabled || loading || !form.formState.isDirty}
                  title={intl.formatMessage(messages.Save)}
                  type="submit">
                  <FormattedMessage {...messages.Save} />
                </Button>
              </div>
            </form>
          </div>
        ) : null
      }
    </React.Fragment>
  );

}

export default React.memo(ContentPage);
