using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    protected readonly string _connectionString;

    public BaseController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }
}
