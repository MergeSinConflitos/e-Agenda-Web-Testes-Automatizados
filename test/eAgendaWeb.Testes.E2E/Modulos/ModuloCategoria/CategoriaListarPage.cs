using Microsoft.Playwright;

namespace eAgendaWeb.Testes.E2E.Modulos.ModuloCategoria;

public class CategoriaListarPage
{
    private readonly IPage page;
    private readonly string urlBase;

    public string Url => $"{urlBase}/Categoria/Listar";

    public ILocator Titulo =>
        page.GetByText("Listagem de Categorias");

    public ILocator CadastrarNovo =>
        page.GetByText("Cadastrar Nova");

    public ILocator EstadoVazio =>
        page.GetByText("Nenhuma categoria cadastrada.");

    public ILocator MensagemErro =>
        page.Locator(".alert-danger");

    public CategoriaListarPage(IPage page, string urlBase)
    {
        this.page = page;
        this.urlBase = urlBase;
    }

    public ILocator TituloDaCategoria(string titulo)
    {
        return page.GetByText(titulo, new()
        {
            Exact = true
        });
    }

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task EditarAsync(string titulo)
    {
        ILocator categoria = TituloDaCategoria(titulo);

        ILocator card = categoria.Locator(
            "xpath=ancestor::div[contains(@class,'card')][1]"
        );

        await card.GetByRole(AriaRole.Link, new()
        {
            Name = "Editar"
        }).ClickAsync();
    }

    public async Task ExcluirAsync(string titulo)
    {
        ILocator categoria = TituloDaCategoria(titulo);

        ILocator card = categoria.Locator(
            "xpath=ancestor::div[contains(@class,'card')][1]"
        );

        await card.GetByRole(AriaRole.Link, new()
        {
            Name = "Excluir"
        }).ClickAsync();
    }
}