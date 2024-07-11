using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            return _context.Categorias.ToList();
        }

        public Categoria GetCategoria(int id)
        {
            Categoria categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria == null) throw new InvalidOperationException("Categoria é null");

            return categoria;
        }

        public Categoria Create(Categoria categoria)
        {
            if (categoria == null) throw new ArgumentNullException(nameof(categoria));

            _context.Add(categoria);
            _context.SaveChanges();
            
            return categoria;
        }

        public Categoria Update(int id, Categoria categoria)
        {
            if (categoria == null) throw new ArgumentNullException(nameof(categoria));

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return categoria;
        }

        public Categoria Delete(int id)
        {
            Categoria categoria = _context.Categorias.Find(id);

            if (categoria == null) throw new ArgumentNullException(nameof(categoria));

            _context.Remove(categoria);
            _context.SaveChanges();

            return categoria;
        }
    }
}
