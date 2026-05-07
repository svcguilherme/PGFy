export type CategoriaDespesa =
  'Alimentacao' | 'Transporte' | 'Educacao' | 'Saude' | 'Lazer' | 'Outros';

export const CATEGORIAS_DESPESA: CategoriaDespesa[] = [
  'Alimentacao', 'Transporte', 'Educacao', 'Saude', 'Lazer', 'Outros'
];

export interface DespesaDto {
  id: string;
  descricao: string;
  valor: number;
  dataPrevista: string;
  pago: boolean;
  categoria: CategoriaDespesa;
  usuarioId: string;
}

export interface CreateDespesaDto {
  descricao: string;
  valor: number;
  dataPrevista: string;
  categoria: CategoriaDespesa;
}

export interface UpdateDespesaDto {
  descricao: string;
  valor: number;
  dataPrevista: string;
  categoria: CategoriaDespesa;
}

export interface RecebivelDto {
  id: string;
  descricao: string;
  valorPrevisto: number;
  dataPrevista: string;
  recebido: boolean;
  usuarioId: string;
}

export interface CreateRecebivelDto {
  descricao: string;
  valorPrevisto: number;
  dataPrevista: string;
}

export interface UpdateRecebivelDto {
  descricao: string;
  valorPrevisto: number;
  dataPrevista: string;
}

export interface FluxoMensalDto {
  ano: number;
  mes: number;
  totalRecebiveis: number;
  totalDespesas: number;
  saldo: number;
  positivo: boolean;
}
