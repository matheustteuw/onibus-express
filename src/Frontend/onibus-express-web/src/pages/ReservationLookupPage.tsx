import { useState } from 'react'
import { reservationsApi } from '../services/api'
import { ApiError } from '../services/apiClient'
import { formatCurrency, formatDateTime } from '../utils/format'
import type { ReservationDetails } from '../types/api'
import styles from './ReservationLookupPage.module.css'

export function ReservationLookupPage() {
  const [code, setCode] = useState('')
  const [reservation, setReservation] = useState<ReservationDetails | null>(null)
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [isCancelling, setIsCancelling] = useState(false)

  async function handleSearch(event: React.FormEvent) {
    event.preventDefault()

    setIsLoading(true)
    setError(null)
    setReservation(null)

    try {
      const result = await reservationsApi.getByCode(code)
      setReservation(result)
    } catch (err) {
      setError(err instanceof ApiError ? err.messages.join(', ') : 'Reserva não encontrada.')
    } finally {
      setIsLoading(false)
    }
  }

  async function handleCancel() {
    if (!reservation) return

    setIsCancelling(true)
    setError(null)

    try {
      await reservationsApi.cancel(reservation.reservationCode)
      setReservation({ ...reservation, status: 'Cancelled' })
    } catch (err) {
      setError(err instanceof ApiError ? err.messages.join(', ') : 'Erro ao cancelar a reserva.')
    } finally {
      setIsCancelling(false)
    }
  }

  return (
    <section>
      <h1>Consultar reserva</h1>

      <form onSubmit={handleSearch} className={styles.form}>
        <label>
          Código da reserva
          <input
            value={code}
            onChange={(e) => setCode(e.target.value)}
            placeholder="ABC-12345"
            required
          />
        </label>

        <button type="submit" disabled={isLoading}>
          {isLoading ? 'Buscando...' : 'Consultar'}
        </button>
      </form>

      {error && <p role="alert">{error}</p>}

      {reservation && (
        <div className={styles.details}>
          <p>
            {reservation.origin} &rarr; {reservation.destination}
          </p>
          <p>{formatDateTime(reservation.departureTime)}</p>
          <p>Assento {reservation.seatNumber}</p>
          <p>{reservation.passengerName}</p>
          <p>{formatCurrency(reservation.basePrice)}</p>
          <p>
            Status: <strong>{reservation.status}</strong>
          </p>

          {reservation.status === 'Active' && (
            <button type="button" onClick={handleCancel} disabled={isCancelling}>
              {isCancelling ? 'Cancelando...' : 'Cancelar reserva'}
            </button>
          )}
        </div>
      )}
    </section>
  )
}
