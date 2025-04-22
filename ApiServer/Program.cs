using ApiServer;

var builder = WebApplication.CreateBuilder(args);
AppInitializer.InitServices(builder, args);

AppInitializer.InitGame(builder.Configuration);

var app = builder.Build();
AppInitializer.InitApp(builder, app);
AppInitializer.InitRoute(app);

app.Run();