using Amg_ingressos_aqui_eventos_api.Infra;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Repository;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.Configure<EventDatabaseSettings>(
    builder.Configuration.GetSection("EventDatabase"));
builder.Services.Configure<CieloSettings>(
    builder.Configuration.GetSection("CieloSettings"));

builder.Services.Configure<MvcOptions>(options =>
{
    options.ModelMetadataDetailsProviders.Add(
        new SystemTextJsonValidationMetadataProvider());
});

// injecao de dependencia
//services
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IVariantService, VariantService>();
builder.Services.AddScoped<ILotService, LotService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportEventTickets, ReportEventTicketsService>();
builder.Services.AddScoped<IReportEventTransactions, ReportEventTransactionsService>();
builder.Services.AddScoped<IEntranceService, EntranceService>();
//repository
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IVariantRepository, VariantRepository<object>>();
builder.Services.AddScoped<ILotRepository, LotRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository<object>>();
builder.Services.AddScoped<ITicketRowRepository, TicketRowRepository<object>>();
builder.Services.AddScoped<IEntranceRepository, EntranceRepository<object>>();
//infra
builder.Services.AddScoped<IDbConnection<Event>, DbConnection<Event>>();
builder.Services.AddScoped<IDbConnection<Variant>, DbConnection<Variant>>();
builder.Services.AddScoped<IDbConnection<Lot>, DbConnection<Lot>>();
builder.Services.AddScoped<IDbConnection<Ticket>, DbConnection<Ticket>>();
builder.Services.AddScoped<IDbConnection<Email>, DbConnection<Email>>();
builder.Services.AddScoped<IDbConnection<StatusTicketsRow>, DbConnection<StatusTicketsRow>>();
builder.Services.AddScoped<IDbConnection<User>, DbConnection<User>>();
builder.Services.AddScoped<IDbConnection<ReadHistory>, DbConnection<ReadHistory>>();
builder.Services.AddScoped<IDbConnection<EventQrReads>, DbConnection<EventQrReads>>();

builder.Services.AddScoped<ICieloClient, CieloClient>();

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
app.UseMiddleware<EventsExceptionHandlerMiddleaware>();
app.Run();
