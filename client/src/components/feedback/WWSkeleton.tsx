import Box from '@mui/material/Box'
import Skeleton from '@mui/material/Skeleton'
import type { SkeletonProps } from '@mui/material/Skeleton'

interface WWSkeletonProps {
  variant?: SkeletonProps['variant']
  width?: number | string
  height?: number
  count?: number
}

export function WWSkeleton({ variant = 'text', width, height, count }: WWSkeletonProps) {
  if (count && count > 1) {
    return (
      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
        {Array.from({ length: count }, (_, i) => (
          <Skeleton key={i} variant={variant} width={width} height={height} />
        ))}
      </Box>
    )
  }

  return <Skeleton variant={variant} width={width} height={height} />
}
