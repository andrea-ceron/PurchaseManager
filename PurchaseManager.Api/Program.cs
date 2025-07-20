using Microsoft.EntityFrameworkCore;
using PurchaseManager.Api.Middlewares;
using PurchaseManager.Business;
using PurchaseManager.Business.Abstraction;
using PurchaseManager.Business.Kafka;
using PurchaseManager.Business.Kafka.MessageHandler;
using PurchaseManager.Repository;
using PurchaseManager.Repository.Abstraction;
using Utility.Kafka.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PurchaseDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("PurchaseDbContext"),
		b => b.MigrationsAssembly("PurchaseManager.Api")
	)
);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();
builder.Services.AddSingleton(p => ActivatorUtilities.CreateInstance<Subject>(p));
builder.Services.AddSingleton<IRawMaterialsObservable>(p => p.GetRequiredService<Subject>());
builder.Services.AddSingleton<IRawMaterialObserver>(p => p.GetRequiredService<Subject>());
builder.Services.AddKafkaConsumerAndProducer<KafkaTopicsInput, KafkaTopicsOutput, MessageHandlerFactory, ProducerServiceWithSubscription>(builder.Configuration);
builder.Logging.AddConsole();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	try
	{
		var db = scope.ServiceProvider.GetRequiredService<PurchaseDbContext>();
		db.Database.Migrate();
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
	}
}
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
