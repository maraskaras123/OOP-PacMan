using MediatR;

namespace PacMan.Shared.Patterns.Mediator
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}