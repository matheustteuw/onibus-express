export interface RouteSummary {
  id: string
  origin: string
  destination: string
  estimatedDuration: string
}

export interface TripSummary {
  id: string
  origin: string
  destination: string
  departureTime: string
  basePrice: number
  availableSeats: number
}

export interface TripDetails {
  id: string
  origin: string
  destination: string
  departureTime: string
  basePrice: number
  totalSeats: number
  availableSeats: number
  occupiedSeats: number[]
}

export interface RegisterReservationRequest {
  tripId: string
  seatNumber: number
  passengerName: string
  cpf: string
  email: string
}

export interface RegisteredReservation {
  reservationCode: string
  seatNumber: number
}

export interface ReservationDetails {
  reservationCode: string
  status: string
  seatNumber: number
  passengerName: string
  origin: string
  destination: string
  departureTime: string
  basePrice: number
}

export interface ApiErrorResponse {
  errors: string[]
}
