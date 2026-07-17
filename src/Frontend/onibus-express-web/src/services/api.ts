import { apiClient } from './apiClient'
import type {
  RegisteredReservation,
  RegisterReservationRequest,
  ReservationDetails,
  RouteSummary,
  TripDetails,
  TripSummary,
} from '../types/api'

export interface SearchTripsParams {
  origin: string
  destination: string
  departureDate: string
}

export const routesApi = {
  getAll: () => apiClient.get<{ routes: RouteSummary[] }>('/rotas'),
}

export const tripsApi = {
  search: ({ origin, destination, departureDate }: SearchTripsParams) => {
    const query = new URLSearchParams({ origin, destination, departureDate })
    return apiClient.get<{ trips: TripSummary[] }>(`/viagens?${query.toString()}`)
  },
  getById: (tripId: string) => apiClient.get<TripDetails>(`/viagens/${tripId}`),
}

export const reservationsApi = {
  register: (payload: RegisterReservationRequest) =>
    apiClient.post<RegisteredReservation>('/reservas', payload),
  getByCode: (code: string) => apiClient.get<ReservationDetails>(`/reservas/${code}`),
  cancel: (code: string) => apiClient.delete(`/reservas/${code}`),
}
