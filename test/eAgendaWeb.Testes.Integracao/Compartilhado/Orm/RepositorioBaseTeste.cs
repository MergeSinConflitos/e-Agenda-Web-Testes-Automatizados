using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using eAgenda.Dominio.Modulos.ModuloDespesa;
using eAgenda.Dominio.Modulos.ModuloTarefa;
using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Infra.Modulos.ModuloCategoria;
using eAgenda.Infra.Modulos.ModuloCompromisso;
using eAgenda.Infra.Modulos.ModuloContato;
using eAgenda.Infra.Modulos.ModuloDespesa;
using eAgenda.Infra.Modulos.ModuloTarefa;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;

namespace eAgendaWeb.Testes.Integracao.Compartilhado.Orm
{
    public sealed class RepositorioBaseTeste()
    {
        protected EAgendaDbContext dbContext = null!;
        protected RepositorioCategoriaEmOrm repositorioCategoria = null!;
        protected RepositorioCompromissoEmOrm repositorioCompromisso = null!;
        protected RepositorioContatoEmOrm repositorioContato = null!;
        protected RepositorioDespesaEmOrm repositorioDespesa = null!;
        protected RepositorioTarefaEmOrm repositorioTarefa = null!;


        [TestInitialize]
        public void InicializarContexto()
        {

            // Categoria
            repositorioCategoria = new RepositorioCategoriaEmOrm(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Categoria>(repositorioCategoria.Cadastrar);
            BuilderSetup.SetCreatePersistenceMethod<IList<Categoria>>((categorias) =>
            {
                foreach (Categoria c in categorias)
                    repositorioCategoria.Cadastrar(c);
            });

            // Compromisso
            repositorioCompromisso = new RepositorioCompromissoEmOrm(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Compromisso>(repositorioCompromisso.Cadastrar);
            BuilderSetup.SetCreatePersistenceMethod<IList<Compromisso>>((compromissos) =>
            {
                foreach (Compromisso c in compromissos)
                    repositorioCompromisso.Cadastrar(c);
            });

            // Contato
            repositorioContato = new RepositorioContatoEmOrm(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Contato>(repositorioContato.Cadastrar);
            BuilderSetup.SetCreatePersistenceMethod<IList<Contato>>((contatos) =>
            {
                foreach (Contato c in contatos)
                    repositorioContato.Cadastrar(c);
            });

            // Despesa
            repositorioDespesa = new RepositorioDespesaEmOrm(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Despesa>(repositorioDespesa.Cadastrar);
            BuilderSetup.SetCreatePersistenceMethod<IList<Despesa>>((despesas) =>
            {
                foreach (Despesa d in despesas)
                    repositorioDespesa.Cadastrar(d);
            });

            // Tarefa
            repositorioTarefa = new RepositorioTarefaEmOrm(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Tarefa>(repositorioTarefa.Cadastrar);
            BuilderSetup.SetCreatePersistenceMethod<IList<Tarefa>>((tarefas) =>
            {
                foreach (Tarefa t in tarefas)
                    repositorioTarefa.Cadastrar(t);
            });
        }

        /*
        private DbContext CriarDbContext(Guid guid)
        {
               DbContextOptions<DbContext> options =
            new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase($"integracao-{Guid.NewGuid():N}")
                .Options;

        return new DbContext(options, new ProvedorDeUsuarioFake(userId));
        }
        */
    }
}