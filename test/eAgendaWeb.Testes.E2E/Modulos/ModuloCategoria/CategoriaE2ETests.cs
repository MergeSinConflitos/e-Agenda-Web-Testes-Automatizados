using System.Text.RegularExpressions;
using eAgendaWeb.Testes.E2E.Modulos.ModuloCategoria;

namespace GeradorDeProvas.Testes.E2E.Modulos.ModuloCategoria;

[TestClass]
public sealed class CategoriaE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveExibir_ListagemVazia_QuandoNaoExistemCategorias()
    {
        // Arrange
        CategoriaListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.IrParaAsync();

        // Assert
        await Expect(Page)
            .ToHaveURLAsync(listarPage.Url);

        await Expect(listarPage.Titulo)
            .ToBeVisibleAsync();

        await Expect(listarPage.CadastrarNovo)
            .ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio)
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Categoria_ComDadosValidos()
    {
        // Arrange
        CategoriaFormPage formPage = new(Page, UrlBase);
        CategoriaListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherTituloAsync("Limpeza");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page)
            .ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.TituloDaCategoria("Limpeza")
        ).ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio)
            .Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Categoria_ComTituloNoLimiteMinimo()
    {
        // Arrange
        CategoriaFormPage formPage = new(Page, UrlBase);
        CategoriaListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherTituloAsync("AB");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page)
            .ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.TituloDaCategoria("AB")
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Categoria_ComTituloNoLimiteMaximo()
    {
        // Arrange
        CategoriaFormPage formPage = new(Page, UrlBase);
        CategoriaListarPage listarPage = new(Page, UrlBase);

        string titulo = new string('A', 100);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherTituloAsync(titulo);

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page)
            .ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.TituloDaCategoria(titulo)
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExibir_ErroAoCadastrar_CategoriaComTituloEmBranco()
    {
        // Arrange
        CategoriaFormPage formPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherTituloAsync("");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroTituloObrigatorio
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExibir_ErroAoCadastrar_CategoriaComTituloCurto()
    {
        // Arrange
        CategoriaFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        // Act
        await formPage.PreencherTituloAsync("A");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroTituloTamanho
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExibir_ErroAoCadastrar_CategoriaComTituloLongo()
    {
        // Arrange
        CategoriaFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        // Act
        await formPage.PreencherTituloAsync(
            new string('A', 101)
        );

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroTituloTamanho
        ).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task NaoDeveCadastrar_CategoriaComTituloDuplicado()
    {
        // Arrange
        await CadastrarCategoriaAsync("Limpeza");

        CategoriaFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        // Act
        await formPage.PreencherTituloAsync("Limpeza");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroTituloDuplicado
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroTituloDuplicado
        ).ToContainTextAsync("Já existe");
    }

    [TestMethod]
    public async Task DeveEditar_Categoria_ComDadosValidos()
    {
        // Arrange
        await CadastrarCategoriaAsync("Limpeza");

        CategoriaListarPage listarPage = new(Page, UrlBase);
        CategoriaFormPage formPage = new(Page, UrlBase);

        await listarPage.EditarAsync("Limpeza");

        // Act
        await formPage.PreencherTituloAsync("Alimentação");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page)
            .ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.TituloDaCategoria("Alimentação")
        ).ToBeVisibleAsync();

        await Expect(
            listarPage.TituloDaCategoria("Limpeza")
        ).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task NaoDeveEditar_CategoriaComTituloDuplicado()
    {
        // Arrange
        await CadastrarCategoriaAsync("Limpeza");
        await CadastrarCategoriaAsync("Alimentação");

        CategoriaListarPage listarPage = new(Page, UrlBase);
        CategoriaFormPage formPage = new(Page, UrlBase);

        await listarPage.EditarAsync("Limpeza");

        // Act
        await formPage.PreencherTituloAsync("Alimentação");

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(
            formPage.ErroTituloDuplicado
        ).ToBeVisibleAsync();

        await Expect(
            formPage.ErroTituloDuplicado
        ).ToContainTextAsync("Já existe");
    }

    [TestMethod]
    public async Task DeveExcluir_Categoria()
    {
        // Arrange
        await CadastrarCategoriaAsync("Limpeza");

        CategoriaListarPage listarPage = new(Page, UrlBase);
        CategoriaExcluirPage excluirPage = new(Page, UrlBase);

        await listarPage.ExcluirAsync("Limpeza");

        // Act
        await Expect(Page).ToHaveURLAsync(
            new Regex(
                $"{Regex.Escape(UrlBase)}/Categoria/Excluir/.*"
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
            listarPage.TituloDaCategoria("Limpeza")
        ).Not.ToBeVisibleAsync();

        await Expect(
            listarPage.EstadoVazio
        ).ToBeVisibleAsync();
    }

    private async Task CadastrarCategoriaAsync(string titulo)
    {
        CategoriaFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherTituloAsync(titulo);

        await formPage.ConfirmarAsync();

        CategoriaListarPage listarPage = new(Page, UrlBase);

        await Expect(Page)
            .ToHaveURLAsync(listarPage.Url);
    }
}