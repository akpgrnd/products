namespace Products.Api.HealthChecks
{
    public static class HealthCheckExtensions
    {
        public static WebApplication MapHealthCheckEndpoints(this WebApplication app)
        {
            // add health check endpoint to root.
            // Additional helthchecks would be wise once external dependencies are added to this project
            app.MapHealthChecks(""); 

            return app;
        }
    }
}
