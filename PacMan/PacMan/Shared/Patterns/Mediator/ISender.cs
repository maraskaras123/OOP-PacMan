using MediatR;

namespace PacMan.Shared.Patterns.Mediator
{
    public interface ISender
    {
        Task Send<TRequest>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest;
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
    }
}