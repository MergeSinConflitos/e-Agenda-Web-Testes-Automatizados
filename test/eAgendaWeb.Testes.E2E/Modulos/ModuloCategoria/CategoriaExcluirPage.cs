using Microsoft.Playwright;

namespace eAgendaWeb.Testes.E2E.Modulos.ModuloCategoria;

public class CategoriaExcluirPage
{
    private readonly IPage page;
    private readonly string urlBase;

    public string Url => $"{urlBase}/Categoria/Excluir";

    public ILocator MensagemConfirmacao =>
        page.GetByText("Deseja realmente excluir esta categoria?");

    public ILocator Confirmar =>
        page.GetByRole(AriaRole.Button, new()
        {
            Name = "Confirmar"
        });

    public ILocator Voltar =>
        page.GetByRole(AriaRole.Link, new()
        {
            Name = "Voltar"
        });

    public CategoriaExcluirPage(IPage page, string urlBase)
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

    public async Task ConfirmarAsync()
    {
        await Confirmar.ClickAsync();
    }

    public async Task VoltarAsync()
    {
        await Voltar.ClickAsync();
    }
}