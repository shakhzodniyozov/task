namespace Application.Features.Users.Queries.GetUserByIdQuery;

public class GetUserByIdDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
