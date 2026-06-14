import Chip from '@mui/material/Chip'
import type { ChipProps } from '@mui/material/Chip'

type StatusColor = ChipProps['color']

const STATUS_COLOR_MAP: Record<string, StatusColor> = {
  Active: 'success',
  Discharged: 'default',
  Deceased: 'error',
  Critical: 'error',
  Inpatient: 'info',
  Scheduled: 'info',
  Confirmed: 'success',
  Cancelled: 'default',
  Completed: 'default',
  NoShow: 'warning',
  'No-show': 'warning',
  'Checked-in': 'primary',
  'In progress': 'warning',
}

interface WWStatusBadgeProps {
  status: string
  variant?: 'patient' | 'appointment'
}

export function WWStatusBadge({ status }: WWStatusBadgeProps) {
  const color: StatusColor = STATUS_COLOR_MAP[status] ?? 'default'

  return <Chip label={status} color={color} size="small" clickable={false} />
}
