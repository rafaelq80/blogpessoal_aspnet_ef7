using blogpessoal.Models;
using blogpessoal.Repositories;
using blogpessoal.Repositories.Implements;
using blogpessoal.Services;
using blogpessoal.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Controllers
{

    [Route("~/usuarios")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IValidator<User> _userValidator;
        private readonly IAppAuthService _appAuthService;

        public UserController(
            IUserRepository userRepository,
            IValidator<User> userValidator,
            IAppAuthService appAuthService
            )
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
            _appAuthService = appAuthService;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _userRepository.GetAll());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _userRepository.GetById(id);

            if (Resposta is null)
            {
                return NotFound("Usuário não encontrado!");
            }

            return Ok(Resposta);
        }

        [AllowAnonymous]
        [HttpPost("cadastrar")]
        public async Task<ActionResult> Post([FromBody] User user)
        {
            var validarUser = await _userValidator.ValidateAsync(user);

            if (!validarUser.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarUser);

            var Resposta = await _userRepository.Create(user);

            if (Resposta is null)
                return BadRequest("Usuário já cadastrado!");

            return CreatedAtAction(nameof(GetById), new { id = Resposta.Id }, Resposta);
        }

        [Authorize]
        [HttpPut("atualizar")]
        public async Task<ActionResult> Put([FromBody] User user)
        {
            if (user.Id == 0)
                return BadRequest("O Id do Usuário é inválido!");
            var validarUser = await _userValidator.ValidateAsync(user);

            if (!validarUser.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarUser);

            var UserUpdate = await _userRepository.GetByUsuario(user.Usuario);

            if ((UserUpdate is not null) && (UserUpdate.Id != user.Id))
                return BadRequest("O Usuário (e-mail) já está em uso por outro usuário.");

            var Resposta = await _userRepository.Update(user);

            if (Resposta is null)
                return BadRequest("Usuário não encontrado!");

            return Ok(Resposta);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("logar")]
        public async Task<IActionResult> Authenticate([FromBody] UserLogin usuarioLogin)
        {
            var Resposta = await _appAuthService.Autenticar(usuarioLogin);

            if (Resposta == null)
            {
                return Unauthorized("Usuário e/ou Senha inválidos!");
            }

            return Ok(Resposta);
        }

    }
}
