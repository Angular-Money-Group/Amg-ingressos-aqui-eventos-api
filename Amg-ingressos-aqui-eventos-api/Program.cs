using Amg_ingressos_aqui_eventos_api.Infra;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Repository;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.Configure<EventDatabaseSettings>(
    builder.Configuration.GetSection("EventDatabase"));

// injecao de dependencia
//services
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IVariantService, VariantService>();
builder.Services.AddScoped<ILotService, LotService>();
builder.Services.AddScoped<ITicketService, TicketService>();
//repository
builder.Services.AddScoped<IEventRepository, EventRepository<object>>();
builder.Services.AddScoped<IVariantRepository, VariantRepository<object>>();
builder.Services.AddScoped<ILotRepository, LotRepository<object>>();
builder.Services.AddScoped<ITicketRepository, TicketRepository<object>>();
//infra
builder.Services.AddScoped<IDbConnection<Event>, DbConnection<Event>>();
builder.Services.AddScoped<IDbConnection<Variant>, DbConnection<Variant>>();
builder.Services.AddScoped<IDbConnection<Lot>, DbConnection<Lot>>();
builder.Services.AddScoped<IDbConnection<Ticket>, DbConnection<Ticket>>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Certifique-se de ter essa linha
app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

app.UseAuthorization();

app.MapControllers();

app.Run();
