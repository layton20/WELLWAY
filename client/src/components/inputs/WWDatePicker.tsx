import { DatePicker } from '@mui/x-date-pickers/DatePicker'
import type { Dayjs } from 'dayjs'

interface WWDatePickerProps {
  label: string
  name: string
  value?: Dayjs | null
  onChange?: (value: Dayjs | null) => void
  error?: boolean
  helperText?: string
  disabled?: boolean
  minDate?: Dayjs
  maxDate?: Dayjs
}

export function WWDatePicker({
  label,
  name,
  value,
  onChange,
  error,
  helperText,
  disabled,
  minDate,
  maxDate,
}: WWDatePickerProps) {
  return (
    <DatePicker
      label={label}
      value={value}
      onChange={onChange}
      disabled={disabled}
      minDate={minDate}
      maxDate={maxDate}
      format="DD/MM/YYYY"
      slotProps={{
        textField: {
          name,
          variant: 'outlined',
          size: 'small',
          fullWidth: true,
          error,
          helperText,
        },
      }}
    />
  )
}
