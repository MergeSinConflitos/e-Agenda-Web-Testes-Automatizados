using eAgenda.Dominio.Modulos.ModuloContato;

namespace eAgenda.Testes.Unidade.Modulos.ModuloContato
{
    [TestClass]
    public sealed class ContatoTests()
    {
        private const string nome = "Tiago";
        private const string email = "teste@example.com";
        private const string telefone = "(49) 99999-9999";

        [TestMethod]
        public void Validar_ComNomeVazio_DeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                "",
                email,
                telefone,
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComNomeCurto_DeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                new string('A', 1),
                email,
                telefone,
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComNomeLongo_DeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                new string('A', 101),
                email,
                telefone,
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComNomeNoLimiteMinimo_NaoDeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                new string('A', 2),
                email,
                telefone,
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.IsEmpty(erros);
        }

        [TestMethod]
        public void Validar_ComNomeNoLimiteMaximo_NaoDeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                new string('A', 100),
                email,
                telefone,
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.IsEmpty(erros);
        }

        [TestMethod]
        public void Validar_ComEmailVazio_DeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                nome,
                "",
                telefone,
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"E-mail\" deve conter um endereço de e-mail válido.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComEmailInvalido_DeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                nome,
                "joao123.com",
                telefone,
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"E-mail\" deve conter um endereço de e-mail válido.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComEmailSemDominio_DeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                nome,
                "joao@",
                telefone,
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"E-mail\" deve conter um endereço de e-mail válido.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComEmailValido_NaoDeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                nome,
                email,
                telefone,
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.IsEmpty(erros);
        }

        [TestMethod]
        public void Validar_ComTelefoneInvalido_DeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                nome,
                email,
                "49999999999",
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Telefone\" deve estar no formato (XX) XXXX-XXXX ou (XX) XXXXX-XXXX.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComTelefoneFixoValido_NaoDeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                nome,
                email,
                "(49) 3333-4444",
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.IsEmpty(erros);
        }

        [TestMethod]
        public void Validar_ComTelefoneCelularValido_NaoDeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                nome,
                email,
                telefone,
                null,
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.IsEmpty(erros);
        }

        [TestMethod]
        public void Validar_ComCargoMuitoLongo_DeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                nome,
                email,
                telefone,
                new string('A', 101),
                null
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Cargo\" deve conter no máximo 100 caracteres.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComEmpresaMuitoLonga_DeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                nome,
                email,
                telefone,
                null,
                new string('A', 101)
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.HasCount(1, erros);
            Assert.AreEqual(
                "O campo \"Empresa\" deve conter no máximo 100 caracteres.",
                erros.First()
            );
        }

        [TestMethod]
        public void Validar_ComDadosValidos_NaoDeveRetornarErro()
        {
            // Arranjo
            Contato contato = new Contato(
                nome,
                email,
                telefone,
                "Desenvolvedor",
                "Empresa Teste"
            );

            // Ação
            List<string> erros = contato.Validar();

            // Asserção
            Assert.IsEmpty(erros);
        }

        [TestMethod]
        public void Atualizar_ComDadosValidos_DeveAtualizarTodosOsDados()
        {
            // Arranjo
            Contato contato = new Contato(
                "Tiago",
                "tiago@example.com",
                "(49) 3333-4444",
                "Desenvolvedor",
                "Empresa A"
            );

            Contato contatoAtualizado = new Contato(
                "João",
                "joao@example.com",
                "(49) 99999-8888",
                "Analista",
                "Empresa B"
            );

            // Ação
            contato.Atualizar(contatoAtualizado);

            // Asserção
            Assert.AreEqual("João", contato.Nome);
            Assert.AreEqual("joao@example.com", contato.Email);
            Assert.AreEqual("(49) 99999-8888", contato.Telefone);
            Assert.AreEqual("Analista", contato.Cargo);
            Assert.AreEqual("Empresa B", contato.Empresa);
        }
    }
}