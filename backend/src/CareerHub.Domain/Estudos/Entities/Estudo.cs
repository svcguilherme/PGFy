using CareerHub.SharedKernel;

namespace CareerHub.Domain.Estudos.Entities;

public class Estudo : AuditableEntity
{
    public string Titulo { get; private set; } = string.Empty;
    public DiaDaSemana DiaDaSemana { get; private set; }
    public TimeOnly HoraInicio { get; private set; }
    public TimeOnly HoraFim { get; private set; }
    public double HorasTotais => (HoraFim - HoraInicio).TotalHours;
    public string? Descricao { get; private set; }
    public Guid UsuarioId { get; private set; }

    private Estudo() { }

    public static Result<Estudo> Create(
        string titulo, DiaDaSemana dia,
        TimeOnly inicio, TimeOnly fim,
        Guid usuarioId, string? descricao = null)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Failure<Estudo>("Título é obrigatório.");
        if (fim <= inicio)
            return Result.Failure<Estudo>("Hora de fim deve ser após hora de início.");

        var estudo = new Estudo
        {
            Titulo = titulo.Trim(),
            DiaDaSemana = dia,
            HoraInicio = inicio,
            HoraFim = fim,
            UsuarioId = usuarioId,
            Descricao = descricao
        };
        estudo.SetCreatedBy(usuarioId);
        estudo.AddDomainEvent(new EstudoCriadoEvent(estudo.Id));
        return Result.Success(estudo);
    }

    public Result<bool> Atualizar(
        string titulo, DiaDaSemana dia,
        TimeOnly inicio, TimeOnly fim,
        string? descricao)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Failure<bool>("Título é obrigatório.");
        if (fim <= inicio)
            return Result.Failure<bool>("Hora de fim deve ser após hora de início.");

        Titulo = titulo.Trim();
        DiaDaSemana = dia;
        HoraInicio = inicio;
        HoraFim = fim;
        Descricao = descricao;
        SetUpdated();
        return Result.Success(true);
    }
}
