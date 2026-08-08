using Microsoft.Playwright;

namespace eAgendaWeb.Testes.E2E.Modulos.ModuloContato;

public class ContatoExcluirPage
{
    private readonly IPage page;

    private readonly string urlBase;

    public string Url => $"{urlBase}/Contato/excluir";


    public ILocator MensagemConfirmacao =>
        page.GetByText("Deseja realmente excluir este contato?");

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

    public ContatoExcluirPage(IPage page)
    {
        this.page = page;
    }

    public ILocator NomeDoContato(string nome)
    {
        return page.GetByText(nome, new()
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