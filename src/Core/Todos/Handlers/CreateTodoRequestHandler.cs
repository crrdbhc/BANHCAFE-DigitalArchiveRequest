using AutoMapper;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Contracts.Response;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Ports;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Data;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Request;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Response;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Ports;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Handlers;

public sealed class CreateTodoRequestHandler(
    ILogger<CreateTodoRequestHandler> logger,
    ITodosRepository repository,
    IValidator<CreateTodoRequest> validator,
    IUserContext user
) : IRequestHandler<CreateTodoRequest, ApiResponse<GetTodoResponse>>
{
    public async Task<ApiResponse<GetTodoResponse>> Handle(
        CreateTodoRequest request,
        CancellationToken cancellationToken
    )
    {
        // get current user Id
        // var id = _user.User.GetUserId();
        // Console.WriteLine($"Current UserId = {id}");

        ApiResponse<GetTodoResponse> response = new();

        try
        {
            var validationResults = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResults.IsValid)
            {
                response.ValidationErrors = validationResults.ToDictionary();
                goto end;
            }

            var mapper = new MapperConfiguration(m =>
                m.CreateMap<CreateTodoRequest, TodoDto>()
            ).CreateMapper();

            var model = mapper.Map<TodoDto>(request);

            var todo = await repository.CreateAsync(model, cancellationToken);

            mapper = new MapperConfiguration(m =>
                m.CreateMap<TodoDto, GetTodoResponse>()
            ).CreateMapper();

            response.Data = mapper.Map<GetTodoResponse>(todo);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error");
        }

        end:
        return response;
    }
}
