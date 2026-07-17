import { useState } from 'react'
import { isValidCpf } from '../utils/cpf'
import styles from './PassengerForm.module.css'

export interface PassengerFormData {
  name: string
  cpf: string
  email: string
}

interface PassengerFormProps {
  onSubmit: (data: PassengerFormData) => void
  isSubmitting?: boolean
}

type FormErrors = Partial<Record<keyof PassengerFormData, string>>

export function PassengerForm({ onSubmit, isSubmitting }: PassengerFormProps) {
  const [name, setName] = useState('')
  const [cpf, setCpf] = useState('')
  const [email, setEmail] = useState('')
  const [errors, setErrors] = useState<FormErrors>({})

  function validate(): FormErrors {
    const nextErrors: FormErrors = {}

    if (!name.trim()) {
      nextErrors.name = 'Informe o nome completo.'
    }

    if (!cpf.trim()) {
      nextErrors.cpf = 'Informe o CPF.'
    } else if (!isValidCpf(cpf)) {
      nextErrors.cpf = 'CPF inválido.'
    }

    if (!email.trim()) {
      nextErrors.email = 'Informe o e-mail.'
    } else if (!/^\S+@\S+\.\S+$/.test(email)) {
      nextErrors.email = 'E-mail inválido.'
    }

    return nextErrors
  }

  function handleSubmit(event: React.FormEvent) {
    event.preventDefault()

    const nextErrors = validate()
    setErrors(nextErrors)

    if (Object.keys(nextErrors).length === 0) {
      onSubmit({ name, cpf, email })
    }
  }

  return (
    <form onSubmit={handleSubmit} className={styles.form} noValidate>
      <label>
        Nome completo
        <input value={name} onChange={(e) => setName(e.target.value)} />
        {errors.name && <span role="alert">{errors.name}</span>}
      </label>

      <label>
        CPF
        <input value={cpf} onChange={(e) => setCpf(e.target.value)} placeholder="000.000.000-00" />
        {errors.cpf && <span role="alert">{errors.cpf}</span>}
      </label>

      <label>
        E-mail
        <input value={email} onChange={(e) => setEmail(e.target.value)} />
        {errors.email && <span role="alert">{errors.email}</span>}
      </label>

      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Confirmando...' : 'Confirmar reserva'}
      </button>
    </form>
  )
}
