using Microsoft.EntityFrameworkCore;
using MiHotel.WebApi.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(ops => 
    ops.UseSqlServer(builder.Configuration.GetConnectionString("Conexion")));

builder.Services.AddControllers().AddJsonOptions(js => js.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//builder.Services.AddMvc().AddNewtonsoftJson(options =>
//               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

//JsonSerializerOptions options = new()
//{
//    ReferenceHandler = ReferenceHandler.IgnoreCycles,
//    WriteIndented = true
//};

//builder.Services.AddMvc().AddJsonOptions(ops => ops.JsonSerializerOptions = options);

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

//Si no existe DB la crea.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();    
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
