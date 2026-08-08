using Microsoft.Playwright;

namespace eAgendaWeb.Testes.E2E.Modulos.ModuloCompromisso;

public class CompromissoListarPage
{
    private readonly IPage page;
    private readonly string urlBase;

    public string Url => $"{urlBase}/Compromisso/Listar";

    public ILocator Titulo =>
        page.GetByText("Listagem de Compromissos");

    public ILocator CadastrarNovo =>
        page.GetByText("Cadastrar Novo");

    public ILocator EstadoVazio =>
        page.GetByText("Nenhum compromisso cadastrado.");

    public ILocator MensagemErro =>
        page.Locator(".alert-danger");

    public CompromissoListarPage(IPage page, string urlBase)
    {
        this.page = page;
        this.urlBase = urlBase;
    }

    public ILocator AssuntoDoCompromisso(string assunto)
    {
        return page.GetByText(assunto, new() { Exact = true });
    }

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task EditarAsync(string assunto)
    {
        ILocator compromisso = AssuntoDoCompromisso(assunto);

        ILocator card = compromisso.Locator(
            "xpath=ancestor::div[contains(@class,'card')][1]"
        );

        await card.GetByRole(AriaRole.Link, new()
        {
            Name = "Editar"
        }).ClickAsync();
    }

    public async Task ExcluirAsync(string assunto)
    {
        ILocator compromisso = AssuntoDoCompromisso(assunto);

        ILocator card = compromisso.Locator(
            "xpath=ancestor::div[contains(@class,'card')][1]"
        );

        await card.GetByRole(AriaRole.Link, new()
        {
            Name = "Excluir"
        }).ClickAsync();
    }
}