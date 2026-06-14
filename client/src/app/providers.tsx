import CssBaseline from '@mui/material/CssBaseline'
import { ThemeProvider } from '@mui/material/styles'
import { LocalizationProvider } from '@mui/x-date-pickers'
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import { QueryClientProvider } from '@tanstack/react-query'
import type { ReactNode } from 'react'
import { queryClient } from '@/lib/queryClient'
import { ThemeModeContext, useThemeModeState } from '@/hooks/useThemeMode'
import { lightTheme, darkTheme } from './theme'

function ThemeWrapper({ children }: { children: ReactNode }) {
  const themeModeState = useThemeModeState()
  const theme = themeModeState.mode === 'dark' ? darkTheme : lightTheme

  return (
    <ThemeModeContext.Provider value={themeModeState}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        {children}
      </ThemeProvider>
    </ThemeModeContext.Provider>
  )
}

export function Providers({ children }: { children: ReactNode }) {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeWrapper>
        <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="en-gb">
          {children}
        </LocalizationProvider>
      </ThemeWrapper>
    </QueryClientProvider>
  )
}
