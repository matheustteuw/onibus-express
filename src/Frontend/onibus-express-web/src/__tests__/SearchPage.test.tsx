import { fireEvent, render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { MemoryRouter } from 'react-router-dom'
import { describe, expect, it, vi } from 'vitest'
import { SearchPage } from '../pages/SearchPage'
import { tripsApi } from '../services/api'
import type { TripSummary } from '../types/api'

vi.mock('../services/api', () => ({
  tripsApi: {
    search: vi.fn(),
    getById: vi.fn(),
  },
}))

const mockedTrips: TripSummary[] = [
  {
    id: 'trip-1',
    origin: 'São Paulo',
    destination: 'Rio de Janeiro',
    departureTime: '2026-08-01T08:00:00',
    basePrice: 120,
    availableSeats: 10,
  },
]

async function fillAndSubmitSearch() {
  const user = userEvent.setup()

  render(
    <MemoryRouter>
      <SearchPage />
    </MemoryRouter>,
  )

  await user.type(screen.getByLabelText('Origem'), 'São Paulo')
  await user.type(screen.getByLabelText('Destino'), 'Rio de Janeiro')
  fireEvent.change(screen.getByLabelText('Data de ida'), { target: { value: '2026-08-01' } })
  await user.click(screen.getByRole('button', { name: 'Buscar' }))
}

describe('SearchPage', () => {
  it('busca viagens ao preencher o formulário e clicar em Buscar', async () => {
    vi.mocked(tripsApi.search).mockResolvedValueOnce({ trips: mockedTrips })

    await fillAndSubmitSearch()

    expect(await screen.findByText('São Paulo → Rio de Janeiro')).toBeInTheDocument()
    expect(tripsApi.search).toHaveBeenCalledWith({
      origin: 'São Paulo',
      destination: 'Rio de Janeiro',
      departureDate: '2026-08-01',
    })
  })

  it('mostra mensagem quando a busca não retorna viagens', async () => {
    vi.mocked(tripsApi.search).mockResolvedValueOnce({ trips: [] })

    await fillAndSubmitSearch()

    expect(
      await screen.findByText('Nenhuma viagem encontrada para os critérios informados.'),
    ).toBeInTheDocument()
  })
})
