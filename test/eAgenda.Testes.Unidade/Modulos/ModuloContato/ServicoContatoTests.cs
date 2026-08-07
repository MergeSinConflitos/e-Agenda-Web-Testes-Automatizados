using eAgenda.Aplicacao.Modulos.ModuloContato;
using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloContato
{
    [TestClass]
    public sealed class ServicoContatoTests()
    {
        [TestMethod]
        public void Cadastrar_ComDadosValidos_PersisteContato()
        {
            // Arranjo
            Mock<IRepositorioContato> repositorioContato = new();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();

            repositorioContato
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            Contato? contatoCadastrado = null;

            repositorioContato
                .Setup(r => r.Cadastrar(It.IsAny<Contato>()))
                .Callback<Contato>(contato =>
                    contatoCadastrado = contato
                );

            ServicoContato servicoContato = new(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            // Ação
            Result resultado = servicoContato.Cadastrar(
                new CadastrarContatoDto(
                    "Tiago",
                    "ti123@gmail.com",
                    "(49) 99999-9999",
                    "Desenvolvedor",
                    "ADP"
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(contatoCadastrado);

            Assert.AreEqual("Tiago", contatoCadastrado.Nome);
            Assert.AreEqual("ti123@gmail.com", contatoCadastrado.Email);
            Assert.AreEqual("(49) 99999-9999", contatoCadastrado.Telefone);
            Assert.AreEqual("Desenvolvedor", contatoCadastrado.Cargo);
            Assert.AreEqual("ADP", contatoCadastrado.Empresa);

            repositorioContato.Verify(
                r => r.Cadastrar(It.IsAny<Contato>()),
                Times.Once
            );
        }

        [TestMethod]
        public void Cadastrar_ApenasCamposObrigatorios_PersisteContato()
        {
            // Arranjo
            Mock<IRepositorioContato> repositorioContato = new();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();

            repositorioContato
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            Contato? contatoCadastrado = null;

            repositorioContato
                .Setup(r => r.Cadastrar(It.IsAny<Contato>()))
                .Callback<Contato>(contato =>
                    contatoCadastrado = contato
                );

            ServicoContato servicoContato = new(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            // Ação
            Result resultado = servicoContato.Cadastrar(
                new CadastrarContatoDto(
                    "Tiago",
                    "ti123@gmail.com",
                    "(49) 99999-9999",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(contatoCadastrado);

            Assert.AreEqual("Tiago", contatoCadastrado.Nome);
            Assert.AreEqual("ti123@gmail.com", contatoCadastrado.Email);
            Assert.AreEqual("(49) 99999-9999", contatoCadastrado.Telefone);
            Assert.IsNull(contatoCadastrado.Cargo);
            Assert.IsNull(contatoCadastrado.Empresa);

            repositorioContato.Verify(
                r => r.Cadastrar(It.IsAny<Contato>()),
                Times.Once
            );
        }

        [TestMethod]
        public void Cadastrar_ComCamposObrigatoriosEmBranco_NaoPersisteContato()
        {
            // Arranjo
            Mock<IRepositorioContato> repositorioContato = new();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();

            repositorioContato
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            ServicoContato servicoContato = new(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            // Ação
            Result resultado = servicoContato.Cadastrar(
                new CadastrarContatoDto(
                    "",
                    "",
                    "",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioContato.Verify(
                r => r.Cadastrar(It.IsAny<Contato>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComEmailDuplicado_NaoPersisteContato()
        {
            // Arranjo
            Contato contatoExistente = new Contato(
                "Ana",
                "ana@email.com",
                "(49) 99999-0001",
                null,
                null
            );

            Mock<IRepositorioContato> repositorioContato = new();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();

            repositorioContato
                .Setup(r => r.SelecionarTodos())
                .Returns([contatoExistente]);

            ServicoContato servicoContato = new(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            // Ação
            Result resultado = servicoContato.Cadastrar(
                new CadastrarContatoDto(
                    "Bruno",
                    "ana@email.com",
                    "(49) 99999-0002",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            Assert.AreEqual(
                "Email",
                resultado.Errors.Single().Metadata["Campo"]
            );

            Assert.Contains(
                "Já existe",
                resultado.Errors.Single().Message
            );

            repositorioContato.Verify(
                r => r.Cadastrar(It.IsAny<Contato>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComTelefoneDuplicado_NaoPersisteContato()
        {
            // Arranjo
            Contato contatoExistente = new Contato(
                "Ana",
                "ana@email.com",
                "(49) 99999-0001",
                null,
                null
            );

            Mock<IRepositorioContato> repositorioContato = new();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();

            repositorioContato
                .Setup(r => r.SelecionarTodos())
                .Returns([contatoExistente]);

            ServicoContato servicoContato = new(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            // Ação
            Result resultado = servicoContato.Cadastrar(
                new CadastrarContatoDto(
                    "Bruno",
                    "bruno@email.com",
                    "(49) 99999-0001",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            Assert.AreEqual(
                "Telefone",
                resultado.Errors.Single().Metadata["Campo"]
            );

            Assert.Contains(
                "Já existe",
                resultado.Errors.Single().Message
            );

            repositorioContato.Verify(
                r => r.Cadastrar(It.IsAny<Contato>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Editar_ComDadosValidos_PersisteContato()
        {
            // Arranjo
            Contato contatoExistente = new Contato(
                "Tiago",
                "tiago@email.com",
                "(49) 99999-9999",
                "Desenvolvedor",
                "ADP"
            );

            Contato? contatoAtualizado = null;

            Mock<IRepositorioContato> repositorioContato = new();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();

            repositorioContato
                .Setup(r => r.SelecionarTodos())
                .Returns([contatoExistente]);

            repositorioContato
                .Setup(r => r.Editar(
                    contatoExistente.Id,
                    It.IsAny<Contato>()
                ))
                .Callback<Guid, Contato>((id, contato) =>
                {
                    contatoAtualizado = contato;
                })
                .Returns(true);

            ServicoContato servicoContato = new(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            // Ação
            Result resultado = servicoContato.Editar(
                new EditarContatoDto(
                    contatoExistente.Id,
                    "João",
                    "joao@email.com",
                    "(49) 98888-8888",
                    "Analista",
                    "Empresa B"
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(contatoAtualizado);

            Assert.AreEqual("João", contatoAtualizado.Nome);
            Assert.AreEqual("joao@email.com", contatoAtualizado.Email);
            Assert.AreEqual("(49) 98888-8888", contatoAtualizado.Telefone);
            Assert.AreEqual("Analista", contatoAtualizado.Cargo);
            Assert.AreEqual("Empresa B", contatoAtualizado.Empresa);

            repositorioContato.Verify(
                r => r.Editar(
                    contatoExistente.Id,
                    It.IsAny<Contato>()
                ),
                Times.Once
            );
        }

        [TestMethod]
        public void Editar_ComEmailDeOutroContato_NaoPersisteContato()
        {
            // Arranjo
            Contato contatoExistente = new Contato(
                "Bruno",
                "bruno@email.com",
                "(49) 99999-0002",
                null,
                null
            );

            Contato outroContato = new Contato(
                "Ana",
                "ana@email.com",
                "(49) 99999-0001",
                null,
                null
            );

            Mock<IRepositorioContato> repositorioContato = new();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();

            repositorioContato
                .Setup(r => r.SelecionarTodos())
                .Returns([contatoExistente, outroContato]);

            ServicoContato servicoContato = new(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            // Ação
            Result resultado = servicoContato.Editar(
                new EditarContatoDto(
                    contatoExistente.Id,
                    "Bruno",
                    "ana@email.com",
                    "(49) 99999-0002",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            Assert.AreEqual(
                "Email",
                resultado.Errors.Single().Metadata["Campo"]
            );

            Assert.Contains(
                "Já existe",
                resultado.Errors.Single().Message
            );

            repositorioContato.Verify(
                r => r.Editar(
                    contatoExistente.Id,
                    It.IsAny<Contato>()
                ),
                Times.Never
            );
        }

        [TestMethod]
        public void Editar_MantendoProprioEmailETelefone_PersisteContato()
        {
            // Arranjo
            Contato contatoExistente = new Contato(
                "Tiago",
                "tiago@email.com",
                "(49) 99999-9999",
                "Desenvolvedor",
                "ADP"
            );

            Contato? contatoAtualizado = null;

            Mock<IRepositorioContato> repositorioContato = new();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();

            repositorioContato
                .Setup(r => r.SelecionarTodos())
                .Returns([contatoExistente]);

            repositorioContato
                .Setup(r => r.Editar(
                    contatoExistente.Id,
                    It.IsAny<Contato>()
                ))
                .Callback<Guid, Contato>((id, contato) =>
                {
                    contatoAtualizado = contato;
                })
                .Returns(true);

            ServicoContato servicoContato = new(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            // Ação
            Result resultado = servicoContato.Editar(
                new EditarContatoDto(
                    contatoExistente.Id,
                    "Tiago Atualizado",
                    "tiago@email.com",
                    "(49) 99999-9999",
                    "Analista",
                    "Empresa B"
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(contatoAtualizado);

            Assert.AreEqual(
                "Tiago Atualizado",
                contatoAtualizado.Nome
            );

            Assert.AreEqual(
                "tiago@email.com",
                contatoAtualizado.Email
            );

            Assert.AreEqual(
                "(49) 99999-9999",
                contatoAtualizado.Telefone
            );

            repositorioContato.Verify(
                r => r.Editar(
                    contatoExistente.Id,
                    It.IsAny<Contato>()
                ),
                Times.Once
            );
        }

        [TestMethod]
        public void Excluir_SemCompromissosVinculados_ExcluiContato()
        {
            // Arranjo
            Contato contato = new Contato(
                "Tiago",
                "tiago@email.com",
                "(49) 99999-9999",
                null,
                null
            );

            Mock<IRepositorioContato> repositorioContato = new();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();

            repositorioContato
                .Setup(r => r.SelecionarPorId(contato.Id))
                .Returns(contato);

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            repositorioContato
                .Setup(r => r.Excluir(contato.Id))
                .Returns(true);

            ServicoContato servicoContato = new(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            // Ação
            Result resultado = servicoContato.Excluir(contato.Id);

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);

            repositorioContato.Verify(
                r => r.Excluir(contato.Id),
                Times.Once
            );
        }

        [TestMethod]
        public void Excluir_ComCompromissosVinculados_RetornaErro()
        {
            //Arranjo
            Contato contato = new Contato(
                 "Tiago",
                 "tiago@email.com",
                 "(49) 99999-9999",
                 null,
                 null
             );

            Compromisso compromisso = new Compromisso(
            "Reuniao",
            DateTime.Today,
            TimeSpan.FromHours(17),
            TimeSpan.FromHours(19),
            TipoCompromisso.Presencial, "Uniplac", null, contato);


            Mock<IRepositorioContato> repositorioContato = new Mock<IRepositorioContato>();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new Mock<IRepositorioCompromisso>();

            repositorioContato.Setup(r => r.SelecionarPorId(contato.Id)).Returns(contato);
            repositorioCompromisso.Setup(r => r.SelecionarTodos()).Returns([compromisso]);

            ServicoContato servicoContato = new ServicoContato(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            //Ação
            Result resultado = servicoContato.Excluir(contato.Id);

            //Asserção
            Assert.IsTrue(resultado.IsFailed);
            Assert.Contains("Não é possível excluir este contato, pois ele possui compromissos vinculados.",
            resultado.Errors.Single().Message);

            repositorioContato.Verify(r => r.Excluir(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Excluir_ComContatoInexistente_NaoExcluiContato()
        {
            // Arranjo
            Guid idContato = Guid.NewGuid();

            Mock<IRepositorioContato> repositorioContato = new();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();

            repositorioContato
                .Setup(r => r.SelecionarPorId(idContato))
                .Returns((Contato?)null);

            ServicoContato servicoContato = new(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );

            // Ação
            Result resultado = servicoContato.Excluir(idContato);

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioContato.Verify(
                r => r.Excluir(idContato),
                Times.Never
            );
        }
        [TestMethod]
        public void SelecionarPorId_DeveRetornarContatoComDadosCorretos()
        {
            // Arranjo
            Contato contato = new Contato(
                "Tiago",
                "tiago@email.com",
                "(49) 99999-9999",
                null,
                null
            );

            Mock<IRepositorioContato> repositorioContato = new Mock<IRepositorioContato>();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new Mock<IRepositorioCompromisso>();

            repositorioContato
                .Setup(r => r.SelecionarPorId(contato.Id))
                .Returns(contato);

            ServicoContato servicoContato = new ServicoContato(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );


            // Ação
            Result<DetalhesContatoDto> resultado = servicoContato.SelecionarPorId(contato.Id);


            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(resultado.Value);

            Assert.AreEqual("Tiago", resultado.Value.Nome);
            Assert.AreEqual("tiago@email.com", resultado.Value.Email);
            Assert.AreEqual("(49) 99999-9999", resultado.Value.Telefone);

            repositorioContato.Verify(
                r => r.SelecionarPorId(contato.Id),
                Times.Once
            );
        }

        [TestMethod]
        public void SelecionarTodos_ExibeContatos()
        {
            // Arranjo
            Contato contato1 = new Contato(
                "Tiago",
                "tiago@email.com",
                "(49) 99999-9999",
                null,
                null
            );

            Contato contato2 = new Contato(
                "Rech",
                "rech@email.com",
                "(49) 98888-8888",
                null,
                null
            );

            Mock<IRepositorioContato> repositorioContato = new Mock<IRepositorioContato>();
            Mock<IRepositorioCompromisso> repositorioCompromisso = new Mock<IRepositorioCompromisso>();

            repositorioContato
                .Setup(r => r.SelecionarTodos())
                .Returns(new List<Contato>
                {
            contato1,
            contato2
                });

            ServicoContato servicoContato = new ServicoContato(
                repositorioContato.Object,
                repositorioCompromisso.Object
            );


            // Ação
            List<ListarContatosDto> listarContatos = servicoContato.SelecionarTodos();


            // Asserção
            Assert.IsNotNull(listarContatos);
            Assert.AreEqual(2, listarContatos.Count);

            Assert.AreEqual("Tiago", listarContatos[0].Nome);
            Assert.AreEqual("tiago@email.com", listarContatos[0].Email);

            Assert.AreEqual("Rech", listarContatos[1].Nome);
            Assert.AreEqual("rech@email.com", listarContatos[1].Email);

            repositorioContato.Verify(
                r => r.SelecionarTodos(),
                Times.Once
            );
        }
    }
}