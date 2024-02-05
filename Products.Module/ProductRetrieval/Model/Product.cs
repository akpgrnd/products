namespace Products.Module.Models;

using Department = string;

public record Product(int Id, string Name, string Colour, Department Department);
