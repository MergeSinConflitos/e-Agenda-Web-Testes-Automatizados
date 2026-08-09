using Microsoft.Playwright;

namespace eAgendaWeb.Testes.E2E.Modulos.ModuloCategoria;

public class CategoriaFormPage
{
    private readonly IPage page;
    private readonly string urlBase;

    public string Url => $"{urlBase}/Categoria/Cadastrar";

    public ILocator Titulo =>
        page.GetByLabel("Título");

    public ILocator ErroTituloObrigatorio =>
        page.GetByText(
            "O campo \"Título\" deve ser preenchido."
        );

    public ILocator ErroTituloTamanho =>
        page.GetByText(
            "O campo \"Título\" deve conter entre 2 e 100 caracteres."
        );

    public ILocator ErroTituloDuplicado =>
        page.GetByText(
            "Já existe uma categoria com este título."
        );

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

    public CategoriaFormPage(IPage page, string urlBase)
    {
        this.page = page;
        this.urlBase = urlBase;
    }

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task PreencherTituloAsync(string titulo)
    {
        await Titulo.FillAsync(titulo);
    }

    public async Task ConfirmarAsync()
    {
        await Confirmar.ClickAsync();
    }

    public async Task DesabilitarValidacaoNativaAsync()
    {
        await page.Locator("form").EvaluateAsync(
            "form => form.noValidate = true"
        );
    }
}