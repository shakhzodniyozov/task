using Application.Features.Users.DTOs;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries;

public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
{
    public GetAllUsersQuery(int pageIndex, int pageSize)
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUnitOfWork uow;
    private readonly IRepository<User> userRepo;
    private readonly IMapper mapper;

    public GetAllUsersQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        userRepo = uow.GetRepository<User>();
        this.mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepo.GetPagedListAsync(pageIndex: request.PageIndex, pageSize: request.PageSize);

        return mapper.Map<IEnumerable<UserDto>>(users.Items);
    }
}