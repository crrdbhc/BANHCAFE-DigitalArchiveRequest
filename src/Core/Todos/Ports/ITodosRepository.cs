using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Data;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Ports;

public interface ITodosRepository
{
    Task<TodoDto> CreateAsync(TodoDto dto, CancellationToken cancellationToken);
}
