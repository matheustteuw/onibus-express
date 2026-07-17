import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import { PassengerForm } from '../components/PassengerForm'

describe('PassengerForm', () => {
  it('mostra erros de validação ao submeter o formulário vazio', async () => {
    const handleSubmit = vi.fn()
    const user = userEvent.setup()

    render(<PassengerForm onSubmit={handleSubmit} />)

    await user.click(screen.getByRole('button', { name: 'Confirmar reserva' }))

    expect(await screen.findByText('Informe o nome completo.')).toBeInTheDocument()
    expect(screen.getByText('Informe o CPF.')).toBeInTheDocument()
    expect(screen.getByText('Informe o e-mail.')).toBeInTheDocument()
    expect(handleSubmit).not.toHaveBeenCalled()
  })

  it('mostra erro quando o CPF é inválido', async () => {
    const handleSubmit = vi.fn()
    const user = userEvent.setup()

    render(<PassengerForm onSubmit={handleSubmit} />)

    await user.type(screen.getByLabelText('Nome completo'), 'Maria Souza')
    await user.type(screen.getByLabelText('CPF'), '111.111.111-11')
    await user.type(screen.getByLabelText('E-mail'), 'maria@example.com')
    await user.click(screen.getByRole('button', { name: 'Confirmar reserva' }))

    expect(await screen.findByText('CPF inválido.')).toBeInTheDocument()
    expect(handleSubmit).not.toHaveBeenCalled()
  })

  it('chama onSubmit com os dados quando o formulário é válido', async () => {
    const handleSubmit = vi.fn()
    const user = userEvent.setup()

    render(<PassengerForm onSubmit={handleSubmit} />)

    await user.type(screen.getByLabelText('Nome completo'), 'Maria Souza')
    await user.type(screen.getByLabelText('CPF'), '111.444.777-35')
    await user.type(screen.getByLabelText('E-mail'), 'maria@example.com')
    await user.click(screen.getByRole('button', { name: 'Confirmar reserva' }))

    expect(handleSubmit).toHaveBeenCalledWith({
      name: 'Maria Souza',
      cpf: '111.444.777-35',
      email: 'maria@example.com',
    })
  })
})
