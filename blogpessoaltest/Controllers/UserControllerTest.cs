using blogpessoal.Models;
using blogpessoaltest.Helpers;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace blogpessoaltest.Controllers
{

    [Order(1)]
    public class UserControllerTest : IClassFixture<TestingWebAppFactory<Program>>
    {

        private readonly HttpClient _client;

        private readonly ITestOutputHelper _output;

        private string Token { get; set; } = string.Empty;

        private string Id { get; set; } = string.Empty;

        public UserControllerTest(
            TestingWebAppFactory<Program> factory,
            ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;

        }

        [Fact, Order(1)]
        public async Task DeveCriarNovoUsuario()
        {
            var novoUsuario = new Dictionary<string, string>
            {
                { "nome", "Root" },
                { "usuario", "root@root.com.br" },
                { "senha", "rootroot" },
                { "foto", "-" }
            };

            var corpoRequisicao = JsonConvert.SerializeObject(novoUsuario);

            var requisicaoPost = new HttpRequestMessage(HttpMethod.Post, "/usuarios/cadastrar")
            {
                Content = new StringContent(
                corpoRequisicao,
                Encoding.UTF8,
                "application/json"
                )
            };

            var respostaPost = await _client.SendAsync(requisicaoPost);

            var statusCode = respostaPost.StatusCode;

            Assert.Equal(201, (int)statusCode);

        }

        [Theory, Order(2)]
        [InlineData("root@root.com.br", "rootroot")]
        public async Task<string> DeveLogarUsuario(string Usuario, string Senha)
        {

            var usuarioLogin = new Dictionary<string, string>
            {
                { "usuario", Usuario },
                { "senha", Senha }
            };

            var corpoRequisicao = JsonConvert.SerializeObject(usuarioLogin);

            var requisicaoPost = new HttpRequestMessage(HttpMethod.Post, "/usuarios/logar")
            {
                Content = new StringContent(
                corpoRequisicao,
                Encoding.UTF8,
                "application/json"
                )
            };

            var respostaPost = await _client.SendAsync(requisicaoPost);

            var corpoResposta = await respostaPost.Content.ReadFromJsonAsync<UserLogin>();

            if (corpoResposta is not null )
                Token = corpoResposta.Token;

            var statusCode = respostaPost.StatusCode;

            Assert.Equal(200, (int)statusCode);
            
            _output.WriteLine(Token);

            return Token;
        }


        [Fact, Order(3)]
        public async Task NaoDeveCriarUsuarioDuplicado()
        {
            var novoUsuario = new Dictionary<string, string>
            {
                { "nome", "Juliana Andrews" },
                { "usuario", "juliana@email.com.br" },
                { "senha", "12345678" },
                { "foto", "-" }
            };

            var corpoRequisicao = JsonConvert.SerializeObject(novoUsuario);

            //Enviar a primeira vez

            var requisicaoPost = new HttpRequestMessage(HttpMethod.Post, "/usuarios/cadastrar")
            {
                Content = new StringContent(
                corpoRequisicao,
                Encoding.UTF8,
                "application/json"
                )
            };

            await _client.SendAsync(requisicaoPost);

            //Enviar a segunda vez

            var requisicaoPostDuplicada = new HttpRequestMessage(HttpMethod.Post, "/usuarios/cadastrar")
            {
                Content = new StringContent(
                corpoRequisicao,
                Encoding.UTF8,
                "application/json"
                )
            };

            var respostaPost = await _client.SendAsync(requisicaoPostDuplicada);

            var statusCode = respostaPost.StatusCode;

            Assert.Equal(400, (int)statusCode);

        }

        [Fact, Order(4)]
        public async Task DeveAtualizarUsuario()
        {
            var criarUsuario = new Dictionary<string, string>
            {
                { "nome", "Paulo Antunes" },
                { "usuario", "paulo@email.com.br" },
                { "senha", "12345678" },
                { "foto", "-" }
            };

            var corpoRequisicaoCriar = JsonConvert.SerializeObject(criarUsuario);

            //Criar usuário

            var requisicaoPost = new HttpRequestMessage(HttpMethod.Post, "/usuarios/cadastrar")
            {
                Content = new StringContent(
                corpoRequisicaoCriar,
                Encoding.UTF8,
                "application/json"
                )
            };

            var respostaPost = await _client.SendAsync(requisicaoPost);

            var corpoRespostaPost = await respostaPost.Content.ReadFromJsonAsync<User>();
            
            if (corpoRespostaPost is not null)
            {
                Id = corpoRespostaPost.Id.ToString();
            }

            //Atualizar Usuário

            var atualizarUsuario = new Dictionary<string, string>
            {
                { "id", Id },
                { "nome", "Paulo Cesar Antunes" },
                { "usuario", "paulo_cesar@email.com.br" },
                { "senha", "12345678" },
                { "foto", "-" }
            };

            var corpoRequisicaoAtualizar = JsonConvert.SerializeObject(atualizarUsuario);

            var requisicaoPut = new HttpRequestMessage(HttpMethod.Put, "/usuarios/atualizar")
            {
                Content = new StringContent(
                corpoRequisicaoAtualizar,
                Encoding.UTF8,
                "application/json"
                )
            };

            var Token = await DeveLogarUsuario("root@root.com.br", "rootroot");

            _client.DefaultRequestHeaders.Add("Authorization", Token);
            var respostaPut = await _client.SendAsync(requisicaoPut);

            var statusCode = respostaPut.StatusCode;

            Assert.Equal(200, (int)statusCode);

        }

        [Fact, Order(5)]
        public async Task DeveListarTodosOsUsuarios()
        {

            var Token = await DeveLogarUsuario("root@root.com.br", "rootroot");

            _client.DefaultRequestHeaders.Add("Authorization", Token);
            var respostaGet = await _client.GetAsync("/usuarios/all");

            var statusCode = respostaGet.StatusCode;

            Assert.Equal(200, (int)statusCode);

        }

        [Fact, Order(6)]
        public async Task DeveListarUmUsuario()
        {
            var criarUsuario = new Dictionary<string, string>
            {
                { "nome", "Paula Ramos" },
                { "usuario", "paula@email.com.br" },
                { "senha", "12345678" },
                { "foto", "-" }
            };

            var corpoRequisicaoCriar = JsonConvert.SerializeObject(criarUsuario);

            //Criar usuário

            var requisicaoPost = new HttpRequestMessage(HttpMethod.Post, "/usuarios/cadastrar")
            {
                Content = new StringContent(
                corpoRequisicaoCriar,
                Encoding.UTF8,
                "application/json"
                )
            };

            var respostaPost = await _client.SendAsync(requisicaoPost);

            var corpoRespostaPost = await respostaPost.Content.ReadFromJsonAsync<User>();

            if (corpoRespostaPost is not null)
            {
                Id = corpoRespostaPost.Id.ToString();
            }

            var Token = await DeveLogarUsuario("root@root.com.br", "rootroot");

            _client.DefaultRequestHeaders.Add("Authorization", Token);
            var respostaGet = await _client.GetAsync("/usuarios/" + Id);

            var statusCode = respostaGet.StatusCode;

            Assert.Equal(200, (int)statusCode);

        }

    }
}
