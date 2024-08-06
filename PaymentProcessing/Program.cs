using Microsoft.EntityFrameworkCore;
using PaymentProcessing;
using PaymentProcessing.Models;

var builder = Host.CreateApplicationBuilder(args);

// baca config
var configuration = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

// baca connection string
var connectionString = configuration.GetConnectionString("HotelDB");

// DI
builder.Services.AddDbContext<HotelReservationSystemDBContext>(options => options.UseSqlServer(connectionString));

// service
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
