import AppBar from '@mui/material/AppBar'
import Box from '@mui/material/Box'
import IconButton from '@mui/material/IconButton'
import InputBase from '@mui/material/InputBase'
import Toolbar from '@mui/material/Toolbar'
import Typography from '@mui/material/Typography'
import DarkModeOutlinedIcon from '@mui/icons-material/DarkModeOutlined'
import LightModeOutlinedIcon from '@mui/icons-material/LightModeOutlined'
import MenuIcon from '@mui/icons-material/Menu'
import NotificationsOutlinedIcon from '@mui/icons-material/NotificationsOutlined'
import SearchIcon from '@mui/icons-material/Search'
import AccountCircleOutlinedIcon from '@mui/icons-material/AccountCircleOutlined'
import { useThemeMode } from '@/hooks/useThemeMode'

interface WWTopBarProps {
  onMenuOpen?: () => void
}

export function WWTopBar({ onMenuOpen }: WWTopBarProps) {
  const { mode, toggleThemeMode } = useThemeMode()

  return (
    <AppBar
      position="static"
      color="transparent"
      elevation={0}
      sx={{ borderBottom: '1px solid', borderColor: 'divider', bgcolor: 'background.paper' }}
    >
      <Toolbar sx={{ gap: 1 }}>
        {onMenuOpen && (
          <IconButton
            onClick={onMenuOpen}
            aria-label="open navigation"
            size="small"
            edge="start"
            sx={{ mr: 0.5 }}
          >
            <MenuIcon />
          </IconButton>
        )}

        <Typography
          variant="h6"
          sx={{ fontWeight: 600, fontSize: 16, flexShrink: 0, color: 'text.primary' }}
        >
          Page title
        </Typography>

        {/* Search — hidden on phones, visible from sm up */}
        <Box
          sx={{
            flex: '0 1 440px',
            display: { xs: 'none', sm: 'flex' },
            alignItems: 'center',
            bgcolor: 'background.default',
            borderRadius: 2.5,
            border: '1px solid',
            borderColor: 'divider',
            px: 1.5,
            height: 40,
            ml: 2,
          }}
        >
          <SearchIcon sx={{ color: 'text.secondary', mr: 1, fontSize: 18 }} />
          <InputBase
            placeholder="Search patients — name, NHS number, or DOB"
            sx={{ flex: 1, fontSize: 14 }}
            inputProps={{ 'aria-label': 'global patient search' }}
          />
        </Box>

        {/* Search icon-only button on phones */}
        <IconButton
          aria-label="search"
          size="small"
          sx={{ display: { xs: 'flex', sm: 'none' } }}
        >
          <SearchIcon />
        </IconButton>

        <Box sx={{ flex: 1 }} />

        <IconButton
          onClick={toggleThemeMode}
          aria-label={mode === 'dark' ? 'switch to light mode' : 'switch to dark mode'}
          size="small"
        >
          {mode === 'dark' ? <LightModeOutlinedIcon /> : <DarkModeOutlinedIcon />}
        </IconButton>

        <IconButton aria-label="notifications" size="small">
          <NotificationsOutlinedIcon />
        </IconButton>

        <IconButton aria-label="user account" size="small">
          <AccountCircleOutlinedIcon />
        </IconButton>
      </Toolbar>
    </AppBar>
  )
}
