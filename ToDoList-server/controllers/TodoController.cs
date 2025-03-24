using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi;

[Route("api/[controller]")]
[ApiController]
public class TodoController:ControllerBase
{
    private readonly ToDoDbContext _context;
    public TodoController(ToDoDbContext context)
    {
        _context=context;
    }
    [HttpGet]
    public IActionResult GetTodos()
    {
        var todos=_context.Items.ToList();
        return Ok(todos);
    }
}