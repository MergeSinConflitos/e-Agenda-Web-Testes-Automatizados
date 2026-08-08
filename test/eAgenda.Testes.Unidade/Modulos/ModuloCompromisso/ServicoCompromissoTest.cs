using eAgenda.Aplicacao.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCompromisso
{
    [TestClass]
    public sealed class ServicoCompromissoTests
    {
        private readonly DateTime dataOcorrencia = DateTime.Today;
        private readonly TimeSpan horaInicio = TimeSpan.FromHours(14);
        private readonly TimeSpan horaTermino = TimeSpan.FromHours(15);

        [TestMethod]
        public void Cadastrar_ComDadosPresenciaisValidos_PersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            Compromisso? compromissoCadastrado = null;

            repositorioCompromisso
                .Setup(r => r.Cadastrar(It.IsAny<Compromisso>()))
                .Callback<Compromisso>(compromisso =>
                    compromissoCadastrado = compromisso);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Reuniao",
                    dataOcorrencia,
                    horaInicio,
                    horaTermino,
                    TipoCompromisso.Presencial,
                    "Sala de Reunioes",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(compromissoCadastrado);

            Assert.AreEqual("Reuniao", compromissoCadastrado.Assunto);
            Assert.AreEqual(
                dataOcorrencia,
                compromissoCadastrado.DataOcorrencia
            );
            Assert.AreEqual(
                horaInicio,
                compromissoCadastrado.HoraInicio
            );
            Assert.AreEqual(
                horaTermino,
                compromissoCadastrado.HoraTermino
            );
            Assert.AreEqual(
                TipoCompromisso.Presencial,
                compromissoCadastrado.Tipo
            );
            Assert.AreEqual(
                "Sala de Reunioes",
                compromissoCadastrado.Local
            );
            Assert.IsNull(compromissoCadastrado.Link);
            Assert.IsNull(compromissoCadastrado.Contato);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Once
            );
        }

        [TestMethod]
        public void Cadastrar_ComDadosRemotosValidos_PersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            Compromisso? compromissoCadastrado = null;

            repositorioCompromisso
                .Setup(r => r.Cadastrar(It.IsAny<Compromisso>()))
                .Callback<Compromisso>(compromisso =>
                    compromissoCadastrado = compromisso);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Reuniao",
                    dataOcorrencia,
                    horaInicio,
                    horaTermino,
                    TipoCompromisso.Remoto,
                    null,
                    "https://meet.google.com/abc",
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(compromissoCadastrado);

            Assert.AreEqual(
                TipoCompromisso.Remoto,
                compromissoCadastrado.Tipo
            );
            Assert.IsNull(compromissoCadastrado.Local);
            Assert.AreEqual(
                "https://meet.google.com/abc",
                compromissoCadastrado.Link
            );

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Once
            );
        }

        [TestMethod]
        public void Cadastrar_SemContato_PersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            Compromisso? compromissoCadastrado = null;

            repositorioCompromisso
                .Setup(r => r.Cadastrar(It.IsAny<Compromisso>()))
                .Callback<Compromisso>(compromisso =>
                    compromissoCadastrado = compromisso);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Reuniao",
                    dataOcorrencia,
                    horaInicio,
                    horaTermino,
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(compromissoCadastrado);
            Assert.IsNull(compromissoCadastrado.Contato);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Once
            );
        }

        [TestMethod]
        public void Cadastrar_ComCamposObrigatoriosEmBranco_NaoPersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "",
                    default,
                    default,
                    default,
                    default,
                    null,
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComAssuntoDeUmCaractere_NaoPersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "A",
                    dataOcorrencia,
                    horaInicio,
                    horaTermino,
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComAssuntoDe100Caracteres_PersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            Compromisso? compromissoCadastrado = null;

            repositorioCompromisso
                .Setup(r => r.Cadastrar(It.IsAny<Compromisso>()))
                .Callback<Compromisso>(c =>
                    compromissoCadastrado = c);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            string assunto = new string('A', 100);

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    assunto,
                    dataOcorrencia,
                    horaInicio,
                    horaTermino,
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(compromissoCadastrado);
            Assert.AreEqual(100, compromissoCadastrado.Assunto.Length);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Once
            );
        }

        [TestMethod]
        public void Cadastrar_ComAssuntoDe101Caracteres_NaoPersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    new string('A', 101),
                    dataOcorrencia,
                    horaInicio,
                    horaTermino,
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComPresencialSemLocal_NaoPersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Reuniao",
                    dataOcorrencia,
                    horaInicio,
                    horaTermino,
                    TipoCompromisso.Presencial,
                    null,
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComRemotoSemLink_NaoPersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Reuniao",
                    dataOcorrencia,
                    horaInicio,
                    horaTermino,
                    TipoCompromisso.Remoto,
                    null,
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComLinkInvalido_NaoPersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Reuniao",
                    dataOcorrencia,
                    horaInicio,
                    horaTermino,
                    TipoCompromisso.Remoto,
                    null,
                    "link-invalido",
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComHoraTerminoAnteriorAoInicio_NaoPersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Reuniao",
                    dataOcorrencia,
                    TimeSpan.FromHours(15),
                    TimeSpan.FromHours(14),
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComHoraTerminoIgualAoInicio_NaoPersisteCompromisso()
        {
            // Arranjo
            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            TimeSpan hora = TimeSpan.FromHours(14);

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Reuniao",
                    dataOcorrencia,
                    hora,
                    hora,
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComSobreposicaoParcial_NaoPersisteCompromisso()
        {
            // Arranjo
            Compromisso compromissoExistente = new(
                "Existente",
                dataOcorrencia,
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(15),
                TipoCompromisso.Presencial,
                "Sala",
                null,
                null
            );

            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([compromissoExistente]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Novo",
                    dataOcorrencia,
                    TimeSpan.FromHours(14.5),
                    TimeSpan.FromHours(16),
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            Assert.Contains(
                "Já existe um compromisso cadastrado neste intervalo de horário.",
                resultado.Errors.Single().Message
            );

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComCompromissoContidoEmOutro_NaoPersisteCompromisso()
        {
            // Arranjo
            Compromisso compromissoExistente = new(
                "Existente",
                dataOcorrencia,
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(17),
                TipoCompromisso.Presencial,
                "Sala",
                null,
                null
            );

            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([compromissoExistente]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Novo",
                    dataOcorrencia,
                    TimeSpan.FromHours(15),
                    TimeSpan.FromHours(16),
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ComNovoCompromissoEnglobandoOutro_NaoPersisteCompromisso()
        {
            // Arranjo
            Compromisso compromissoExistente = new(
                "Existente",
                dataOcorrencia,
                TimeSpan.FromHours(15),
                TimeSpan.FromHours(16),
                TipoCompromisso.Presencial,
                "Sala",
                null,
                null
            );

            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([compromissoExistente]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Novo",
                    dataOcorrencia,
                    TimeSpan.FromHours(14),
                    TimeSpan.FromHours(17),
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Never
            );
        }

        [TestMethod]
        public void Cadastrar_ImediatamenteAposOutro_PersisteCompromisso()
        {
            // Arranjo
            Compromisso compromissoExistente = new(
                "Existente",
                dataOcorrencia,
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(15),
                TipoCompromisso.Presencial,
                "Sala",
                null,
                null
            );

            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([compromissoExistente]);

            Compromisso? compromissoCadastrado = null;

            repositorioCompromisso
                .Setup(r => r.Cadastrar(It.IsAny<Compromisso>()))
                .Callback<Compromisso>(c =>
                    compromissoCadastrado = c);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Novo",
                    dataOcorrencia,
                    TimeSpan.FromHours(15),
                    TimeSpan.FromHours(16),
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(compromissoCadastrado);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Once
            );
        }

        [TestMethod]
        public void Cadastrar_ComMesmoHorarioEmOutraData_PersisteCompromisso()
        {
            // Arranjo
            Compromisso compromissoExistente = new(
                "Existente",
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Presencial,
                "Sala",
                null,
                null
            );

            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([compromissoExistente]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Cadastrar(
                new CadastrarCompromissoDto(
                    "Novo",
                    dataOcorrencia.AddDays(1),
                    horaInicio,
                    horaTermino,
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);

            repositorioCompromisso.Verify(
                r => r.Cadastrar(It.IsAny<Compromisso>()),
                Times.Once
            );
        }

        [TestMethod]
        public void Editar_ComDadosValidos_PersisteCompromisso()
        {
            // Arranjo
            Compromisso compromissoExistente = new(
                "Reuniao A",
                dataOcorrencia,
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(15),
                TipoCompromisso.Presencial,
                "Sala A",
                null,
                null
            );

            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([compromissoExistente]);

            repositorioCompromisso
                .Setup(r => r.Editar(
                    compromissoExistente.Id,
                    It.IsAny<Compromisso>()
                ))
                .Returns(true);

            Compromisso? compromissoAtualizado = null;

            repositorioCompromisso
                .Setup(r => r.Editar(
                    compromissoExistente.Id,
                    It.IsAny<Compromisso>()
                ))
                .Callback<Guid, Compromisso>((id, compromisso) =>
                {
                    compromissoAtualizado = compromisso;
                })
                .Returns(true);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Editar(
                new EditarCompromissoDto(
                    compromissoExistente.Id,
                    "Reuniao B",
                    dataOcorrencia,
                    TimeSpan.FromHours(16),
                    TimeSpan.FromHours(17),
                    TipoCompromisso.Presencial,
                    "Sala B",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(compromissoAtualizado);

            Assert.AreEqual(
                "Reuniao B",
                compromissoAtualizado.Assunto
            );

            Assert.AreEqual(
                TimeSpan.FromHours(16),
                compromissoAtualizado.HoraInicio
            );

            Assert.AreEqual(
                TimeSpan.FromHours(17),
                compromissoAtualizado.HoraTermino
            );

            repositorioCompromisso.Verify(
                r => r.Editar(
                    compromissoExistente.Id,
                    It.IsAny<Compromisso>()
                ),
                Times.Once
            );
        }

        [TestMethod]
        public void Editar_GerandoConflito_NaoPersisteCompromisso()
        {
            // Arranjo
            Compromisso compromissoExistente = new(
                "Reuniao A",
                dataOcorrencia,
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(15),
                TipoCompromisso.Presencial,
                "Sala A",
                null,
                null
            );

            Compromisso outroCompromisso = new(
                "Reuniao B",
                dataOcorrencia,
                TimeSpan.FromHours(16),
                TimeSpan.FromHours(17),
                TipoCompromisso.Presencial,
                "Sala B",
                null,
                null
            );

            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([
                    compromissoExistente,
                outroCompromisso
                ]);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Editar(
                new EditarCompromissoDto(
                    compromissoExistente.Id,
                    "Reuniao Atualizada",
                    dataOcorrencia,
                    TimeSpan.FromHours(16.5),
                    TimeSpan.FromHours(18),
                    TipoCompromisso.Presencial,
                    "Sala",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsFailed);

            Assert.Contains(
                "Já existe um compromisso cadastrado neste intervalo de horário.",
                resultado.Errors.Single().Message
            );

            repositorioCompromisso.Verify(
                r => r.Editar(
                    compromissoExistente.Id,
                    It.IsAny<Compromisso>()
                ),
                Times.Never
            );
        }

        [TestMethod]
        public void Editar_MantendoProprioHorario_PersisteCompromisso()
        {
            // Arranjo
            Compromisso compromissoExistente = new(
                "Reuniao",
                dataOcorrencia,
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(15),
                TipoCompromisso.Presencial,
                "Sala",
                null,
                null
            );

            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([compromissoExistente]);

            repositorioCompromisso
                .Setup(r => r.Editar(
                    compromissoExistente.Id,
                    It.IsAny<Compromisso>()
                ))
                .Returns(true);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Editar(
                new EditarCompromissoDto(
                    compromissoExistente.Id,
                    "Reuniao Atualizada",
                    dataOcorrencia,
                    TimeSpan.FromHours(14),
                    TimeSpan.FromHours(15),
                    TipoCompromisso.Presencial,
                    "Sala Nova",
                    null,
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);

            repositorioCompromisso.Verify(
                r => r.Editar(
                    compromissoExistente.Id,
                    It.IsAny<Compromisso>()
                ),
                Times.Once
            );
        }

        [TestMethod]
        public void Editar_AlterandoPresencialParaRemoto_PersisteCompromisso()
        {
            // Arranjo
            Compromisso compromissoExistente = new(
                "Reuniao",
                dataOcorrencia,
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(15),
                TipoCompromisso.Presencial,
                "Sala",
                null,
                null
            );

            Mock<IRepositorioCompromisso> repositorioCompromisso = new();
            Mock<IRepositorioContato> repositorioContato = new();

            repositorioCompromisso
                .Setup(r => r.SelecionarTodos())
                .Returns([compromissoExistente]);

            Compromisso? compromissoAtualizado = null;

            repositorioCompromisso
                .Setup(r => r.Editar(
                    compromissoExistente.Id,
                    It.IsAny<Compromisso>()
                ))
                .Callback<Guid, Compromisso>((id, compromisso) =>
                {
                    compromissoAtualizado = compromisso;
                })
                .Returns(true);

            ServicoCompromisso servicoCompromisso = new(
                repositorioCompromisso.Object,
                repositorioContato.Object
            );

            // Ação
            Result resultado = servicoCompromisso.Editar(
                new EditarCompromissoDto(
                    compromissoExistente.Id,
                    "Reuniao Online",
                    dataOcorrencia,
                    TimeSpan.FromHours(14),
                    TimeSpan.FromHours(15),
                    TipoCompromisso.Remoto,
                    null,
                    "https://meet.google.com/abc",
                    null
                )
            );

            // Asserção
            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsNotNull(compromissoAtualizado);

            Assert.AreEqual(
                TipoCompromisso.Remoto,
                compromissoAtualizado.Tipo
            );

            Assert.IsNull(compromissoAtualizado.Local);

            Assert.AreEqual(
                "https://meet.google.com/abc",
                compromissoAtualizado.Link
            );

            repositorioCompromisso.Verify(
                r => r.Editar(
                    compromissoExistente.Id,
                    It.IsAny<Compromisso>()
                ),
                Times.Once
            );
        }
    }
}