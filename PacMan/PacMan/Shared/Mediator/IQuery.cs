using MediatR;

namespace PacMan.Shared.Mediator
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}