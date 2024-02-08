﻿namespace PostMortem.Debugging.DOMAIN.Models;

public class Setup
{
    public string Method { get; set; } = string.Empty;
    public object[]? Input { get; set; } = null;
    public object? Output { get; set; } = null;
}
