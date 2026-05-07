using CareerHub.SharedKernel;

namespace CareerHub.Domain.Financeiro.Entities;

public class Recebivel : AuditableEntity
{
    public string Descricao { get; private set; } = string.Empty;
    public decimal ValorPrevisto { get; private set; }
    public DateTime DataPrevista { get; private set; }
    public bool Recebido { get; private set; }
    public Guid UsuarioId { get; private set; }

    private Recebivel() { }

    public static Result<Recebivel> Create(
        string descricao, decimal valorPrevisto,
        DateTime dataPrevista, Guid usuarioId)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure<Recebivel>("Descrição é obrigatória.");
        if (valorPrevisto <= 0)
            return Result.Failure<Recebivel>("Valor previsto deve ser positivo.");

        var recebivel = new Recebivel
        {
            Descricao = descricao.Trim(),
            ValorPrevisto = valorPrevisto,
            DataPrevista = dataPrevista,
            UsuarioId = usuarioId
        };
        recebivel.SetCreatedBy(usuarioId);
        recebivel.AddDomainEvent(new RecebivelCriadoEvent(recebivel.Id));
        return Result.Success(recebivel);
    }

    public void MarcarComoRecebido()
    {
        if (!Recebido)
        {
            Recebido = true;
            SetUpdated();
        }
    }

    public Result<bool> Atualizar(string descricao, decimal valorPrevisto, DateTime dataPrevista)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure<bool>("Descrição é obrigatória.");
        if (valorPrevisto <= 0)
            return Result.Failure<bool>("Valor previsto deve ser positivo.");

        Descricao = descricao.Trim();
        ValorPrevisto = valorPrevisto;
        DataPrevista = dataPrevista;
        SetUpdated();
        return Result.Success(true);
    }
}
