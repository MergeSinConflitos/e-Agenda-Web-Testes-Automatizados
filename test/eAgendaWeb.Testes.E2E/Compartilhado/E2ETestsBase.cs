using Microsoft.Playwright.MSTest;

public abstract class E2ETestsBase : PageTest
{
    private TestApplicationFactory aplicacao = null!;

    protected string UrlBase { get; private set; } = string.Empty;

    [TestInitialize]
    public void InicializarAplicacao()
    {
        aplicacao = new TestApplicationFactory();
        UrlBase = aplicacao.UrlBase;
    }

    [TestCleanup]
    public async Task EncerrarAplicacao()
    {
        if (aplicacao is not null)
            await aplicacao.DisposeAsync();
    }
}