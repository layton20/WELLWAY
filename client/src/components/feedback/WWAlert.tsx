import Alert from '@mui/material/Alert'
import type { AlertProps } from '@mui/material/Alert'

interface WWAlertProps {
  severity: AlertProps['severity']
  message: string
  onClose?: () => void
}

export function WWAlert({ severity, message, onClose }: WWAlertProps) {
  return (
    <Alert severity={severity} variant="outlined" onClose={onClose}>
      {message}
    </Alert>
  )
}
