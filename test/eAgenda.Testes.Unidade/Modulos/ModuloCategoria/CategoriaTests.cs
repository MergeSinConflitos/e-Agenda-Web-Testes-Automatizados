using eAgenda.Dominio.Modulos.ModuloCategoria;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCategoria;

[TestClass]
public sealed class CategoriaTests()
{
    [TestMethod]
    public void Validar_ComTituloVazio_DeveRetornarErro()
    {
        //Arranjo
        Categoria categoria = new Categoria("");

        //Ação
        List<string> erros = categoria.Validar();

        //Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Título\" deve conter entre 2 e 100 caracteres.", erros.First());

    }

    [TestMethod]
    public void Validar_ComTituloCurto_DeveRetornarErro()
    {
        //Arranjo
        Categoria categoria = new Categoria(new string('A', 1));

        //Ação
        List<string> erros = categoria.Validar();

        //Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Título\" deve conter entre 2 e 100 caracteres.", erros.First());

    }

    [TestMethod]
    public void Validar_ComTituloLongo_DeveRetornarErro()
    {
        //Arranjo
        Categoria categoria = new Categoria(new string('A', 101));

        //Ação
        List<string> erros = categoria.Validar();

        //Asserção
        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Título\" deve conter entre 2 e 100 caracteres.", erros.First());

    }

    [TestMethod]
    public void Validar_ComTituloNoLimiteMinimo_NaoDeveRetornarErro()
    {
        //Arranjo
        Categoria categoria = new Categoria(new string('A', 2));

        //Ação
        List<string> erros = categoria.Validar();

        //Asserção
        Assert.IsEmpty(erros);

    }

    [TestMethod]
    public void Validar_ComTituloNoLimiteMaximo_NaoDeveRetornarErro()
    {
        //Arranjo
        Categoria categoria = new Categoria(new string('A', 100));

        //Ação
        List<string> erros = categoria.Validar();

        //Asserção
        Assert.IsEmpty(erros);

    }

    [TestMethod]
    public void Validar_ComDadosValidos_NaoDeveRetornarErro()
    {
        //Arranjo
        Categoria categoria = new Categoria("Limpeza");

        //Ação
        List<string> erros = categoria.Validar();

        //Asserção
        Assert.IsEmpty(erros);

    }

    [TestMethod]
    public void Atualizar_ComDadosValidos_DeveAtualizarTodosOsDados()
    {
        //Arranjo
        Categoria categoria = new Categoria("Limpeza");

        Categoria categoriaAtualizada = new Categoria("Alimento");

        //Ação
        categoria.Atualizar(categoriaAtualizada);

        //Asserção
        Assert.AreEqual("Alimento", categoriaAtualizada.Titulo);
    }
}