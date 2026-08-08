using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using eAgendaWeb.Testes.Integracao.Compartilhado.Orm;
using FizzWare.NBuilder;

namespace eAgendaWeb.Testes.Integracao.Modulos.ModuloCompromisso
{
    [TestClass]
    public sealed class RepositorioCompromissoEmOrmTests : RepositorioBaseTeste
    {
        [TestMethod]
        public void CadastrarESelecionarPorId_CarregaCompromisso_ComTodosOsCampos()
        {
            // Arranjo
            Contato contato = Builder<Contato>
                .CreateNew()
                .With(c => c.Nome = "Tiago")
                .With(c => c.Email = "tiago@email.com")
                .With(c => c.Telefone = "(49) 99999-9999")
                .With(c => c.Cargo = "Desenvolvedor")
                .With(c => c.Empresa = "ADP")
                .Persist();

            Compromisso compromisso = Builder<Compromisso>
                .CreateNew()
                .With(c => c.Assunto = "Reuniao")
                .With(c => c.DataOcorrencia = DateTime.Today)
                .With(c => c.HoraInicio = TimeSpan.FromHours(18))
                .With(c => c.HoraTermino = TimeSpan.FromHours(19))
                .With(c => c.Tipo = TipoCompromisso.Presencial)
                .With(c => c.Local = "Uniplac")
                .With(c => c.Link = null)
                .With(c => c.Contato = contato)
                .Build();

            // Ação
            repositorioCompromisso.Cadastrar(compromisso);
            dbContext.ChangeTracker.Clear();

            Compromisso? compromissoSelecionado =
                repositorioCompromisso.SelecionarPorId(compromisso.Id);

            // Asserção
            Assert.IsNotNull(compromissoSelecionado);

            Assert.AreEqual(
                compromisso.Assunto,
                compromissoSelecionado.Assunto
            );

            Assert.AreEqual(
                compromisso.DataOcorrencia,
                compromissoSelecionado.DataOcorrencia
            );

            Assert.AreEqual(
                compromisso.HoraInicio,
                compromissoSelecionado.HoraInicio
            );

            Assert.AreEqual(
                compromisso.HoraTermino,
                compromissoSelecionado.HoraTermino
            );

            Assert.AreEqual(
                compromisso.Tipo,
                compromissoSelecionado.Tipo
            );

            Assert.AreEqual(
                compromisso.Local,
                compromissoSelecionado.Local
            );

            Assert.AreEqual(
                compromisso.Link,
                compromissoSelecionado.Link
            );

            Assert.IsNotNull(compromissoSelecionado.Contato);

            Assert.AreEqual(
                contato.Id,
                compromissoSelecionado.Contato.Id
            );

            Assert.AreEqual(
                contato.Nome,
                compromissoSelecionado.Contato.Nome
            );
        }

        [TestMethod]
        public void CadastrarESelecionarPorId_CarregaCompromisso_RemotoComLink()
        {
            // Arranjo
            Contato contato = Builder<Contato>
                .CreateNew()
                .With(c => c.Nome = "Tiago")
                .With(c => c.Email = "tiago@email.com")
                .With(c => c.Telefone = "(49) 99999-9999")
                .Persist();

            Compromisso compromisso = Builder<Compromisso>
                .CreateNew()
                .With(c => c.Assunto = "Reuniao Online")
                .With(c => c.DataOcorrencia = DateTime.Today)
                .With(c => c.HoraInicio = TimeSpan.FromHours(18))
                .With(c => c.HoraTermino = TimeSpan.FromHours(19))
                .With(c => c.Tipo = TipoCompromisso.Remoto)
                .With(c => c.Local = null)
                .With(c => c.Link = "https://meet.google.com/reuniao")
                .With(c => c.Contato = contato)
                .Build();

            // Ação
            repositorioCompromisso.Cadastrar(compromisso);
            dbContext.ChangeTracker.Clear();

            Compromisso? compromissoSelecionado =
                repositorioCompromisso.SelecionarPorId(compromisso.Id);

            // Asserção
            Assert.IsNotNull(compromissoSelecionado);

            Assert.AreEqual(
                "Reuniao Online",
                compromissoSelecionado.Assunto
            );

            Assert.AreEqual(
                TipoCompromisso.Remoto,
                compromissoSelecionado.Tipo
            );

            Assert.IsNull(compromissoSelecionado.Local);

            Assert.AreEqual(
                "https://meet.google.com/reuniao",
                compromissoSelecionado.Link
            );

            Assert.IsNotNull(compromissoSelecionado.Contato);

            Assert.AreEqual(
                contato.Id,
                compromissoSelecionado.Contato.Id
            );
        }

