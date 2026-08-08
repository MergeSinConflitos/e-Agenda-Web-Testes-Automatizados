using System.Text.RegularExpressions;
using eAgendaWeb.Testes.E2E.Modulos.ModuloContato;

namespace GeradorDeProvas.Testes.E2E.Modulos.ModuloContato;

[TestClass]
public sealed class ContatoE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveExibir_ListagemVazia_QuandoNaoExistemContatos()
    {
        // Arrange
        ContatoListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.IrParaAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.Titulo).ToBeVisibleAsync();
        await Expect(listarPage.CadastrarNovo).ToBeVisibleAsync();
        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Contato_ComDadosValidos()
    {
        // Arrange
        ContatoFormPage formPage = new(Page, UrlBase);
        ContatoListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherNomeAsync("Tiago");
        await formPage.PreencherEmailAsync("tiago@email.com");
        await formPage.PreencherTelefoneAsync("(49) 99999-9999");
        await formPage.PreencherCargoAsync("Desenvolvedor");
        await formPage.PreencherEmpresaAsync("ADP");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.NomeDoContato("Tiago")
        ).ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio)
            .Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Contato_ApenasComCamposObrigatorios()
    {
        // Arrange
        ContatoFormPage formPage = new(Page, UrlBase);
        ContatoListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherNomeAsync("Tiago");
        await formPage.PreencherEmailAsync("tiago@email.com");
        await formPage.PreencherTelefoneAsync("(49) 99999-9999");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.NomeDoContato("Tiago")
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Contato_ComDadosValidos()
    {
        // Arrange
        await CadastrarContatoAsync(
            "Tiago",
            "tiago@email.com",
            "(49) 99999-9999",
            "Desenvolvedor",
            "ADP"
        );

        ContatoListarPage listarPage = new(Page, UrlBase);
        ContatoFormPage formPage = new(Page, UrlBase);

        await listarPage.EditarAsync("Tiago");

        // Act
        await formPage.PreencherNomeAsync("Tiago Atualizado");
        await formPage.PreencherEmailAsync("tiago.atualizado@email.com");
        await formPage.PreencherTelefoneAsync("(49) 98888-8888");
        await formPage.PreencherCargoAsync("Analista");
        await formPage.PreencherEmpresaAsync("Empresa B");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.NomeDoContato("Tiago Atualizado")
        ).ToBeVisibleAsync();

        await Expect(
            listarPage.NomeDoContato("Tiago")
        ).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Contato()
    {
        // Arrange
        await CadastrarContatoAsync(
            "Tiago",
            "tiago@email.com",
            "(49) 99999-9999",
            "Desenvolvedor",
            "ADP"
        );

        ContatoListarPage listarPage = new(Page, UrlBase);
        ContatoExcluirPage excluirPage = new(Page);

        await listarPage.ExcluirAsync("Tiago");

        // Act
        await Expect(Page).ToHaveURLAsync(
            new Regex($"{Regex.Escape(UrlBase)}/Contato/Excluir/.*")
        );

        await Expect(
            excluirPage.MensagemConfirmacao
        ).ToBeVisibleAsync();

        await excluirPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.NomeDoContato("Tiago")
        ).Not.ToBeVisibleAsync();

        await Expect(
            listarPage.EstadoVazio
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExibir_ErrosAoCadastrar_ContatoComDadosInvalidos()
    {
        // Arrange
        ContatoFormPage formPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherNomeAsync("");
        await formPage.PreencherEmailAsync("");
        await formPage.PreencherTelefoneAsync("");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroNomeObrigatorio
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroEmailObrigatorio
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroTelefoneObrigatorio
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExibir_ErrosAoCadastrar_ContatoComFormatosInvalidos()
    {
        // Arrange
        ContatoFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        // Desabilita somente a validação HTML5 do navegador
        await formPage.DesabilitarValidacaoNativaAsync();

        // Act
        await formPage.PreencherNomeAsync("A");
        await formPage.PreencherEmailAsync("email-invalido");
        await formPage.PreencherTelefoneAsync("123456");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroNomeTamanho
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroEmailFormato
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroTelefoneFormato
        ).ToBeVisibleAsync();
    }
    [TestMethod]
    public async Task NaoDeveCadastrar_ContatoComEmailDuplicado()
    {
        // Arrange
        await CadastrarContatoAsync(
            "Tiago",
            "tiago@email.com",
            "(49) 99999-9999",
            null,
            null
        );

        ContatoListarPage listarPage = new(Page, UrlBase);
        ContatoFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        // Act
        await formPage.PreencherNomeAsync("João");
        await formPage.PreencherEmailAsync("tiago@email.com");
        await formPage.PreencherTelefoneAsync("(49) 98888-8888");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(formPage.ErroEmailDuplicado)
            .ToContainTextAsync("Já existe");
    }
    [TestMethod]
    public async Task NaoDeveCadastrar_ContatoComTelefoneDuplicado()
    {
        // Arrange
        await CadastrarContatoAsync(
            "Tiago",
            "tiago@email.com",
            "(49) 99999-9999",
            null,
            null
        );

        ContatoListarPage listarPage = new(Page, UrlBase);
        ContatoFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        // Act
        await formPage.PreencherNomeAsync("João");
        await formPage.PreencherEmailAsync("joao@email.com");
        await formPage.PreencherTelefoneAsync("(49) 99999-9999");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(formPage.ErroTelefoneDuplicado)
            .ToContainTextAsync("Já existe");
    }


    private async Task CadastrarContatoAsync(
         string nome,
         string email,
         string telefone,
         string? cargo,
         string? empresa)
    {
        ContatoFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherNomeAsync(nome);
        await formPage.PreencherEmailAsync(email);
        await formPage.PreencherTelefoneAsync(telefone);

        if (cargo is not null)
            await formPage.PreencherCargoAsync(cargo);

        if (empresa is not null)
            await formPage.PreencherEmpresaAsync(empresa);

        await formPage.ConfirmarAsync();

        ContatoListarPage listarPage = new(Page, UrlBase);

        await Expect(Page).ToHaveURLAsync(listarPage.Url);
    }
}