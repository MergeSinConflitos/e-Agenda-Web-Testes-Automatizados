using Microsoft.Playwright;

namespace eAgendaWeb.Testes.E2E.Modulos.ModuloContato;

public class ContatoFormPage
{
    private readonly IPage page;
    private readonly string urlBase;

    public string Url => $"{urlBase}/Contato/Cadastrar";

    public ILocator Nome =>
        page.GetByLabel("Nome");

    public ILocator Email =>
        page.GetByLabel("E-mail");

    public ILocator Telefone =>
        page.GetByLabel("Telefone");

    public ILocator Cargo =>
        page.GetByLabel("Cargo");

    public ILocator Empresa =>
        page.GetByLabel("Empresa");

    public ILocator ErroNomeObrigatorio =>
     page.GetByText(
         "O campo \"Nome\" deve ser preenchido."
     );

    public ILocator ErroEmailObrigatorio =>
        page.GetByText(
            "O campo \"E-mail\" deve ser preenchido."
        );

    public ILocator ErroTelefoneObrigatorio =>
        page.GetByText(
            "O campo \"Telefone\" deve ser preenchido."
        );

    public ILocator ErroNomeTamanho =>
        page.GetByText(
            "O campo \"Nome\" deve conter entre 2 e 100 caracteres."
        );

    public ILocator ErroEmailFormato =>
        page.GetByText(
            "O campo \"E-mail\" deve conter um endereço de e-mail válido."
        );

    public ILocator ErroTelefoneFormato =>
        page.GetByText(
            "O campo \"Telefone\" deve estar no formato (XX) XXXX-XXXX ou (XX) XXXXX-XXXX."

        );

    public ILocator ErroEmailDuplicado =>
page.GetByText
("Já existe um contato com este email.");

    public ILocator ErroTelefoneDuplicado =>
page.GetByText
("Já existe um contato com este telefone.");


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

    public ContatoFormPage(IPage page, string urlBase)
    {
        this.page = page;
        this.urlBase = urlBase;
    }

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task PreencherNomeAsync(string nome)
    {
        await Nome.FillAsync(nome);
    }

    public async Task PreencherEmailAsync(string email)
    {
        await Email.FillAsync(email);
    }

    public async Task PreencherTelefoneAsync(string telefone)
    {
        await Telefone.FillAsync(telefone);
    }

    public async Task PreencherCargoAsync(string cargo)
    {
        await Cargo.FillAsync(cargo);
    }

    public async Task PreencherEmpresaAsync(string empresa)
    {
        await Empresa.FillAsync(empresa);
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