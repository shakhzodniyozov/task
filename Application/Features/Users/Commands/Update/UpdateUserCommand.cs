using Application.Common.Services;
using Application.Features.Users.DTOs;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Commands.Update;

public class UpdateUserCommand : IRequest<Result<UserDto>>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    private readonly IUnitOfWork uow;
    private readonly IRepository<User> userRepo;
    private readonly IMapper mapper;
    private readonly IAuthService authService;

    public UpdateUserCommandHandler(IUnitOfWork uow, IAuthService authService, IMapper mapper)
    {
        this.uow = uow;
        userRepo = uow.GetRepository<User>();
        this.mapper = mapper;
        this.authService = authService;
    }

    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = authService.GetUserId();
        var user = await userRepo.GetFirstOrDefaultAsync(predicate: x => x.Id == userId, disableTracking: false);

        mapper.Map(request, user);
        user.UpdatedAt = DateTime.UtcNow;
        await uow.SaveChangesAsync();

        return Result.Ok(mapper.Map<UserDto>(user));
    }
}