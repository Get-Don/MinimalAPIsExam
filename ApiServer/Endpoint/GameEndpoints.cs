using ApiServer.Protocol;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Repository;

namespace ApiServer.Endpoint;

public static class GameEndpoints
{
    public static RouteGroupBuilder MapGameEndpoints(this RouteGroupBuilder routeGroup)
    {
        routeGroup.MapPost("/get-money", GetMoney);
        routeGroup.MapPost("/get-stats", GetStats);
        routeGroup.MapPost("/upgrade-stats", UpgradeStats);
        
        return routeGroup;
    }
    
    private static async Task<ApiResponse<List<MoneyDTO>>> GetMoney(AccountDTO accountDTO, IMapper mapper, IGameRepository repository)
    {
        var response = new ApiResponse<List<MoneyDTO>>();
        
        try
        {
            var dbData = await repository.LoadMoney(accountDTO.AccountId);
            response.Result = mapper.Map<List<MoneyDTO>>(dbData);
            return response;
        }
        catch (Exception e)
        {
            await ErrorReporter.NotifyExceptionAsync(e);
            return ErrorReporter.ResponseException(response);
        }
    }

    private static async Task<ApiResponse<List<StatDTO>>> GetStats(AccountDTO accountDTO, IMapper mapper, IGameRepository repository)
    {
        var response = new ApiResponse<List<StatDTO>>();
        
        try
        {
            var dbData = await repository.LoadStat(accountDTO.AccountId);
            response.Result = mapper.Map<List<StatDTO>>(dbData);
            return response;
        }
        catch (Exception e)
        {
            await ErrorReporter.NotifyExceptionAsync(e);
            return ErrorReporter.ResponseException(response);
        }
    }

    private static async Task<ApiResponse> UpgradeStats(UpgradeStatsDTO upgradeStatsDTO, IGameRepository repository)
    {
        var response = new ApiResponse();
        
        try
        {
            // 미작성
        
            return response;
        }
        catch (Exception e)
        {
            await ErrorReporter.NotifyExceptionAsync(e);
            return ErrorReporter.ResponseException(response);
        }
    }
}