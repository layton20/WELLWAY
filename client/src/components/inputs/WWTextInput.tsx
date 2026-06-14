import TextField from '@mui/material/TextField'
import type { ChangeEventHandler } from 'react'

interface WWTextInputProps {
  label: string
  name: string
  value?: string
  onChange?: ChangeEventHandler<HTMLInputElement | HTMLTextAreaElement>
  error?: boolean
  helperText?: string
  disabled?: boolean
  placeholder?: string
  multiline?: boolean
  rows?: number
  type?: string
}

export function WWTextInput({
  label,
  name,
  value,
  onChange,
  error,
  helperText,
  disabled,
  placeholder,
  multiline,
  rows,
  type,
}: WWTextInputProps) {
  return (
    <TextField
      label={label}
      name={name}
      value={value}
      onChange={onChange}
      error={error}
      helperText={helperText}
      disabled={disabled}
      placeholder={placeholder}
      multiline={multiline}
      rows={rows}
      type={type}
      variant="outlined"
      size="small"
      fullWidth
    />
  )
}
