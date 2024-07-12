using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public ProdutosController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("produtos/{id:int:min(1)}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            IEnumerable<Produto> produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos == null) return NotFound($"A categoria de id {id} não possui produtos.");

            return Ok(produtos);
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            IEnumerable<Produto> produtos = _uof.ProdutoRepository.GetAll();
            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> GetById(int id)
        {
            Produto? produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto == null) return NotFound($"O produto de id {id} não existe.");

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto == null) return BadRequest();

            Produto produtoCriado = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();
            return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriado.ProdutoId }, produtoCriado);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId) return BadRequest();

            Produto produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok(produto);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            Produto? produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto == null) return NotFound($"O produto de id {id} não existe.");

            Produto produtoExcluido = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return Ok(produtoExcluido);
        }
    }
}
