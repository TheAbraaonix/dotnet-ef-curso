using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repository;

        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _repository.GetProdutos().ToList();
            
            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> GetById(int id)
        {
            Produto produto = _repository.GetProduto(id);

            if (produto == null) return NotFound($"O produto de id {id} não existe.");

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto == null) return BadRequest();

            Produto produtoCriado = _repository.Create(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriado.ProdutoId }, produtoCriado);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId) return BadRequest();

            bool resultado = _repository.Update(id, produto);

            if (!resultado) return StatusCode(500, $"Falha ao atualizar o produto de id {id}.");

            return Ok(produto);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            bool resultado = _repository.Delete(id);

            if (!resultado) return StatusCode(500, $"Falha ao atualizar o produto de id {id}.");

            return Ok();
        }
    }
}
