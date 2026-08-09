using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgendaWeb.Testes.Integracao.Compartilhado.Orm;
using FizzWare.NBuilder;

namespace eAgendaWeb.Testes.Integracao.Modulos.ModuloCategoria
{
    [TestClass]
    public sealed class RepositorioCategoriaEmOrmTests : RepositorioBaseTeste
    {
        [TestMethod]
        public void CadastrarESelecionarPorId_CarregaCategoria_ComTodosOsCampos()
        {
            // Arranjo
            Categoria categoria = Builder<Categoria>
                .CreateNew()
                .With(c => c.Titulo = "Limpeza")
                .Build();

            // Ação
            repositorioCategoria.Cadastrar(categoria);
            dbContext.ChangeTracker.Clear();

            Categoria? categoriaSelecionada =
                repositorioCategoria.SelecionarPorId(categoria.Id);

            // Asserção
            Assert.IsNotNull(categoriaSelecionada);

            Assert.AreEqual(
                categoria.Titulo,
                categoriaSelecionada.Titulo
            );
        }

        [TestMethod]
        public void Editar_AtualizaRegistroExistente()
        {
            // Arranjo
            Categoria categoria = Builder<Categoria>
                .CreateNew()
                .With(c => c.Titulo = "Limpeza")
                .Persist();

            Categoria categoriaAtualizada = Builder<Categoria>
                .CreateNew()
                .With(c => c.Titulo = "Limpeza Atualizada")
                .Build();

            // Ação
            bool conseguiuEditar =
                repositorioCategoria.Editar(
                    categoria.Id,
                    categoriaAtualizada
                );

            dbContext.ChangeTracker.Clear();

            Categoria? categoriaSelecionada =
                repositorioCategoria.SelecionarPorId(categoria.Id);

            // Asserção
            Assert.IsTrue(conseguiuEditar);
            Assert.IsNotNull(categoriaSelecionada);

            Assert.AreEqual(
                "Limpeza Atualizada",
                categoriaSelecionada.Titulo
            );
        }

        [TestMethod]
        public void Excluir_RemoveRegistroExistente()
        {
            // Arranjo
            Categoria categoria = Builder<Categoria>
                .CreateNew()
                .With(c => c.Titulo = "Limpeza")
                .Persist();

            // Ação
            bool conseguiuExcluir =
                repositorioCategoria.Excluir(categoria.Id);

            dbContext.ChangeTracker.Clear();

            // Asserção
            Assert.IsTrue(conseguiuExcluir);

            Assert.IsNull(
                repositorioCategoria.SelecionarPorId(categoria.Id)
            );
        }

        [TestMethod]
        public void SelecionarTodos_CarregaRegistros()
        {
            // Arranjo
            IList<Categoria> categorias = Builder<Categoria>
                .CreateListOfSize(3)
                .All()
                .Persist();

            dbContext.ChangeTracker.Clear();

            // Ação
            IList<Categoria> categoriasSelecionadas =
                repositorioCategoria.SelecionarTodos();

            // Asserção
            Assert.HasCount(3, categoriasSelecionadas);
        }
    }
}