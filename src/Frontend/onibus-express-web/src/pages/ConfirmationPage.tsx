import { Link, Navigate } from 'react-router-dom'
import { useBookingStore } from '../store/bookingStore'

export function ConfirmationPage() {
  const reservationCode = useBookingStore((state) => state.reservationCode)
  const reset = useBookingStore((state) => state.reset)

  if (!reservationCode) {
    return <Navigate to="/" replace />
  }

  return (
    <section>
      <h1>Reserva confirmada!</h1>
      <p>Guarde o código abaixo para consultar ou cancelar sua reserva depois.</p>
      <p>
        <strong>{reservationCode}</strong>
      </p>
      <Link to="/" onClick={reset}>
        Voltar para a busca
      </Link>
    </section>
  )
}
