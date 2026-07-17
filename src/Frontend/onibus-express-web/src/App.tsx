import { Link, Route, Routes } from 'react-router-dom'
import { SearchPage } from './pages/SearchPage'
import { SeatSelectionPage } from './pages/SeatSelectionPage'
import { PassengerPage } from './pages/PassengerPage'
import { ConfirmationPage } from './pages/ConfirmationPage'
import { ReservationLookupPage } from './pages/ReservationLookupPage'
import styles from './App.module.css'

function App() {
  return (
    <div className={styles.app}>
      <header className={styles.header}>
        <Link to="/" className={styles.brand}>
          OniBus Express
        </Link>
        <nav>
          <Link to="/reservas/consulta">Consultar reserva</Link>
        </nav>
      </header>

      <main className={styles.main}>
        <Routes>
          <Route path="/" element={<SearchPage />} />
          <Route path="/viagens/:tripId/assentos" element={<SeatSelectionPage />} />
          <Route path="/viagens/:tripId/passageiro" element={<PassengerPage />} />
          <Route path="/confirmacao" element={<ConfirmationPage />} />
          <Route path="/reservas/consulta" element={<ReservationLookupPage />} />
        </Routes>
      </main>
    </div>
  )
}

export default App
