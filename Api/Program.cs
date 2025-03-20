using Api.DTO_s;
using Api.Services.Implementations;
using Api.Services.Interfaces;
using Api.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<INegotiationService, NegotiationService>();

builder.Services.AddScoped<IValidator<CreateProductDTO>,CreateProductDtoValidator>();
builder.Services.AddScoped<IValidator<PriceProposalDTO>,PriceProposalDtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();