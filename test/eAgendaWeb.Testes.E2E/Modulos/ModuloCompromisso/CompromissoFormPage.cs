using Microsoft.Playwright;

namespace eAgendaWeb.Testes.E2E.Modulos.ModuloCompromisso;

public class CompromissoFormPage
{
    private readonly IPage page;
    private readonly string urlBase;

    public string Url => $"{urlBase}/Compromisso/Cadastrar";

    public ILocator Assunto =>
        page.GetByLabel("Assunto");

    public ILocator DataOcorrencia =>
        page.GetByLabel("Data de Ocorrência");

    public ILocator HoraInicio =>
        page.GetByLabel("Hora de Início");

    public ILocator HoraTermino =>
        page.GetByLabel("Hora de Término");

    public ILocator Tipo =>
        page.GetByLabel("Tipo de Compromisso");

    public ILocator Contato =>
        page.GetByLabel("Contato");

    public ILocator Local =>
        page.GetByLabel("Local");

    public ILocator Link =>
        page.GetByLabel("Link");

    public ILocator ErroAssuntoObrigatorio =>
        page.GetByText(
            "O campo \"Assunto\" deve ser preenchido."
        );

    public ILocator ErroAssuntoTamanho =>
        page.GetByText(
            "O campo \"Assunto\" deve conter entre 2 e 100 caracteres."
        );

    public ILocator ErroDataOcorrenciaObrigatoria =>
        page.GetByText(
            "O campo \"Data de Ocorrência\" deve ser preenchido."
        );

    public ILocator ErroHoraInicioObrigatoria =>
        page.GetByText(
            "O campo \"Hora de Início\" deve ser preenchido."
        );

    public ILocator ErroHoraTerminoObrigatoria =>
        page.GetByText(
            "O campo \"Hora de Término\" deve ser preenchido."
        );
    public ILocator ErroHoraTerminoMenorQueHoraInicio =>
    page.GetByText(
         "A hora de término deve ser posterior à hora de início."
    );

    public ILocator ErroConflitoDeHorario =>
    page.GetByText(
        "Já existe um compromisso cadastrado neste intervalo de horário."
    );

    public ILocator ErroTipoObrigatorio =>
        page.GetByText(
            "O campo \"Tipo de Compromisso\" deve ser preenchido."
        );

    public ILocator ErroLocalObrigatorio =>
    page.GetByText(
    "O campo \"Local\" deve ser preenchido para compromissos presenciais.");

    public ILocator ErroLinkObrigatorio =>
    page.GetByText(
        "O campo \"Link\" deve ser preenchido para compromissos remotos."
    );

    public ILocator ErroLocalTamanho =>
        page.GetByText(
            "O campo \"Local\" deve conter no máximo 255 caracteres."
        );

    public ILocator ErroLinkTamanho =>
        page.GetByText(
            "O campo \"Link\" deve conter no máximo 500 caracteres."
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

    public CompromissoFormPage(IPage page, string urlBase)
    {
        this.page = page;
        this.urlBase = urlBase;
    }

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task PreencherAssuntoAsync(string assunto)
    {
        await Assunto.FillAsync(assunto);
    }

    public async Task PreencherDataOcorrenciaAsync(string data)
    {
        await DataOcorrencia.FillAsync(data);
    }

    public async Task PreencherHoraInicioAsync(string hora)
    {
        await HoraInicio.FillAsync(hora);
    }

    public async Task PreencherHoraTerminoAsync(string hora)
    {
        await HoraTermino.FillAsync(hora);
    }
    public async Task SelecionarTipoAsync(string tipo)
    {
        await Tipo.SelectOptionAsync(tipo);
    }

    public async Task SelecionarContatoAsync(Guid contatoId)
    {
        await Contato.SelectOptionAsync(contatoId.ToString());
    }

    public async Task PreencherLocalAsync(string local)
    {
        await Local.FillAsync(local);
    }

    public async Task PreencherLinkAsync(string link)
    {
        await Link.FillAsync(link);
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