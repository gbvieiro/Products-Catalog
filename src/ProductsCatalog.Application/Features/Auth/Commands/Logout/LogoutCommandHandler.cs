using MediatR;

namespace ProductsCatalog.Application.Features.Auth.Commands.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    public Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(Unit.Value);
}
