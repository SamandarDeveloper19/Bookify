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

    public async Task<UserRolesResponse?> GetRolesForUserAsync(string identityId)
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
            .FirstOrDefaultAsync();

        if (roles is null)
        {
            return null;
        }

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

        var userExists = await dbContext.Set<User>()
            .AnyAsync(user => user.IdentityId == identityId);

        if (!userExists)
        {
            return [];
        }

        var permissions = await dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .SelectMany(user => user.Roles)
            .SelectMany(role => role.Permissions)
            .Select(permission => permission.Name)
            .Distinct()
            .ToListAsync();

        var permissionsSet = permissions.ToHashSet();

        await cacheService.SetAsync(cacheKey, permissionsSet);

        return permissionsSet;
    }
}
