using CareerHub.SharedKernel;

namespace CareerHub.Domain.Financeiro.Entities;

public class Despesa : AuditableEntity
{
    public string Descricao { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
    public DateTime DataPrevista { get; private set; }
    public bool Pago { get; private set; }
    public CategoriaDespesa Categoria { get; private set; }
    public Guid UsuarioId { get; private set; }

    private Despesa() { }

    public static Result<Despesa> Create(
        string descricao, decimal valor,
        DateTime dataPrevista, CategoriaDespesa categoria,
        Guid usuarioId)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure<Despesa>("Descrição é obrigatória.");
        if (valor <= 0)
            return Result.Failure<Despesa>("Valor deve ser positivo.");

        var despesa = new Despesa
        {
            Descricao = descricao.Trim(),
            Valor = valor,
            DataPrevista = dataPrevista,
            Categoria = categoria,
            UsuarioId = usuarioId
        };
        despesa.SetCreatedBy(usuarioId);
        despesa.AddDomainEvent(new DespesaCriadaEvent(despesa.Id));
        return Result.Success(despesa);
    }

    public void MarcarComoPago()
    {
        if (!Pago)
        {
            Pago = true;
            SetUpdated();
        }
    }

    public Result<bool> Atualizar(string descricao, decimal valor, DateTime dataPrevista, CategoriaDespesa categoria)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure<bool>("Descrição é obrigatória.");
        if (valor <= 0)
            return Result.Failure<bool>("Valor deve ser positivo.");

        Descricao = descricao.Trim();
        Valor = valor;
        DataPrevista = dataPrevista;
        Categoria = categoria;
        SetUpdated();
        return Result.Success(true);
    }
}
