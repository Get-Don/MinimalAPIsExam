using ApiServer.Config;
using ApiServer.Endpoint;
using ApiServer.GameData;
using ApiServer.Middleware;
using ApiServer.Protocol;
using FluentValidation;
using MemoryDB;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Repository;
using StackExchange.Redis;

namespace ApiServer;

public static class AppInitializer
{
    /// <summary>
    /// 서비스 등록 
    /// </summary>
    public static void InitServices(WebApplicationBuilder builder, string[] args)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            // 🔐 Authorization 헤더 정의
            options.AddSecurityDefinition("AccountIdAuth", new OpenApiSecurityScheme
            {
                Name = "Authorization", // 👈 실제 헤더 키
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "AccountID를 Authorization 헤더에 입력하세요. (예: 123456)"
            });

            // 🔐 이 헤더를 모든 API 요청에 적용
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "AccountIdAuth"
                        }
                    },
                    []
                }
            });
        });
        
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        
        builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(builder.Configuration["ConnectionStrings:Redis"]!)
        );

        builder.Services.AddScoped<IAccountCache, AccountCache>(serviceProvider =>
        {
            var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
            var connectionMultiplexer = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
            return new AccountCache(connectionMultiplexer, appSettings.LoginExpiredMinute);
        });
    }

    /// <summary>
    /// 앱 설정 전 게임 관련 초기화
    /// </summary>
    public static void InitGame(IConfiguration config)
    {
        var gameDataDir = config["GameDataDir"];
        if (string.IsNullOrWhiteSpace(gameDataDir))
        {
            throw new InvalidOperationException("GameDataDir 설정이 누락되었습니다.");
        }
        
        GameDataContainer.Instance.Load(gameDataDir);
    }

    /// <summary>
    /// 앱 초기화
    /// </summary>
    public static void InitApp(WebApplicationBuilder builder, WebApplication app)
    {
        if (builder.Environment.IsDevelopment())
        {
            // 개발 환경에서 예외 상세 메시지 전송
            app.UseDeveloperExceptionPage();
            
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    await ErrorReporter.NotifyExceptionAsync(exception);

                    // 모든 응답의 StatusCode는 200으로 처리했다.
                    // ApiResponse의 IsSuccess와 ErrorCode를 통해 요청 결과 판단
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(new ApiResponse
                    {
                        IsSuccess = false,
                        ErrorCode = ErrorCode.InternalServerError
                    });
                });
            });
        }
        
        app.UseMiddleware<AuthCheckMiddleware>();
    }
    
    /// <summary>
    /// Endpoint 초기화 
    /// </summary>
    public static void InitRoute(WebApplication app)
    {
        app.MapGroup("/api/account").MapAccountEndpoints();
        app.MapGroup("/api/game").MapGameEndpoints();
    }

}