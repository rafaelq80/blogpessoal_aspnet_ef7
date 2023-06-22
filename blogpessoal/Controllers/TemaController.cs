using blogpessoal.Models;
using blogpessoal.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace blogpessoal.Controllers
{
    [Authorize]
    [Route("~/temas")]
    [ApiController]
    public class TemaController : ControllerBase
    {

        private readonly ITemaRepository _temaRepository;
        private readonly IValidator<Tema> _temaValidator;

        public TemaController(
            ITemaRepository temaRepository, 
            IValidator<Tema> temaValidator
            )
        {
            _temaRepository = temaRepository;
            _temaValidator = temaValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _temaRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _temaRepository.GetById(id);

            if (Resposta is null)
            {
                return NotFound();
            }

            return Ok(Resposta);
        }

        [HttpGet("descricao/{descricao}")]
        public async Task<ActionResult> GetByDescricao(string descricao)
        {
            return Ok(await _temaRepository.GetByDescricao(descricao));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Tema tema)
        {
            var validarTema = await _temaValidator.ValidateAsync(tema);

            if (!validarTema.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarTema);

            var Resposta = await _temaRepository.Create(tema);
            return CreatedAtAction(nameof(GetById), new { id = Resposta.Id }, Resposta);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Tema tema)
        {
            if (tema.Id == 0)
                return BadRequest("O Id do Tema é inválido!");

            var validarTema = await _temaValidator.ValidateAsync(tema);

            if (!validarTema.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarTema);

            var Resposta = await _temaRepository.Update(tema);

            if (Resposta is null)
                return NotFound();

            return Ok(Resposta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var BuscaTema = await _temaRepository.GetById(id);

            if (BuscaTema is null)
                return NotFound();

            await _temaRepository.Delete(BuscaTema);

            return NoContent();

        }

    }
}
