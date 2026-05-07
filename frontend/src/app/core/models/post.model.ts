export interface PostDto {
  id: string;
  titulo: string;
  conteudo: string;
  dataPublicacao: string;
  usuarioId: string;
}

export interface CreatePostDto {
  titulo: string;
  conteudo: string;
}

export interface UpdatePostDto {
  titulo: string;
  conteudo: string;
}
