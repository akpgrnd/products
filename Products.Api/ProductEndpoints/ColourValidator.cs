using System.Text.RegularExpressions;

namespace Products.Api.ProductEndpoints;

public class ColourValidator : IEndpointFilter
{
    private static readonly Regex ValidRegex = new("^[a-zA-Z]{3,15}$", RegexOptions.Compiled);

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var colour = context.Arguments[1] as string;

        if (!ValidRegex.IsMatch(colour!))
        {
            var dict = new Dictionary<string, string[]> {
                { "Colour", [ "Colour must be one word with length between 3 and 15 characters containing letters a to z."] }
            };

            return Results.ValidationProblem(dict);
        }

        return await next(context);
    }
}
