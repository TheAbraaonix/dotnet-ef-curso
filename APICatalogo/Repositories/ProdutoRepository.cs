using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public IQueryable<Produto> GetProdutos()
        {
            return _context.Produtos;
        }

        public Produto GetProduto(int id)
        {
            Produto produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null) throw new InvalidOperationException("Produto é null");

            return produto;
        }

        public Produto Create(Produto produto)
        {
            if (produto == null) throw new InvalidOperationException("Produto é null");

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return produto;
        }
        
        public bool Update(int id, Produto produto)
        {
            if (produto == null) throw new InvalidOperationException("Produto é null");

            if (_context.Produtos.Any(p => p.ProdutoId == id))
            {
                _context.Update(produto);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            Produto produto = _context.Produtos.Find(id);

            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
