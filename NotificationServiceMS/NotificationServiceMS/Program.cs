using NotificationServiceMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHostedService<RabbitMqListener>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration["RabbitMQ:ConnectionString"] = "amqps://bihjdlmj:y5YYXHG26Ttl6VfN7KU6c3rS3Z0XEUEz@possum.lmq.cloudamqp.com/bihjdlmj";

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
