using Microsoft.Playwright;

namespace eAgendaWeb.Testes.E2E.Modulos.ModuloCompromisso;

public class CompromissoExcluirPage
{
    private readonly IPage page;
    private readonly string urlBase;

    public string Url => $"{urlBase}/Compromisso/Excluir";

    public ILocator MensagemConfirmacao =>
        page.GetByText("Deseja realmente excluir este compromisso?");

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

    public CompromissoExcluirPage(IPage page, string urlBase)
    {
        this.page = page;
        this.urlBase = urlBase;
    }

    public ILocator AssuntoDoCompromisso(string assunto)
    {
        return page.GetByText(assunto, new()
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