using System;
using DataAccess.IRepository;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace DataAccess.Repository
{
    public class DomicilioRepository : GenericRepository<Domicilio>, IDomicilioRepository
    {


        public DomicilioRepository(DbveterinariaContext _context) : base(_context)
        {
        }

    


    }
}

