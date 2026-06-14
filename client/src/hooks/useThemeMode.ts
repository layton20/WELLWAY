import { createContext, useContext, useState, useCallback } from 'react'

type ThemeMode = 'light' | 'dark'

interface ThemeModeContextValue {
  mode: ThemeMode
  toggleThemeMode: () => void
}

export const ThemeModeContext = createContext<ThemeModeContextValue>({
  mode: 'light',
  toggleThemeMode: () => undefined,
})

export function useThemeMode(): ThemeModeContextValue {
  return useContext(ThemeModeContext)
}

export function useThemeModeState(): ThemeModeContextValue {
  const [mode, setMode] = useState<ThemeMode>(
    () => (localStorage.getItem('wellway-theme-mode') as ThemeMode | null) ?? 'light',
  )

  const toggleThemeMode = useCallback(() => {
    setMode((prev) => {
      const next: ThemeMode = prev === 'dark' ? 'light' : 'dark'
      localStorage.setItem('wellway-theme-mode', next)
      return next
    })
  }, [])

  return { mode, toggleThemeMode }
}
