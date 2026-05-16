// PulseHub.Application/Common/Result.cs

namespace PulseHub.Application.Common;

///<summary>
/// Result Pattern — retorna sucesso ou falha sem lançar exceções.
/// Muito usado em sistemas modernos para fluxo de controle explícito.
///</summary>
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}