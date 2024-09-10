using AutoMapper;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Contracts.Response;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Exceptions;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Ports;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Models;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Ports;
using FluentValidation;
using MediatR;
using Microsoft.VisualBasic;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Core.Migrations.Commands.Create
{
    public sealed class CreateMigrationsCommand : IRequest<ApiResponse<MigrationsBase>>
    {
        public CreateMigrations Migrations { get; set; }
    }

    public sealed class MigrationsCommandHandler(
        IMigrationsRepository repository,
        IUserContext userContext,
        IMapper mapper,
        IValidator<CreateMigrations> _validator
    ) : IRequestHandler<CreateMigrationsCommand, ApiResponse<MigrationsBase>>
    {
        public async Task<ApiResponse<MigrationsBase>> Handle(
            CreateMigrationsCommand request,
            CancellationToken cancellationToken
        )
        {
            var handlerResponse = new ApiResponse<MigrationsBase>();

            var validationResults = await _validator.ValidateAsync(
                request.Migrations,
                cancellationToken
            );

            if (!validationResults.IsValid)
            {
                handlerResponse.ValidationErrors = validationResults.ToDictionary();
                return handlerResponse;
            }

            var dto = mapper.Map<CreateMigrationsDto>(request.Migrations);

            try
            {
                var results = await repository.Create(dto, cancellationToken);
                handlerResponse.Data = results;
            }
            catch (ServiceException ex)
            {
                handlerResponse.AddError(ex.Message);
            }

            return handlerResponse;
        }
    }
}
