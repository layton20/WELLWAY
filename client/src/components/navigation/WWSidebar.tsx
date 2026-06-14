import Box from '@mui/material/Box'
import Divider from '@mui/material/Divider'
import Drawer from '@mui/material/Drawer'
import List from '@mui/material/List'
import ListItem from '@mui/material/ListItem'
import ListItemButton from '@mui/material/ListItemButton'
import ListItemIcon from '@mui/material/ListItemIcon'
import ListItemText from '@mui/material/ListItemText'
import Typography from '@mui/material/Typography'
import AccountCircleOutlinedIcon from '@mui/icons-material/AccountCircleOutlined'
import CalendarMonthOutlinedIcon from '@mui/icons-material/CalendarMonthOutlined'
import DashboardOutlinedIcon from '@mui/icons-material/DashboardOutlined'
import GroupsOutlinedIcon from '@mui/icons-material/GroupsOutlined'
import PeopleOutlinedIcon from '@mui/icons-material/PeopleOutlined'
import type { SvgIconComponent } from '@mui/icons-material'
import { useLocation, useNavigate } from 'react-router-dom'
import { Routes } from '@/constants/routes'

const SIDEBAR_WIDTH = 240

interface NavItem {
  label: string
  icon: SvgIconComponent
  path: string
}

const navItems: NavItem[] = [
  { label: 'Dashboard', icon: DashboardOutlinedIcon, path: Routes.dashboard },
  { label: 'Patients', icon: PeopleOutlinedIcon, path: Routes.patients.list },
  { label: 'Appointments', icon: CalendarMonthOutlinedIcon, path: Routes.appointments.list },
  { label: 'Staff', icon: GroupsOutlinedIcon, path: Routes.staff.list },
]

interface WWSidebarProps {
  open?: boolean
  onClose?: () => void
  isMobile?: boolean
}

export function WWSidebar({ open = false, onClose, isMobile = false }: WWSidebarProps) {
  const location = useLocation()
  const navigate = useNavigate()

  const handleNav = (path: string) => {
    navigate(path)
    if (isMobile) onClose?.()
  }

  const drawerContent = (
    <>
      <Box sx={{ px: 3, py: 2.5, borderBottom: '1px solid', borderColor: 'divider' }}>
        <Typography
          sx={{
            fontFamily: '"Cormorant Garamond", Georgia, serif',
            fontWeight: 300,
            fontSize: 26,
            letterSpacing: '0.25em',
            textTransform: 'uppercase',
            color: 'primary.main',
            lineHeight: 1,
          }}
        >
          Wellway
        </Typography>
        <Typography
          variant="caption"
          sx={{
            letterSpacing: '0.14em',
            textTransform: 'uppercase',
            color: 'text.secondary',
            display: 'block',
            mt: 1,
          }}
        >
          Care, Simplified
        </Typography>
      </Box>

      <Box sx={{ flex: 1, overflow: 'auto', py: 1.5 }}>
        <List dense disablePadding>
          {navItems.map(({ label, icon: NavIcon, path }) => {
            const isActive =
              location.pathname === path || location.pathname.startsWith(`${path}/`)
            return (
              <ListItem key={label} disablePadding sx={{ px: 1.5, mb: 0.5 }}>
                <ListItemButton
                  selected={isActive}
                  onClick={() => handleNav(path)}
                  sx={{
                    borderRadius: 2,
                    '&.Mui-selected': {
                      bgcolor: 'primary.main',
                      color: 'primary.contrastText',
                      '& .MuiListItemIcon-root': { color: 'primary.contrastText' },
                      '&:hover': { bgcolor: 'primary.dark' },
                    },
                  }}
                >
                  <ListItemIcon sx={{ minWidth: 36 }}>
                    <NavIcon fontSize="small" />
                  </ListItemIcon>
                  <ListItemText
                    primary={label}
                    slotProps={{ primary: { sx: { fontSize: 14, fontWeight: 500 } } }}
                  />
                </ListItemButton>
              </ListItem>
            )
          })}
        </List>
      </Box>

      <Divider />
      <Box sx={{ p: 1.5 }}>
        <ListItemButton sx={{ borderRadius: 2 }}>
          <ListItemIcon sx={{ minWidth: 36 }}>
            <AccountCircleOutlinedIcon fontSize="small" />
          </ListItemIcon>
          <ListItemText
            primary="User"
            secondary="Role"
            slotProps={{
              primary: { sx: { fontSize: 13, fontWeight: 600 } },
              secondary: { sx: { fontSize: 11.5 } },
            }}
          />
        </ListItemButton>
      </Box>
    </>
  )

  return (
    <Drawer
      variant={isMobile ? 'temporary' : 'permanent'}
      open={isMobile ? open : true}
      onClose={onClose}
      sx={{
        width: isMobile ? 'auto' : SIDEBAR_WIDTH,
        flexShrink: 0,
        '& .MuiDrawer-paper': {
          width: SIDEBAR_WIDTH,
          boxSizing: 'border-box',
          border: 'none',
          borderRight: isMobile ? 'none' : '1px solid',
          borderColor: 'divider',
          display: 'flex',
          flexDirection: 'column',
        },
      }}
    >
      {drawerContent}
    </Drawer>
  )
}
