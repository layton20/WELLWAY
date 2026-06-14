import Card from '@mui/material/Card'
import CardActions from '@mui/material/CardActions'
import CardContent from '@mui/material/CardContent'
import CardHeader from '@mui/material/CardHeader'
import type { ReactNode } from 'react'

interface WWCardProps {
  title?: string
  children: ReactNode
  actions?: ReactNode
}

export function WWCard({ title, children, actions }: WWCardProps) {
  return (
    <Card>
      {title && (
        <CardHeader
          title={title}
          titleTypographyProps={{ variant: 'h6', fontWeight: 650, fontSize: 15 }}
        />
      )}
      <CardContent>{children}</CardContent>
      {actions && <CardActions>{actions}</CardActions>}
    </Card>
  )
}
