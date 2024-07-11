using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.Take(10).AsNoTracking().ToList();

            if (produtos == null) return NotFound("Produtos não encontrados...");
            
            return produtos;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> GetById(int id)
        {
            Produto? produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null) return NotFound($"O produto de id {id} não existe.");

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto == null) return BadRequest();
            
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId) return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null) return NotFound($"O produto de id {id} não existe.");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok();
        }
    }
}
