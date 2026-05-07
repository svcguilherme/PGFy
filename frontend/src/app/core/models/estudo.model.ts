export type DiaDaSemana =
  'Segunda' | 'Terca' | 'Quarta' | 'Quinta' | 'Sexta' | 'Sabado' | 'Domingo';

export const DIAS_DA_SEMANA: DiaDaSemana[] = [
  'Segunda', 'Terca', 'Quarta', 'Quinta', 'Sexta', 'Sabado', 'Domingo'
];

export interface EstudoDto {
  id: string;
  titulo: string;
  diaDaSemana: DiaDaSemana;
  horaInicio: string;
  horaFim: string;
  horasTotais: number;
  descricao?: string;
  usuarioId: string;
}

export interface CreateEstudoDto {
  titulo: string;
  diaDaSemana: DiaDaSemana;
  horaInicio: string;
  horaFim: string;
  descricao?: string;
}

export interface UpdateEstudoDto {
  titulo: string;
  diaDaSemana: DiaDaSemana;
  horaInicio: string;
  horaFim: string;
  descricao?: string;
}
