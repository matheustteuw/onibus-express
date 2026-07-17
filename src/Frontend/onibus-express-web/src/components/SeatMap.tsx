import styles from './SeatMap.module.css'

interface SeatMapProps {
  totalSeats: number
  occupiedSeats: number[]
  selectedSeat: number | null
  onSelectSeat: (seat: number) => void
}

export function SeatMap({ totalSeats, occupiedSeats, selectedSeat, onSelectSeat }: SeatMapProps) {
  const seats = Array.from({ length: totalSeats }, (_, index) => index + 1)

  return (
    <div className={styles.grid}>
      {seats.map((seat) => {
        const isOccupied = occupiedSeats.includes(seat)
        const isSelected = selectedSeat === seat

        const status = isOccupied ? 'ocupado' : isSelected ? 'selecionado' : 'livre'

        return (
          <button
            key={seat}
            type="button"
            disabled={isOccupied}
            onClick={() => onSelectSeat(seat)}
            aria-label={`Assento ${seat}, ${status}`}
            aria-pressed={isSelected}
            className={[
              styles.seat,
              isOccupied && styles.occupied,
              isSelected && styles.selected,
            ]
              .filter(Boolean)
              .join(' ')}
          >
            {seat}
          </button>
        )
      })}
    </div>
  )
}
