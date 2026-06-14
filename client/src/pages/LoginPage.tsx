import Box from '@mui/material/Box'
import Typography from '@mui/material/Typography'
import { WWCard } from '@/components'

export function LoginPage() {
  return (
    <Box sx={{ height: '100vh', display: 'grid', placeItems: 'center', bgcolor: 'background.default' }}>
      <Box sx={{ width: 400 }}>
        <WWCard title="Sign in">
          <Typography color="text.secondary">Login form coming soon.</Typography>
        </WWCard>
      </Box>
    </Box>
  )
}
