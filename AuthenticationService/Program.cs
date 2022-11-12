using AuthenticationService;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
Startup startup = new(builder.Environment);
startup.ConfigureServices(builder.Services);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
startup.Configure(app);

app.Run("http://localhost:5000");
