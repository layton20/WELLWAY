import Typography from '@mui/material/Typography'
import { useParams } from 'react-router-dom'
import { WWCard } from '@/components'

export function PatientDetailPage() {
  const { patientId } = useParams<{ patientId: string }>()

  return (
    <WWCard title="Patient Record">
      <Typography color="text.secondary">Patient {patientId} detail coming soon.</Typography>
    </WWCard>
  )
}
