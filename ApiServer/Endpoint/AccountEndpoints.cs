using ApiServer.GameData;
using ApiServer.Protocol;
using ApiServer.Protocol.Validation;
using ApiServer.Utility;
using AutoMapper;
using MemoryDB;
using Microsoft.AspNetCore.Identity;
using Repository;
using Repository.Entyties;

namespace ApiServer.Endpoint;

public static class AccountEndpoints
{
    public static RouteGroupBuilder MapAccountEndpoints(this RouteGroupBuilder routeGroup)
    {
        routeGroup.MapPost("/create", Create).AddEndpointFilter<ValidationFilter<LoginDTO>>();
        routeGroup.MapPost("/login", Login).AddEndpointFilter<ValidationFilter<LoginDTO>>();
        routeGroup.MapPost("/extend-login", ExtendLogin);

        return routeGroup;
    }

    /// <summary>
    /// 계정 생성
    /// </summary>
    private static async Task<ApiResponse> Create(LoginDTO loginDTO,
        IAccountRepository accountRepository, IGameRepository gameRepository)
    {
        var response = new ApiResponse();
        
        try
        {
            var exists = await accountRepository.Exists(loginDTO.Email);
            if (exists)
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.EmailAlreadyExists;
                return response;
            }

            var hasher = new PasswordHasher<Account>();
            var encryptedPassword = hasher.HashPassword(new Account(), loginDTO.Password);

            var accountId = await accountRepository.CreateAccount(loginDTO.Email, encryptedPassword);

            var defaultData = CreateUserDefaultData(accountId);
            if (defaultData is null || !await gameRepository.CreateDefaultData(defaultData))
            {
                // 디폴트 데이터 삽입 실패 시 생성한 계정 삭제.
                // AccountDB와 GameDB를 따로 둘 수 있기 때문에 계정과 데이터를 트랜잭션 처리 하지 않음.
                await accountRepository.RemoveAccount(accountId);
                throw new Exception("failed to create default data");
            }

            return response;
        }
        catch (Exception e)
        {
            await ErrorReporter.NotifyExceptionAsync(e);
            return ErrorReporter.ResponseException(response);
        }
    }

    /// <summary>
    /// 로그인
    /// </summary>
    private static async Task<ApiResponse<AccountDTO>> Login(LoginDTO loginDTO, IMapper mapper,
        IAccountRepository repository, IAccountCache cache)
    {
        var response = new ApiResponse<AccountDTO>();

        try
        {
            var account = await repository.GetAccount(loginDTO.Email);
            if (account == null)
            {
                response.ErrorCode = ErrorCode.AccountNotExist;
                response.IsSuccess = false;
                return response;
            }

            var hasher = new PasswordHasher<Account>();
            var result = hasher.VerifyHashedPassword(new Account(), account.Password, loginDTO.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                response.ErrorCode = ErrorCode.IncorrectPassword;
                response.IsSuccess = false;
                return response;
            }

            // TODO: 중복 로그인등 처리 필요

            await repository.Login(account.AccountId);
            
            // 로그인 캐시.
            // TODO: 기기 정보등 추가 정보 캐시 필요
            await cache.Login(account.AccountId);

            response.Result = mapper.Map<AccountDTO>(account);
            return response;
        }
        catch (Exception e)
        {
            await ErrorReporter.NotifyExceptionAsync(e);
            return ErrorReporter.ResponseException(response);
        }
    }

    /// <summary>
    /// 로그인 시간 연장 (캐시 TTL 연장)
    /// </summary>
    private static async Task<ApiResponse> ExtendLogin(AccountDTO accountDTO, IAccountCache cache)
    {
        var response = new ApiResponse();

        try
        {
            if (!await cache.CheckLogin(accountDTO.AccountId))
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.NotLoggedIn;
                return response;
            }
        
            await cache.ExtendLogin(accountDTO.AccountId);
            return response;
        }
        catch (Exception e)
        {
            await ErrorReporter.NotifyExceptionAsync(e);
            return ErrorReporter.ResponseException(response);
        }
    }

    /// <summary>
    /// 계정 생성 시 GameDB에 삽입할 유저 데이터 세팅
    /// </summary>
    private static UserInitData? CreateUserDefaultData(long accountId)
    {
        var defaultData = new UserInitData
        {
            AccountId = accountId,
            MoneyList = [],
            StatList = []
        };

        // 재화 셋업
        foreach (var money in GameDataContainer.Instance.MoneyGDList)
        {
            defaultData.MoneyList.Add(new Money
            {
                Id = Util.GetUIdToLong(),
                AccountId = accountId,
                MoneyType = (byte)money.MoneyType,
                Value = 0
            });
        }

        // 스탯 정보 셋업
        foreach (var stat in GameDataContainer.Instance.StatGDList)
        {
            defaultData.StatList.Add(new Stat
            {
                Id = Util.GetUIdToLong(),
                AccountId = accountId,
                StatType = (byte)stat.StatType,
                Level = 1
            });
        }
        
        return defaultData;
    }
}