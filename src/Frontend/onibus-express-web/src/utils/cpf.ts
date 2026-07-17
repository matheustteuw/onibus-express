export function onlyDigits(value: string): string {
  return value.replace(/\D/g, '')
}

export function isValidCpf(value: string): boolean {
  const cpf = onlyDigits(value)

  if (cpf.length !== 11) return false
  if (new Set(cpf).size === 1) return false

  const calculateCheckDigit = (length: number): number => {
    let sum = 0
    let weight = length + 1

    for (let i = 0; i < length; i++) {
      sum += Number(cpf[i]) * weight
      weight--
    }

    const remainder = sum % 11
    return remainder < 2 ? 0 : 11 - remainder
  }

  const firstCheckDigit = calculateCheckDigit(9)
  if (firstCheckDigit !== Number(cpf[9])) return false

  const secondCheckDigit = calculateCheckDigit(10)
  if (secondCheckDigit !== Number(cpf[10])) return false

  return true
}
