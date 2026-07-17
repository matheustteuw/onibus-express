import { formatCurrency, formatDateTime } from '../utils/format'
import type { TripSummary } from '../types/api'
import styles from './TripCard.module.css'

interface TripCardProps {
  trip: TripSummary
  onSelect: (trip: TripSummary) => void
}

export function TripCard({ trip, onSelect }: TripCardProps) {
  return (
    <article className={styles.card}>
      <div>
        <p className={styles.route}>
          {trip.origin} &rarr; {trip.destination}
        </p>
        <p className={styles.departure}>{formatDateTime(trip.departureTime)}</p>
      </div>

      <div className={styles.details}>
        <span className={styles.price}>{formatCurrency(trip.basePrice)}</span>
        <span className={styles.seats}>{trip.availableSeats} vagas restantes</span>
      </div>

      <button
        type="button"
        onClick={() => onSelect(trip)}
        disabled={trip.availableSeats === 0}
      >
        {trip.availableSeats === 0 ? 'Esgotado' : 'Selecionar'}
      </button>
    </article>
  )
}