        [TestMethod]
        public void CadastrarESelecionarPorId_CarregaCompromisso_SemContato()
        {
            // Arranjo
            Compromisso compromisso = Builder<Compromisso>
                .CreateNew()
                .With(c => c.Assunto = "Reuniao")
                .With(c => c.DataOcorrencia = DateTime.Today)
                .With(c => c.HoraInicio = TimeSpan.FromHours(18))
                .With(c => c.HoraTermino = TimeSpan.FromHours(19))
                .With(c => c.Tipo = TipoCompromisso.Presencial)
                .With(c => c.Local = "Uniplac")
                .With(c => c.Link = null)
                .With(c => c.Contato = null)
                .Build();

            // Ação
            repositorioCompromisso.Cadastrar(compromisso);
            dbContext.ChangeTracker.Clear();

            Compromisso? compromissoSelecionado =
                repositorioCompromisso.SelecionarPorId(compromisso.Id);

            // Asserção
            Assert.IsNotNull(compromissoSelecionado);

            Assert.AreEqual(
                compromisso.Assunto,
                compromissoSelecionado.Assunto
            );

            Assert.AreEqual(
                compromisso.DataOcorrencia,
                compromissoSelecionado.DataOcorrencia
            );

            Assert.AreEqual(
                compromisso.HoraInicio,
                compromissoSelecionado.HoraInicio
            );

            Assert.AreEqual(
                compromisso.HoraTermino,
                compromissoSelecionado.HoraTermino
            );

            Assert.AreEqual(
                compromisso.Tipo,
                compromissoSelecionado.Tipo
            );

            Assert.AreEqual(
                compromisso.Local,
                compromissoSelecionado.Local
            );

            Assert.IsNull(compromissoSelecionado.Link);
            Assert.IsNull(compromissoSelecionado.Contato);
        }

        [TestMethod]
        public void Editar_AtualizaRegistroExistente()
        {
            // Arranjo
            Contato contato = Builder<Contato>
                .CreateNew()
                .Persist();

            Compromisso compromisso = Builder<Compromisso>
                .CreateNew()
                .With(c => c.Contato = contato)
                .Persist();

            Compromisso compromissoAtualizado = Builder<Compromisso>
                .CreateNew()
                .With(c => c.Assunto = "Reuniao Atualizada")
                .With(c => c.DataOcorrencia = DateTime.Today.AddDays(1))
                .With(c => c.HoraInicio = TimeSpan.FromHours(14))
                .With(c => c.HoraTermino = TimeSpan.FromHours(15))
                .With(c => c.Tipo = TipoCompromisso.Presencial)
                .With(c => c.Local = "Empresa")
                .With(c => c.Link = null)
                .With(c => c.Contato = contato)
                .Build();

            // Ação
            bool conseguiuEditar =
                repositorioCompromisso.Editar(
                    compromisso.Id,
                    compromissoAtualizado
                );

            dbContext.ChangeTracker.Clear();

            Compromisso? compromissoSelecionado =
                repositorioCompromisso.SelecionarPorId(compromisso.Id);

            // Asserção
            Assert.IsTrue(conseguiuEditar);
            Assert.IsNotNull(compromissoSelecionado);

            Assert.AreEqual(
                "Reuniao Atualizada",
                compromissoSelecionado.Assunto
            );

            Assert.AreEqual(
                DateTime.Today.AddDays(1),
                compromissoSelecionado.DataOcorrencia
            );

            Assert.AreEqual(
                TimeSpan.FromHours(14),
                compromissoSelecionado.HoraInicio
            );

            Assert.AreEqual(
                TimeSpan.FromHours(15),
                compromissoSelecionado.HoraTermino
            );

            Assert.AreEqual(
                TipoCompromisso.Presencial,
                compromissoSelecionado.Tipo
            );

            Assert.AreEqual(
                "Empresa",
                compromissoSelecionado.Local
            );

            Assert.IsNotNull(compromissoSelecionado.Contato);

            Assert.AreEqual(
                contato.Id,
                compromissoSelecionado.Contato.Id
            );
        }

        [TestMethod]
        public void Excluir_RemoveRegistroExistente()
        {
            // Arranjo
            Contato contato = Builder<Contato>
                .CreateNew()
                .Persist();

            Compromisso compromisso = Builder<Compromisso>
                .CreateNew()
                .With(c => c.Contato = contato)
                .Persist();

            // Ação
            bool conseguiuExcluir =
                repositorioCompromisso.Excluir(compromisso.Id);

            dbContext.ChangeTracker.Clear();

            // Asserção
            Assert.IsTrue(conseguiuExcluir);

            Assert.IsNull(
                repositorioCompromisso.SelecionarPorId(compromisso.Id)
            );
        }

        [TestMethod]
        public void SelecionarTodos_CarregaRegistros()
        {
            // Arranjo
            Contato contato = Builder<Contato>
                .CreateNew()
                .Persist();

            IList<Compromisso> compromissos = Builder<Compromisso>
                .CreateListOfSize(3)
                .All()
                .With(c => c.Contato = contato)
                .Persist();

            dbContext.ChangeTracker.Clear();

            // Ação
            IList<Compromisso> compromissosSelecionados =
                repositorioCompromisso.SelecionarTodos();

            // Asserção
            Assert.HasCount(3, compromissosSelecionados);
        }
    }
}