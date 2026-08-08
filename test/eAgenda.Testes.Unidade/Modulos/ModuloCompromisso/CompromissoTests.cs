using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCompromisso
{
    [TestClass]
    public sealed class CompromissoTests
    {
        private const string assunto = "Reuniao";
        private readonly DateTime dataOcorrencia = DateTime.Today;
        private readonly TimeSpan horaInicio = new(14, 0, 0);
        private readonly TimeSpan horaTermino = new(15, 0, 0);

        [TestMethod]
        public void Validar_ComAssuntoVazio_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                "",
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Presencial,
                "Sala 1",
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Assunto\" deve conter entre 2 e 100 caracteres.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComAssuntoCurto_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                new string('A', 1),
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Presencial,
                "Sala 1",
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Assunto\" deve conter entre 2 e 100 caracteres.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComAssuntoNoLimiteMaximo_NaoDeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                new string('A', 100),
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Presencial,
                "Sala 1",
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.IsEmpty(erros);
        }

        [TestMethod]
        public void Validar_ComAssuntoLongo_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                new string('A', 101),
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Presencial,
                "Sala 1",
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Assunto\" deve conter entre 2 e 100 caracteres.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComDataOcorrenciaNaoPreenchida_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                default,
                horaInicio,
                horaTermino,
                TipoCompromisso.Presencial,
                "Sala 1",
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Data de Ocorrência\" deve ser preenchido.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComHoraInicioNaoPreenchida_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                default,
                horaTermino,
                TipoCompromisso.Presencial,
                "Sala 1",
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Hora de Início\" deve ser preenchido.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComHoraTerminoNaoPreenchida_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                horaInicio,
                default,
                TipoCompromisso.Presencial,
                "Sala 1",
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(2, erros);

            Assert.Contains(
                "O campo \"Hora de Término\" deve ser preenchido.",
                erros
            );

            Assert.Contains(
                "A hora de término deve ser posterior à hora de início.",
                erros
            );
        }

        [TestMethod]
        public void Validar_ComHoraTerminoAnteriorAoInicio_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                new TimeSpan(15, 0, 0),
                new TimeSpan(14, 0, 0),
                TipoCompromisso.Presencial,
                "Sala 1",
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "A hora de término deve ser posterior à hora de início.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComHoraTerminoIgualAoInicio_DeveRetornarErro()
        {
            // Arranjo
            TimeSpan hora = new(14, 0, 0);

            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                hora,
                hora,
                TipoCompromisso.Presencial,
                "Sala 1",
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "A hora de término deve ser posterior à hora de início.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComTipoNaoPreenchido_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                horaInicio,
                horaTermino,
                default,
                null,
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
               "O campo \"Local\" deve ser preenchido para compromissos presenciais.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComPresencialSemLocal_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Presencial,
                null,
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Local\" deve ser preenchido para compromissos presenciais.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComRemotoSemLink_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Remoto,
                null,
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Link\" deve ser preenchido para compromissos remotos.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComLocalMuitoLongo_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Presencial,
                new string('A', 256),
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Local\" deve conter no máximo 255 caracteres.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComLinkMuitoLongo_DeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Remoto,
                null,
                new string('A', 501),
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Link\" deve conter no máximo 500 caracteres.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComCompromissoPresencialValido_NaoDeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Presencial,
                "Sala de Reuniões",
                null,
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.IsEmpty(erros);
        }

        [TestMethod]
        public void Validar_ComCompromissoRemotoValido_NaoDeveRetornarErro()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                assunto,
                dataOcorrencia,
                horaInicio,
                horaTermino,
                TipoCompromisso.Remoto,
                null,
                "https://meet.google.com/abc-defg-hij",
                null
            );

            // Ação
            List<string> erros = compromisso.Validar();

            // Asserção
            Assert.IsEmpty(erros);
        }

        [TestMethod]
        public void Atualizar_ComDadosValidos_DeveAtualizarTodosOsDados()
        {
            // Arranjo
            Compromisso compromisso = new Compromisso(
                "Reuniao A",
                dataOcorrencia,
                new TimeSpan(14, 0, 0),
                new TimeSpan(15, 0, 0),
                TipoCompromisso.Presencial,
                "Sala A",
                null,
                null
            );

            Compromisso compromissoAtualizado = new Compromisso(
                "Reuniao B",
                dataOcorrencia.AddDays(1),
                new TimeSpan(16, 0, 0),
                new TimeSpan(17, 0, 0),
                TipoCompromisso.Remoto,
                null,
                "https://meet.google.com/abc",
                null
            );

            // Ação
            compromisso.Atualizar(compromissoAtualizado);

            // Asserção
            Assert.AreEqual("Reuniao B", compromisso.Assunto);
            Assert.AreEqual(dataOcorrencia.AddDays(1), compromisso.DataOcorrencia);
            Assert.AreEqual(new TimeSpan(16, 0, 0), compromisso.HoraInicio);
            Assert.AreEqual(new TimeSpan(17, 0, 0), compromisso.HoraTermino);
            Assert.AreEqual(TipoCompromisso.Remoto, compromisso.Tipo);
            Assert.IsNull(compromisso.Local);
            Assert.AreEqual("https://meet.google.com/abc", compromisso.Link);
            Assert.IsNull(compromisso.Contato);
        }
    }
}