namespace Bookify.Api.FunctionalTests.Infrastructure;

[CollectionDefinition(Name)]
public sealed class FunctionalTestCollection : ICollectionFixture<FunctionalTestWebAppFactory>
{
    public const string Name = nameof(FunctionalTestCollection);
}
