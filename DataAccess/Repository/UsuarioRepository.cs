using System;
using DataAccess.IRepository;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace DataAccess.Repository
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {


        //que hace esta linea
        public UsuarioRepository(DbveterinariaContext _context) : base(_context)
        {
        }

    


    }
}

