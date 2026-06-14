import { lazy, Suspense } from 'react'
import { createBrowserRouter, Navigate } from 'react-router-dom'
import type { ReactNode } from 'react'
import { Routes } from '@/constants/routes'
import { WWSkeleton } from '@/components'
import { Layout } from './Layout'

const DashboardPage = lazy(() =>
  import('@/pages/DashboardPage').then((m) => ({ default: m.DashboardPage })),
)
const PatientsPage = lazy(() =>
  import('@/pages/PatientsPage').then((m) => ({ default: m.PatientsPage })),
)
const PatientDetailPage = lazy(() =>
  import('@/pages/PatientDetailPage').then((m) => ({ default: m.PatientDetailPage })),
)
const AppointmentsPage = lazy(() =>
  import('@/pages/AppointmentsPage').then((m) => ({ default: m.AppointmentsPage })),
)
const StaffPage = lazy(() =>
  import('@/pages/StaffPage').then((m) => ({ default: m.StaffPage })),
)
const LoginPage = lazy(() =>
  import('@/pages/LoginPage').then((m) => ({ default: m.LoginPage })),
)

function ProtectedRoute({ children }: { children: ReactNode }) {
  return <>{children}</>
}

function PageShell({ children }: { children: ReactNode }) {
  return (
    <Suspense fallback={<WWSkeleton variant="rectangular" height={400} />}>
      {children}
    </Suspense>
  )
}

export const router = createBrowserRouter([
  {
    path: '/',
    element: (
      <ProtectedRoute>
        <Layout />
      </ProtectedRoute>
    ),
    children: [
      { index: true, element: <Navigate to={Routes.dashboard} replace /> },
      {
        path: 'dashboard',
        element: <PageShell><DashboardPage /></PageShell>,
      },
      {
        path: 'patients',
        element: <PageShell><PatientsPage /></PageShell>,
      },
      {
        path: 'patients/:patientId',
        element: <PageShell><PatientDetailPage /></PageShell>,
      },
      {
        path: 'appointments',
        element: <PageShell><AppointmentsPage /></PageShell>,
      },
      {
        path: 'staff',
        element: <PageShell><StaffPage /></PageShell>,
      },
    ],
  },
  {
    path: Routes.login,
    element: <PageShell><LoginPage /></PageShell>,
  },
])
