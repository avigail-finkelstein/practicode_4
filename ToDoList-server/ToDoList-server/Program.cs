
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// בדיקה אם ה-ConnectionString קיים
var connectionString = builder.Configuration.GetConnectionString("ToDoDb");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Missing connection string for ToDoDb");
}

// רישום ה-DbContext
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.41-mysql"))
);

// רישום CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// רישום השירותים
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// שימוש ב-CORS
app.UseCors("AllowAll");

// Middleware
app.UseAuthorization();
app.MapControllers();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

// מיפוי נתיבים
app.MapGet("/items", async (ToDoDbContext db) =>
{
    var items = await db.Items.ToListAsync();
    return Results.Ok(items);
});

app.MapPut("/addItems", async (ToDoDbContext db, Item item) =>
{
    var newItem = await db.Items.AddAsync(item);
    await db.SaveChangesAsync();
    return Results.Ok(newItem.Entity);
});

app.MapPost("/updateItems/{id}", async (int id, ToDoDbContext db, Item updateItem) =>
{
    var toUpdate = await db.Items.FindAsync(id);
    if (toUpdate is null) return Results.NotFound();

    toUpdate.IsComplete = updateItem.IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapDelete("/DeleteItems/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
