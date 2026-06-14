export const PatientQueryKeys = {
  all: ['patients'] as const,
  lists: () => [...PatientQueryKeys.all, 'list'] as const,
  detail: (id: string) => [...PatientQueryKeys.all, 'detail', id] as const,
}

export const AppointmentQueryKeys = {
  all: ['appointments'] as const,
  lists: () => [...AppointmentQueryKeys.all, 'list'] as const,
  detail: (id: string) => [...AppointmentQueryKeys.all, 'detail', id] as const,
}

export const StaffQueryKeys = {
  all: ['staff'] as const,
  lists: () => [...StaffQueryKeys.all, 'list'] as const,
  detail: (id: string) => [...StaffQueryKeys.all, 'detail', id] as const,
}
