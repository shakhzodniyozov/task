using Application.Features.Users.DTOs;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Queries;

public class GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public GetUserByIdQuery(Guid id) => Id = id;

    public Guid Id { get; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IRepository<User> userRepo;
    private readonly IMapper mapper;

    public GetUserByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        userRepo = uow.GetRepository<User>();
        this.mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id);

        return user is not null ? Result.Ok(mapper.Map<UserDto>(user)) : Result.Fail<UserDto>($"User with provided Id={request.Id} was not found.");
    }
}
