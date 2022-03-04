using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Git.Application.Clients.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Fullname { get; set; }
    public string Pic { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}