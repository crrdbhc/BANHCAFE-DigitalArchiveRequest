using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Contracts.Response;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Request;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Todos.Contracts.Response;
using Bogus.Extensions;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.IntegrationTests.Todos.Handlers;

public sealed class CreateTodoRequestHandlerTests : IntegrationTest
{
    [Fact]
    public async Task CreateTodoWithLongName_ShouldGiveValidationErrors()
    {
        // arrange
        var faker = new Faker<CreateTodoRequest>().CustomInstantiator(f => new CreateTodoRequest(
            Name: f.Hacker.Phrase().ClampLength(51, 100)
        ));
        var request = faker.Generate();

        // act
        var result = await SendAsync<ApiResponse<GetTodoResponse>>(request);

        result.ValidationErrors.Should().NotBeEmpty();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task CreateTodoWithValidData_ShouldReturnCreatedTodo()
    {
        // arrange
        var faker = new Faker<CreateTodoRequest>().CustomInstantiator(f => new CreateTodoRequest(
            Name: f.Hacker.Phrase().ClampLength(3, 49)
        ));
        var request = faker.Generate();

        // act
        var result = await SendAsync<ApiResponse<GetTodoResponse>>(request);

        result.ValidationErrors.Should().BeEmpty();
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().BeEquivalentTo(request.Name);
    }
}
