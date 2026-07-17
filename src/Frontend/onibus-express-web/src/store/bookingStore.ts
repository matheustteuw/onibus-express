import { create } from 'zustand'
import type { TripDetails } from '../types/api'

interface BookingState {
  selectedTrip: TripDetails | null
  selectedSeat: number | null
  reservationCode: string | null
  setSelectedTrip: (trip: TripDetails) => void
  setSelectedSeat: (seat: number) => void
  setReservationCode: (code: string) => void
  reset: () => void
}

export const useBookingStore = create<BookingState>((set) => ({
  selectedTrip: null,
  selectedSeat: null,
  reservationCode: null,
  setSelectedTrip: (trip) => set({ selectedTrip: trip, selectedSeat: null }),
  setSelectedSeat: (seat) => set({ selectedSeat: seat }),
  setReservationCode: (code) => set({ reservationCode: code }),
  reset: () => set({ selectedTrip: null, selectedSeat: null, reservationCode: null }),
}))
