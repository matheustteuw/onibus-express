import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import { SeatMap } from '../components/SeatMap'

describe('SeatMap', () => {
  it('permite selecionar um assento livre', async () => {
    const handleSelectSeat = vi.fn()
    const user = userEvent.setup()

    render(
      <SeatMap totalSeats={10} occupiedSeats={[3]} selectedSeat={null} onSelectSeat={handleSelectSeat} />,
    )

    await user.click(screen.getByRole('button', { name: 'Assento 5, livre' }))

    expect(handleSelectSeat).toHaveBeenCalledWith(5)
  })

  it('bloqueia a seleção de um assento já ocupado', async () => {
    const handleSelectSeat = vi.fn()
    const user = userEvent.setup()

    render(
      <SeatMap totalSeats={10} occupiedSeats={[3]} selectedSeat={null} onSelectSeat={handleSelectSeat} />,
    )

    const occupiedSeat = screen.getByRole('button', { name: 'Assento 3, ocupado' })
    expect(occupiedSeat).toBeDisabled()

    await user.click(occupiedSeat)

    expect(handleSelectSeat).not.toHaveBeenCalled()
  })

  it('exibe o assento selecionado como marcado', () => {
    render(<SeatMap totalSeats={10} occupiedSeats={[]} selectedSeat={5} onSelectSeat={vi.fn()} />)

    expect(screen.getByRole('button', { name: 'Assento 5, selecionado' })).toHaveAttribute(
      'aria-pressed',
      'true',
    )
  })
})
