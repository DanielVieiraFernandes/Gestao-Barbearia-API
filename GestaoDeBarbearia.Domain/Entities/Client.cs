﻿namespace GestaoDeBarbearia.Domain.Entities;
public class Client
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
