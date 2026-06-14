import { useState, useEffect } from 'react'
import Box from '@mui/material/Box'
import { useTheme } from '@mui/material/styles'
import useMediaQuery from '@mui/material/useMediaQuery'
import { Outlet, useLocation } from 'react-router-dom'
import { WWSidebar, WWTopBar } from '@/components'

export function Layout() {
  const theme = useTheme()
  const isMobile = useMediaQuery(theme.breakpoints.down('md'))
  const [drawerOpen, setDrawerOpen] = useState(false)
  const location = useLocation()

  // Close drawer on every route change (tap nav item on tablet/phone)
  useEffect(() => {
    if (isMobile) setDrawerOpen(false)
  }, [location.pathname, isMobile])

  return (
    <Box sx={{ display: 'flex', height: '100vh' }}>
      <WWSidebar
        open={drawerOpen}
        onClose={() => setDrawerOpen(false)}
        isMobile={isMobile}
      />
      <Box sx={{ flex: 1, display: 'flex', flexDirection: 'column', minWidth: 0 }}>
        <WWTopBar
          onMenuOpen={isMobile ? () => setDrawerOpen(true) : undefined}
        />
        <Box
          component="main"
          sx={{
            flex: 1,
            overflow: 'auto',
            p: { xs: '16px 16px 40px', md: '28px 32px 60px' },
          }}
        >
          <Outlet />
        </Box>
      </Box>
    </Box>
  )
}
