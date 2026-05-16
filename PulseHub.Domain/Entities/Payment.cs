// PulseHub.Domain/Entities/Payment.cs

using PulseHub.Domain.Exceptions;

namespace PulseHub.Domain.Entities;

public enum PaymentMethod
{
    Pix = 1,
    CreditCard = 2,
    DebitCard = 3,
    BankSlip = 4   // Boleto
}

public enum PaymentStatus
{
    Pending = 1,   // Aguardando pagamento
    Processing = 2,   // Processando junto à operadora
    Approved = 3,   // Aprovado
    Refused = 4,   // Recusado pela operadora
    Cancelled = 5,   // Cancelado antes de processar
    Refunded = 6    // Estornado após aprovação
}

public class Payment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Order? Order { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? TransactionCode { get; private set; } // código retornado pela operadora
    public string? RefusalReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? RefundedAt { get; private set; }

    private Payment() { }

    /// <summary>
    /// Cria um pagamento pendente vinculado a um pedido.
    /// O pagamento começa sempre como Pending — nunca como Approved.
    /// </summary>
    public static Payment Create(Guid orderId, decimal amount, PaymentMethod method)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("OrderId inválido.");

        if (amount <= 0)
            throw new DomainException("Valor do pagamento deve ser maior que zero.");

        return new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Amount = amount,
            Method = method,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Marca o pagamento como em processamento junto à operadora.
    /// </summary>
    public void StartProcessing()
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException("Apenas pagamentos pendentes podem ser processados.");

        Status = PaymentStatus.Processing;
    }

    /// <summary>
    /// Confirma aprovação do pagamento pela operadora.
    /// </summary>
    public void Approve(string transactionCode)
    {
        if (Status != PaymentStatus.Processing)
            throw new DomainException("Apenas pagamentos em processamento podem ser aprovados.");

        ArgumentException.ThrowIfNullOrWhiteSpace(transactionCode, nameof(transactionCode));

        Status = PaymentStatus.Approved;
        TransactionCode = transactionCode;
        PaidAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Registra recusa do pagamento pela operadora.
    /// </summary>
    public void Refuse(string reason)
    {
        if (Status != PaymentStatus.Processing)
            throw new DomainException("Apenas pagamentos em processamento podem ser recusados.");

        ArgumentException.ThrowIfNullOrWhiteSpace(reason, nameof(reason));

        Status = PaymentStatus.Refused;
        RefusalReason = reason;
    }

    /// <summary>
    /// Cancela um pagamento pendente antes de processar.
    /// </summary>
    public void Cancel()
    {
        if (Status is not (PaymentStatus.Pending or PaymentStatus.Processing))
            throw new DomainException("Apenas pagamentos pendentes ou em processamento podem ser cancelados.");

        Status = PaymentStatus.Cancelled;
    }

    /// <summary>
    /// Estorna um pagamento já aprovado.
    /// </summary>
    public void Refund()
    {
        if (Status != PaymentStatus.Approved)
            throw new DomainException("Apenas pagamentos aprovados podem ser estornados.");

        Status = PaymentStatus.Refunded;
        RefundedAt = DateTime.UtcNow;
    }
}