// PulseHub.Domain/Interfaces/IUserRepository.cs

using PulseHub.Domain.Entities;

namespace PulseHub.Domain.Interfaces;

///<summary>
/// Interface de repositório definida no Domain.
/// O Domain diz O QUE precisa — não COMO é feito.
/// Quem implementa é a Infrastructure.
///</summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<(IEnumerable<User> Users, int Total)> GetPagedAsync( string? searchTerm = null,
      bool? isActive = null,int page = 1, int pageSize = 20, 
      CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithOrdersAsync(Guid id,CancellationToken cancellationToken = default);
}