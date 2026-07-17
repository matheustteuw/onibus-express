import { useState } from 'react'
import { Navigate, useNavigate } from 'react-router-dom'
import { useBookingStore } from '../store/bookingStore'
import { PassengerForm, type PassengerFormData } from '../components/PassengerForm'
import { reservationsApi } from '../services/api'
import { ApiError } from '../services/apiClient'
import { formatCurrency, formatDateTime } from '../utils/format'
import styles from './PassengerPage.module.css'

export function PassengerPage() {
  const navigate = useNavigate()
  const selectedTrip = useBookingStore((state) => state.selectedTrip)
  const selectedSeat = useBookingStore((state) => state.selectedSeat)
  const setReservationCode = useBookingStore((state) => state.setReservationCode)

  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)

  if (!selectedTrip || selectedSeat === null) {
    return <Navigate to="/" replace />
  }

  async function handleSubmit(data: PassengerFormData) {
    setIsSubmitting(true)
    setError(null)

    try {
      const result = await reservationsApi.register({
        tripId: selectedTrip!.id,
        seatNumber: selectedSeat!,
        passengerName: data.name,
        cpf: data.cpf,
        email: data.email,
      })

      setReservationCode(result.reservationCode)
      navigate('/confirmacao')
    } catch (err) {
      setError(err instanceof ApiError ? err.messages.join(', ') : 'Erro ao criar a reserva.')
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <section>
      <h1>Seus dados</h1>

      <div className={styles.summary}>
        <h2>Resumo da compra</h2>
        <p>
          {selectedTrip.origin} &rarr; {selectedTrip.destination}
        </p>
        <p>{formatDateTime(selectedTrip.departureTime)}</p>
        <p>Assento {selectedSeat}</p>
        <p className={styles.price}>{formatCurrency(selectedTrip.basePrice)}</p>
      </div>

      <PassengerForm onSubmit={handleSubmit} isSubmitting={isSubmitting} />

      {error && <p role="alert">{error}</p>}
    </section>
  )
}
