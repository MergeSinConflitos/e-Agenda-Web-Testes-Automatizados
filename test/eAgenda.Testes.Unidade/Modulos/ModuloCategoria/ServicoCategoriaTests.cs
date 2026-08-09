using eAgenda.Aplicacao.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloDespesa;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCategoria;

[TestClass]
public sealed class ServicoCategoriaTests()
{
    [TestMethod]
    public void Cadastrar_ComDadosValidos_PersisteCategoria()
    {
        //Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new Mock<IRepositorioCategoria>();
        Mock<IRepositorioDespesa> repositorioDespesa = new Mock<IRepositorioDespesa>();

        repositorioCategoria.Setup(r => r.SelecionarTodos()).Returns([]);

        Categoria? categoriaCadastrada = null;

        repositorioCategoria.Setup(r => r.Cadastrar(It.IsAny<Categoria>()))
        .Callback<Categoria>(categoria => categoriaCadastrada = categoria);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        //Ação
        Result resultado = servicoCategoria.Cadastrar(new CadastrarCategoriaDto("Limpeza"));

        //Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(categoriaCadastrada);
        Assert.AreEqual("Limpeza", categoriaCadastrada.Titulo);

        repositorioCategoria.Verify(r => r.Cadastrar(It.IsAny<Categoria>()), Times.Once);

    }

    [TestMethod]
    public void Cadastrar_ComTituloEmBranco_NaoPersisteCategoria()
    {
        //Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new Mock<IRepositorioCategoria>();
        Mock<IRepositorioDespesa> repositorioDespesa = new Mock<IRepositorioDespesa>();

        repositorioCategoria.Setup(r => r.SelecionarTodos()).Returns([]);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        //Ação
        Result resultado = servicoCategoria.Cadastrar(new CadastrarCategoriaDto(""));

        //Asserção
        Assert.IsTrue(resultado.IsFailed);

        repositorioCategoria.Verify(r => r.Cadastrar(It.IsAny<Categoria>()), Times.Never);

    }

    [TestMethod]
    public void Cadastrar_ComDadosTituloDuplicado_NaoPersisteCategoria()
    {
        //Arranjo
        Categoria? categoriaExistente = new Categoria("Alimentação");

        Mock<IRepositorioCategoria> repositorioCategoria = new Mock<IRepositorioCategoria>();
        Mock<IRepositorioDespesa> repositorioDespesa = new Mock<IRepositorioDespesa>();

        repositorioCategoria.Setup(r => r.SelecionarTodos()).Returns([categoriaExistente]);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        //Ação
        Result resultado = servicoCategoria.Cadastrar(new CadastrarCategoriaDto("Alimentação"));

        //Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Titulo", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);

        repositorioCategoria.Verify(r => r.Cadastrar(It.IsAny<Categoria>()), Times.Never);
    }

    [TestMethod]
    public void Editar_ComDadosValidos_PersisteCategoria()
    {
        // Arranjo
        Categoria categoriaExistente = new Categoria("Limpeza");

        Categoria? categoriaAtualizada = null;

        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria
            .Setup(r => r.SelecionarTodos())
            .Returns([categoriaExistente]);

        repositorioCategoria
            .Setup(r => r.Editar(
                categoriaExistente.Id,
                It.IsAny<Categoria>()
            ))
            .Callback<Guid, Categoria>((id, contato) =>
            {
                categoriaAtualizada = contato;
            })
            .Returns(true);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoCategoria.Editar(
            new EditarCategoriaDto(
                categoriaExistente.Id,
                "Alimentação"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(categoriaAtualizada);

        Assert.AreEqual("Alimentação", categoriaAtualizada.Titulo);

        repositorioCategoria.Verify(
            r => r.Editar(
                categoriaExistente.Id,
                It.IsAny<Categoria>()
            ),
            Times.Once
        );
    }

    [TestMethod]
    public void Editar_ComTituloExistente_NaoPersisteCategoria()
    {
        // Arranjo
        Categoria categoriaExistente = new Categoria("Limpeza");

        Categoria? outraCategoria = new Categoria("Alimentação");

        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria
            .Setup(r => r.SelecionarTodos())
            .Returns([categoriaExistente, outraCategoria]);


        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoCategoria.Editar(
            new EditarCategoriaDto(
                categoriaExistente.Id,
                "Alimentação"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(
                "Titulo",
                resultado.Errors.Single().Metadata["Campo"]
            );

        Assert.Contains(
            "Já existe",
            resultado.Errors.Single().Message
        );

        repositorioCategoria.Verify(
            r => r.Editar(
                categoriaExistente.Id,
                It.IsAny<Categoria>()
            ),
            Times.Never
        );
    }

    [TestMethod]
    public void Excluir_SemDespesasVinculadas_ExcluiCategoria()
    {
        // Arranjo
        Categoria categoria = new Categoria(
            "Limpeza"
        );

        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria
            .Setup(r => r.SelecionarPorId(categoria.Id))
            .Returns(categoria);

        repositorioDespesa
            .Setup(r => r.SelecionarTodos())
            .Returns([]);

        repositorioCategoria
            .Setup(r => r.Excluir(categoria.Id))
            .Returns(true);

        ServicoCategoria servicoContato = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoContato.Excluir(categoria.Id);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);

        repositorioCategoria.Verify(
            r => r.Excluir(categoria.Id),
            Times.Once
        );
    }

    [TestMethod]
    public void Excluir_ComDespesasVinculadas_NaoExcluiCategoria()
    {
        // Arranjo
        Categoria categoria = new Categoria("Limpeza");

        Despesa despesa = new Despesa();

        despesa.Categorias.Add(categoria);

        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria
            .Setup(r => r.SelecionarPorId(categoria.Id))
            .Returns(categoria);

        repositorioDespesa
            .Setup(r => r.SelecionarTodos())
            .Returns([despesa]);

        ServicoCategoria servicoCategoria = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoCategoria.Excluir(categoria.Id);

        // Asserção
        Assert.IsFalse(resultado.IsSuccess);

        Assert.AreEqual(
            "Não é possível excluir esta categoria, pois ela possui despesas vinculadas.",
            resultado.Errors.First().Message
        );

        repositorioCategoria.Verify(
            r => r.Excluir(categoria.Id),
            Times.Never
        );
    }

}