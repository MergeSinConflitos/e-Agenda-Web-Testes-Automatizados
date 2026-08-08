using Microsoft.Playwright;

namespace eAgendaWeb.Testes.E2E.Modulos.ModuloContato;

public class ContatoListarPage
{
    private readonly IPage page;
    private readonly string urlBase;

    public string Url => $"{urlBase}/Contato/Listar";

    public ILocator Titulo =>
        page.GetByText("Listagem de Contatos");

    public ILocator CadastrarNovo =>
        page.GetByText("Cadastrar Novo");

    public ILocator EstadoVazio =>
        page.GetByText("Nenhum contato cadastrado.");

    public ILocator MensagemErro =>
        page.Locator(".alert-danger");

    public ContatoListarPage(IPage page, string urlBase)
    {
        this.page = page;
        this.urlBase = urlBase;
    }

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public ILocator NomeDoContato(string nome)
    {
        return page.GetByText(nome, new() { Exact = true });
    }

    public async Task EditarAsync(string nome)
    {
        ILocator contato = NomeDoContato(nome);

        ILocator card = contato.Locator(
            "xpath=ancestor::div[contains(@class,'card')][1]"
        );

        await card.GetByRole(AriaRole.Link, new()
        {
            Name = "Editar"
        }).ClickAsync();
    }

    public async Task ExcluirAsync(string nome)
    {
        ILocator contato = NomeDoContato(nome);

        ILocator card = contato.Locator(
            "xpath=ancestor::div[contains(@class,'card')][1]"
        );

        await card.GetByRole(AriaRole.Link, new()
        {
            Name = "Excluir"
        }).ClickAsync();
    }
}