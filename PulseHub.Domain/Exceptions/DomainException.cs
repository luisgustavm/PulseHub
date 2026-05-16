// PulseHub.Domain/Exceptions/DomainException.cs

namespace PulseHub.Domain.Exceptions;

/// <summary>
/// Exceção base para violações de regras de negócio do domínio.
///
/// Quando usar:
///   Lance DomainException sempre que uma regra de negócio for violada
///   dentro de uma entidade ou serviço de domínio.
///
/// Quando NÃO usar:
///   Não use para erros de infraestrutura (banco, rede, arquivo).
///   Não use para erros de validação de entrada da API (use FluentValidation).
///   DomainException representa violação de REGRA DE NEGÓCIO — não de formato.
///
/// Exemplos corretos:
///   throw new DomainException("Usuário já está inativo.");
///   throw new DomainException("Estoque insuficiente.");
///   throw new DomainException("Pedido não pode ser cancelado após entrega.");
///
/// Exemplos incorretos (não são regras de negócio):
///   throw new DomainException("Email não pode ser nulo.");   ← use ArgumentException
///   throw new DomainException("Conexão com banco falhou.");  ← use IOException
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message)
        : base(message)
    {
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}