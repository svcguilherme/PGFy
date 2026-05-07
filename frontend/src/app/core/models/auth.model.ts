export interface LoginDto {
  email: string;
  password: string;
}

export interface CurrentUser {
  id: string;
  nome: string;
  email: string;
}

export interface RegisterDto {
  nomeCompleto: string;
  email: string;
  password: string;
}

export interface AuthResponseDto {
  accessToken: string;
  refreshToken: string;
}
