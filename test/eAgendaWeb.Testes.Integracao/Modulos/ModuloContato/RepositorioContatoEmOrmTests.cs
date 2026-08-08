using eAgenda.Dominio.Modulos.ModuloContato;
using eAgendaWeb.Testes.Integracao.Compartilhado.Orm;
using FizzWare.NBuilder;

namespace eAgendaWeb.Testes.Integracao.Modulos.ModuloContato
{
    [TestClass]
    public sealed class RepositorioContatoEmOrmTests : RepositorioBaseTeste
    {
        [TestMethod]
        public void CadastrarESelecionarPorId_CarregaContato_ComTodosOsCampos()
        {
            // Arranjo
            Contato contato = Builder<Contato>
                .CreateNew()
                .With(c => c.Nome = "Tiago")
                .With(c => c.Email = "tiago@email.com")
                .With(c => c.Telefone = "(49) 99999-9999")
                .With(c => c.Cargo = "Desenvolvedor")
                .With(c => c.Empresa = "ADP")
                .Build();

            // Ação
            repositorioContato.Cadastrar(contato);
            dbContext.ChangeTracker.Clear();

            Contato? contatoSelecionado =
                repositorioContato.SelecionarPorId(contato.Id);

            // Asserção
            Assert.IsNotNull(contatoSelecionado);
            Assert.AreEqual(contato.Nome, contatoSelecionado.Nome);
            Assert.AreEqual(contato.Email, contatoSelecionado.Email);
            Assert.AreEqual(contato.Telefone, contatoSelecionado.Telefone);
            Assert.AreEqual(contato.Cargo, contatoSelecionado.Cargo);
            Assert.AreEqual(contato.Empresa, contatoSelecionado.Empresa);
        }

        [TestMethod]
        public void CadastrarESelecionarPorId_CarregaContato_ComTodosCamposObrigatorios()
        {
            // Arranjo
            Contato contato = Builder<Contato>
                .CreateNew()
                .With(c => c.Nome = "Tiago")
                .With(c => c.Email = "tiago@email.com")
                .With(c => c.Telefone = "(49) 99999-9999")
                .With(c => c.Cargo = null)
                .With(c => c.Empresa = null)
                .Build();

            // Ação
            repositorioContato.Cadastrar(contato);
            dbContext.ChangeTracker.Clear();

            Contato? contatoSelecionado =
                repositorioContato.SelecionarPorId(contato.Id);

            // Asserção
            Assert.IsNotNull(contatoSelecionado);
            Assert.AreEqual(contato.Nome, contatoSelecionado.Nome);
            Assert.AreEqual(contato.Email, contatoSelecionado.Email);
            Assert.AreEqual(contato.Telefone, contatoSelecionado.Telefone);
            Assert.IsNull(contatoSelecionado.Cargo);
            Assert.IsNull(contatoSelecionado.Empresa);
        }

        [TestMethod]
        public void Editar_AtualizaRegistroExistente()
        {
            // Arranjo
            Contato contato = Builder<Contato>
                .CreateNew()
                .Persist();

            Contato contatoAtualizado = Builder<Contato>
                .CreateNew()
                .With(c => c.Nome = "NomeAtualizado")
                .Build();

            // Ação
            bool conseguiuEditar =
                repositorioContato.Editar(
                    contato.Id,
                    contatoAtualizado
                );

            dbContext.ChangeTracker.Clear();

            Contato? contatoSelecionado =
                repositorioContato.SelecionarPorId(contato.Id);

            // Asserção
            Assert.IsTrue(conseguiuEditar);
            Assert.IsNotNull(contatoSelecionado);
            Assert.AreEqual(
                "NomeAtualizado",
                contatoSelecionado.Nome
            );
        }

        [TestMethod]
        public void Excluir_RemoveRegistroExistente()
        {
            // Arranjo
            Contato contato = Builder<Contato>
                .CreateNew()
                .Persist();

            // Ação
            bool conseguiuExcluir =
                repositorioContato.Excluir(contato.Id);

            dbContext.ChangeTracker.Clear();

            // Asserção
            Assert.IsTrue(conseguiuExcluir);
            Assert.IsNull(
                repositorioContato.SelecionarPorId(contato.Id)
            );
        }

        [TestMethod]
        public void SelecionarTodos_CarregaRegistros()
        {
            // Arranjo
            IList<Contato> contatos = Builder<Contato>
                .CreateListOfSize(3)
                .All()
                .Persist();

            dbContext.ChangeTracker.Clear();

            // Ação
            IList<Contato> contatosSelecionados =
                repositorioContato.SelecionarTodos();

            // Asserção
            Assert.HasCount(3, contatosSelecionados);
        }
    }
}