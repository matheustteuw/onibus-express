import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { tripsApi } from '../services/api'
import { ApiError } from '../services/apiClient'
import { useBookingStore } from '../store/bookingStore'
import { TripCard } from '../components/TripCard'
import type { TripSummary } from '../types/api'
import styles from './SearchPage.module.css'

export function SearchPage() {
  const navigate = useNavigate()
  const setSelectedTrip = useBookingStore((state) => state.setSelectedTrip)

  const [origin, setOrigin] = useState('')
  const [destination, setDestination] = useState('')
  const [departureDate, setDepartureDate] = useState('')

  const [trips, setTrips] = useState<TripSummary[] | null>(null)
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  async function handleSearch(event: React.FormEvent) {
    event.preventDefault()

    setIsLoading(true)
    setError(null)
    setTrips(null)

    try {
      const result = await tripsApi.search({ origin, destination, departureDate })
      setTrips(result.trips)
    } catch (err) {
      setError(err instanceof ApiError ? err.messages.join(', ') : 'Erro ao buscar viagens.')
    } finally {
      setIsLoading(false)
    }
  }

  async function handleSelectTrip(trip: TripSummary) {
    const details = await tripsApi.getById(trip.id)
    setSelectedTrip(details)
    navigate(`/viagens/${trip.id}/assentos`)
  }

  return (
    <section>
      <h1>Buscar passagens</h1>

      <form onSubmit={handleSearch} className={styles.form}>
        <label>
          Origem
          <input
            value={origin}
            onChange={(e) => setOrigin(e.target.value)}
            placeholder="São Paulo"
            required
          />
        </label>

        <label>
          Destino
          <input
            value={destination}
            onChange={(e) => setDestination(e.target.value)}
            placeholder="Rio de Janeiro"
            required
          />
        </label>

        <label>
          Data de ida
          <input
            type="date"
            value={departureDate}
            onChange={(e) => setDepartureDate(e.target.value)}
            required
          />
        </label>

        <button type="submit" disabled={isLoading}>
          {isLoading ? 'Buscando...' : 'Buscar'}
        </button>
      </form>

      {isLoading && <p role="status">Buscando viagens...</p>}

      {error && <p role="alert">{error}</p>}

      {trips !== null && !isLoading && trips.length === 0 && (
        <p>Nenhuma viagem encontrada para os critérios informados.</p>
      )}

      {trips !== null && trips.length > 0 && (
        <div className={styles.results}>
          {trips.map((trip) => (
            <TripCard key={trip.id} trip={trip} onSelect={handleSelectTrip} />
          ))}
        </div>
      )}
    </section>
  )
}
