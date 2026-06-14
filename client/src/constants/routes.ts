export const Routes = {
  login: '/login',
  dashboard: '/dashboard',
  patients: {
    list: '/patients',
    detail: (patientId: string) => `/patients/${patientId}`,
  },
  appointments: {
    list: '/appointments',
  },
  staff: {
    list: '/staff',
  },
}
