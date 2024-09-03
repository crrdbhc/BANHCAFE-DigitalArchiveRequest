using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Contracts.Response;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Response;
using MediatR;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Request;

public sealed record CreateTodoRequest(string Name) : IRequest<ApiResponse<GetTodoResponse>>;
