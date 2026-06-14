import FormControl from '@mui/material/FormControl'
import FormHelperText from '@mui/material/FormHelperText'
import InputLabel from '@mui/material/InputLabel'
import MenuItem from '@mui/material/MenuItem'
import Select from '@mui/material/Select'
import type { SelectChangeEvent } from '@mui/material/Select'

export interface SelectOption {
  value: string | number
  label: string
}

interface WWSelectProps {
  label: string
  name: string
  value?: string | number
  onChange?: (event: SelectChangeEvent<string | number>) => void
  options: SelectOption[]
  error?: boolean
  helperText?: string
  disabled?: boolean
}

export function WWSelect({
  label,
  name,
  value,
  onChange,
  options,
  error,
  helperText,
  disabled,
}: WWSelectProps) {
  const labelId = `${name}-label`

  return (
    <FormControl variant="outlined" size="small" fullWidth disabled={disabled} error={error}>
      <InputLabel id={labelId}>{label}</InputLabel>
      <Select
        labelId={labelId}
        name={name}
        value={value ?? ''}
        onChange={onChange}
        label={label}
      >
        {options.map((option) => (
          <MenuItem key={option.value} value={option.value}>
            {option.label}
          </MenuItem>
        ))}
      </Select>
      {helperText && <FormHelperText>{helperText}</FormHelperText>}
    </FormControl>
  )
}
