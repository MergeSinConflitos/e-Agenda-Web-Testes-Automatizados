using System.Text.RegularExpressions;
using eAgendaWeb.Testes.E2E.Modulos.ModuloCompromisso;

namespace GeradorDeProvas.Testes.E2E.Modulos.ModuloCompromisso;

[TestClass]
public sealed class CompromissoE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveExibir_ListagemVazia_QuandoNaoExistemCompromissos()
    {
        // Arrange
        CompromissoListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.IrParaAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(listarPage.Titulo)
            .ToBeVisibleAsync();

        await Expect(listarPage.CadastrarNovo)
            .ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio)
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Compromisso_Presencial_ComDadosValidos()
    {
        // Arrange
        CompromissoFormPage formPage = new(Page, UrlBase);
        CompromissoListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAssuntoAsync("Reunião presencial");
        await formPage.PreencherDataOcorrenciaAsync("2026-08-10");
        await formPage.PreencherHoraInicioAsync("10:00");
        await formPage.PreencherHoraTerminoAsync("11:00");
        await formPage.SelecionarTipoAsync("Presencial");
        await formPage.PreencherLocalAsync("Escritório");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.AssuntoDoCompromisso("Reunião presencial")
        ).ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio)
            .Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Compromisso_Remoto_ComDadosValidos()
    {
        // Arrange
        CompromissoFormPage formPage = new(Page, UrlBase);
        CompromissoListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAssuntoAsync("Reunião remota");
        await formPage.PreencherDataOcorrenciaAsync("2026-08-10");
        await formPage.PreencherHoraInicioAsync("14:00");
        await formPage.PreencherHoraTerminoAsync("15:00");
        await formPage.SelecionarTipoAsync("Remoto");
        await formPage.PreencherLinkAsync("https://meet.google.com");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.AssuntoDoCompromisso("Reunião remota")
        ).ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio)
            .Not.ToBeVisibleAsync();
    }
    [TestMethod]
    public async Task DeveCadastrar_Compromisso_ApenasComCamposObrigatorios()
    {
        // Arrange
        CompromissoFormPage formPage = new(Page, UrlBase);
        CompromissoListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAssuntoAsync("Reunião");
        await formPage.PreencherDataOcorrenciaAsync("2026-08-10");
        await formPage.PreencherHoraInicioAsync("10:00");
        await formPage.PreencherHoraTerminoAsync("11:00");
        await formPage.SelecionarTipoAsync("Presencial");
        await formPage.PreencherLocalAsync("Escritório");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page)
            .ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.AssuntoDoCompromisso("Reunião")
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExibir_ErrosAoCadastrar_CompromissoComCamposObrigatoriosEmBranco()
    {
        // Arrange
        CompromissoFormPage formPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAssuntoAsync("");
        await formPage.PreencherDataOcorrenciaAsync("");
        await formPage.PreencherHoraInicioAsync("");
        await formPage.PreencherHoraTerminoAsync("");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroAssuntoObrigatorio
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroDataOcorrenciaObrigatoria
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroHoraInicioObrigatoria
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroHoraTerminoObrigatoria
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExibir_ErrosAoCadastrar_CompromissoComHoraInicioMaiorQueHoraTermino()
    {
        //Arrange
        CompromissoFormPage formPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAssuntoAsync("Reuniao");
        await formPage.PreencherDataOcorrenciaAsync("2026-08-10");
        await formPage.PreencherHoraInicioAsync("10:00");
        await formPage.PreencherHoraTerminoAsync("09:00");
        await formPage.SelecionarTipoAsync("Presencial");
        await formPage.PreencherLocalAsync("Uniplac");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroHoraTerminoMenorQueHoraInicio
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task NaoDeveCadastrar_Compromisso_ComHorarioConflitante()
    {
        // Arrange
        await CadastrarCompromissoAsync(
            "Reunião existente",
            "2026-08-10",
            "14:00",
            "15:00",
            "Presencial",
            "Escritório",
            null
        );

        CompromissoFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAssuntoAsync("Nova reunião");
        await formPage.PreencherDataOcorrenciaAsync("2026-08-10");
        await formPage.PreencherHoraInicioAsync("14:30");
        await formPage.PreencherHoraTerminoAsync("16:00");
        await formPage.SelecionarTipoAsync("Presencial");
        await formPage.PreencherLocalAsync("Sala de Reuniões");

        // Act
        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroConflitoDeHorario
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task NaoDeveCadastrar_CompromissoPresencial_SemLocal()
    {
        // Arrange
        CompromissoFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAssuntoAsync("Reunião presencial");
        await formPage.PreencherDataOcorrenciaAsync("2026-08-10");
        await formPage.PreencherHoraInicioAsync("10:00");
        await formPage.PreencherHoraTerminoAsync("11:00");
        await formPage.SelecionarTipoAsync("Presencial");

        // Não informa o Local

        // Act
        await formPage.ConfirmarAsync();

        // Assert
        await Expect(formPage.ErroLocalObrigatorio)
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task NaoDeveCadastrar_CompromissoRemoto_SemLink()
    {
        // Arrange
        CompromissoFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAssuntoAsync("Reunião remota");
        await formPage.PreencherDataOcorrenciaAsync("2026-08-10");
        await formPage.PreencherHoraInicioAsync("14:00");
        await formPage.PreencherHoraTerminoAsync("15:00");
        await formPage.SelecionarTipoAsync("Remoto");

        // Não informa o Link

        // Act
        await formPage.ConfirmarAsync();

        // Assert
        await Expect(formPage.ErroLinkObrigatorio)
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Compromisso_ComDadosValidos()
    {
        // Arrange
        await CadastrarCompromissoAsync(
            "Reunião com cliente",
            "2026-08-11",
            "10:00",
            "11:00",
            "Presencial",
            "Escritório",
            "https://google.com"
        );

        CompromissoListarPage listarPage = new(Page, UrlBase);
        CompromissoFormPage formPage = new(Page, UrlBase);

        await listarPage.EditarAsync("Reunião com cliente");

        // Act
        await formPage.PreencherAssuntoAsync("Reunião atualizada");
        await formPage.PreencherDataOcorrenciaAsync("2026-08-11");
        await formPage.PreencherHoraInicioAsync("14:00");
        await formPage.PreencherHoraTerminoAsync("15:00");
        await formPage.SelecionarTipoAsync("Remoto");
        await formPage.PreencherLocalAsync("Home Office");
        await formPage.PreencherLinkAsync("https://meet.google.com");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page)
            .ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.AssuntoDoCompromisso("Reunião atualizada")
        ).ToBeVisibleAsync();

        await Expect(
            listarPage.AssuntoDoCompromisso("Reunião com cliente")
        ).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Compromisso()
    {
        // Arrange
        await CadastrarCompromissoAsync(
            "Reunião com cliente",
            "2026-08-10",
            "10:00",
            "11:00",
            "Presencial",
            "Escritório",
            "https://google.com"
        );

        CompromissoListarPage listarPage = new(Page, UrlBase);
        CompromissoExcluirPage excluirPage = new(Page, UrlBase);

        await listarPage.ExcluirAsync("Reunião com cliente");

        // Act
        await Expect(Page).ToHaveURLAsync(
            new Regex(
                $"{Regex.Escape(UrlBase)}/Compromisso/Excluir/.*"
            )
        );

        await Expect(
            excluirPage.MensagemConfirmacao
        ).ToBeVisibleAsync();

        await excluirPage.ConfirmarAsync();

        // Assert
        await Expect(Page)
            .ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.AssuntoDoCompromisso("Reunião com cliente")
        ).Not.ToBeVisibleAsync();

        await Expect(
            listarPage.EstadoVazio
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExibir_ErrosAoCadastrar_CompromissoComDadosInvalidos()
    {
        // Arrange
        CompromissoFormPage formPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAssuntoAsync("");
        await formPage.PreencherDataOcorrenciaAsync("");
        await formPage.PreencherHoraInicioAsync("");
        await formPage.PreencherHoraTerminoAsync("");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroAssuntoObrigatorio
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroDataOcorrenciaObrigatoria
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroHoraInicioObrigatoria
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroHoraTerminoObrigatoria
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExibir_ErrosAoCadastrar_CompromissoComDadosForaDosLimites()
    {
        // Arrange
        CompromissoFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.DesabilitarValidacaoNativaAsync();

        // Act
        await formPage.PreencherAssuntoAsync("A");
        await formPage.PreencherLocalAsync(new string('A', 256));
        await formPage.PreencherLinkAsync(new string('A', 501));

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroAssuntoTamanho
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroLocalTamanho
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroLinkTamanho
        ).ToBeVisibleAsync();
    }

    private async Task CadastrarCompromissoAsync(
        string assunto,
        string dataOcorrencia,
        string horaInicio,
        string horaTermino,
        string tipo,
        string? local,
        string? link)
    {
        CompromissoFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAssuntoAsync(assunto);
        await formPage.PreencherDataOcorrenciaAsync(dataOcorrencia);
        await formPage.PreencherHoraInicioAsync(horaInicio);
        await formPage.PreencherHoraTerminoAsync(horaTermino);
        await formPage.SelecionarTipoAsync(tipo);

        if (local is not null)
            await formPage.PreencherLocalAsync(local);

        if (link is not null)
            await formPage.PreencherLinkAsync(link);

        await formPage.ConfirmarAsync();

        CompromissoListarPage listarPage = new(Page, UrlBase);

        await Expect(Page)
            .ToHaveURLAsync(listarPage.Url);
    }
}
