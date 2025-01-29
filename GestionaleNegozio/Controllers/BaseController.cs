using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    protected readonly string _connectionString;

    public BaseController()
    {
        _connectionString = "connection_string";     //dobbiamo metterla
    }
}
