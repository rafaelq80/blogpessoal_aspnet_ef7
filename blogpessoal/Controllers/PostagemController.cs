using blogpessoal.Models;
using blogpessoal.Repositories;
using blogpessoal.Repositories.Implements;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogpessoal.Controllers
{
    
    [Authorize]
    [Route("~/postagens")]
    [ApiController]
    public class PostagemController : ControllerBase
    {

        private readonly IPostagemRepository _postagemRepository;
        private readonly IValidator<Postagem> _postagemValidator;

        public PostagemController(
            IPostagemRepository postagemRepository, 
            IValidator<Postagem> postagemValidator
            )
        {
            _postagemRepository = postagemRepository;
            _postagemValidator = postagemValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _postagemRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var resposta = await _postagemRepository.GetById(id);

            if (resposta is null)
            {
                return NotFound();
            }

            return Ok(resposta);
        }

        [HttpGet("titulo/{titulo}")]
        public async Task<ActionResult> GetByTitulo(string titulo)
        {
            return Ok(await _postagemRepository.GetByTitulo(titulo));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Postagem postagem)
        {
            var validarPostagem = await _postagemValidator.ValidateAsync(postagem);

            if (!validarPostagem.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarPostagem);

            var Resposta = await _postagemRepository.Create(postagem);

            if(Resposta is null)
                return BadRequest("Tema não encontrado!");

            return CreatedAtAction(nameof(GetById), new { id = Resposta.Id }, Resposta);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Postagem postagem)
        {
            if (postagem.Id == 0)
                return BadRequest("Id da Postagem é inválido!");

            var validarPostagem = await _postagemValidator.ValidateAsync(postagem);

            if (!validarPostagem.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarPostagem);

            var Resposta = await _postagemRepository.Update(postagem);

            if (Resposta is null)
                return NotFound("Postagem e/ou Tema não encontrados!");

            return Ok(Resposta);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var BuscaPostagem = await _postagemRepository.GetById(id);

            if (BuscaPostagem is null)
                return NotFound("Postagem não encontrada!");
            
           await _postagemRepository.Delete(BuscaPostagem);
            
           return NoContent();

        }

    }
}
