import { createTheme } from '@mui/material/styles'
import type { ThemeOptions } from '@mui/material/styles'

const sharedOptions: Pick<ThemeOptions, 'typography' | 'shape' | 'components'> = {
  typography: {
    fontFamily: 'Inter, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
    fontSize: 14,
  },
  shape: {
    borderRadius: 10,
  },
  components: {
    MuiPaper: {
      defaultProps: { elevation: 0 },
      styleOverrides: {
        root: ({ theme }) => ({
          border: `1px solid ${theme.palette.divider}`,
        }),
      },
    },
    MuiCard: {
      defaultProps: { elevation: 0 },
      styleOverrides: {
        root: { borderRadius: 14 },
      },
    },
    MuiButton: {
      defaultProps: { disableRipple: true, disableElevation: true },
    },
    MuiIconButton: {
      defaultProps: { disableRipple: true },
    },
    MuiChip: {
      styleOverrides: {
        root: { cursor: 'default' },
      },
    },
  },
}

export const lightTheme = createTheme({
  ...sharedOptions,
  palette: {
    mode: 'light',
    background: { default: '#F8F9FA', paper: '#FFFFFF' },
    primary: { main: '#0D9488', dark: '#0F766E' },
    text: { primary: '#111827', secondary: '#6B7280' },
    divider: '#E5E7EB',
    error: { main: '#DC2626' },
    warning: { main: '#D97706' },
    success: { main: '#16A34A' },
    info: { main: '#2563EB' },
  },
})

export const darkTheme = createTheme({
  ...sharedOptions,
  palette: {
    mode: 'dark',
    background: { default: '#0F172A', paper: '#1E293B' },
    primary: { main: '#14B8A6', dark: '#2DD4BF' },
    text: { primary: '#F1F5F9', secondary: '#94A3B8' },
    divider: '#334155',
    error: { main: '#EF4444' },
    warning: { main: '#F59E0B' },
    success: { main: '#22C55E' },
    info: { main: '#60A5FA' },
  },
})
