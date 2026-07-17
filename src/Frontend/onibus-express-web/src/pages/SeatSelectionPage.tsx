import { Navigate, useNavigate } from 'react-router-dom'
import { useBookingStore } from '../store/bookingStore'
import { SeatMap } from '../components/SeatMap'
import { formatCurrency, formatDateTime } from '../utils/format'
import styles from './SeatSelectionPage.module.css'

export function SeatSelectionPage() {
  const navigate = useNavigate()
  const selectedTrip = useBookingStore((state) => state.selectedTrip)
  const selectedSeat = useBookingStore((state) => state.selectedSeat)
  const setSelectedSeat = useBookingStore((state) => state.setSelectedSeat)

  if (!selectedTrip) {
    return <Navigate to="/" replace />
  }

  return (
    <section>
      <h1>Selecione seu assento</h1>

      <div className={styles.tripInfo}>
        <p>
          {selectedTrip.origin} &rarr; {selectedTrip.destination}
        </p>
        <p>{formatDateTime(selectedTrip.departureTime)}</p>
        <p>{formatCurrency(selectedTrip.basePrice)}</p>
      </div>

      <SeatMap
        totalSeats={selectedTrip.totalSeats}
        occupiedSeats={selectedTrip.occupiedSeats}
        selectedSeat={selectedSeat}
        onSelectSeat={setSelectedSeat}
      />

      <button
        type="button"
        disabled={selectedSeat === null}
        onClick={() => navigate(`/viagens/${selectedTrip.id}/passageiro`)}
      >
        Prosseguir
      </button>
    </section>
  )
}
