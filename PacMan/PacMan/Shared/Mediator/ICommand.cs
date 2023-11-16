using MediatR;

namespace PacMan.Shared.Mediator
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
}