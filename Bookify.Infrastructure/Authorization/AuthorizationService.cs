using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService
{
    private readonly ApplicationDbContext dbContext;
    private readonly ICacheService cacheService;

    public AuthorizationService(
        ApplicationDbContext dbContext,
        ICacheService cacheService)
    {
        this.dbContext = dbContext;
        this.cacheService = cacheService;
    }

    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId)
    {
        var cacheKey = $"auth:roles-{identityId}";

        var cachedRoles = await cacheService.GetAsync<UserRolesResponse>(cacheKey);

        if (cachedRoles is not null)
        {
            return cachedRoles;
        }

        var roles = await dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            })
            .FirstAsync();

        await cacheService.SetAsync(cacheKey, roles);

        return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var cacheKey = $"auth:permissions-{identityId}";

        var cachedPermissions = await cacheService.GetAsync<HashSet<string>>(cacheKey);

        if (cachedPermissions is not null)
        {
            return cachedPermissions;
        }

        var permissions = await dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .SelectMany(user => user.Roles.Select(role => role.Permissions))
            .FirstAsync();

        var permissionsSet = permissions.Select(p => p.Name).ToHashSet();

        await cacheService.SetAsync(cacheKey, permissionsSet);

        return permissionsSet;
    }
}
