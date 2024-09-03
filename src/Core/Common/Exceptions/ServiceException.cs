namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Exceptions;

public sealed class ServiceException : Exception
{
    public ServiceException()
        : base("Ocurrio un error.") { }

    public ServiceException(string message)
        : base(message) { }
}
