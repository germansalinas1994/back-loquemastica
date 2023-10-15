using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DataAccess.IRepository;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        protected readonly EcommercedbContext _context;
        private IDbContextTransaction _transaction;



        //tengo que crear _transaction

        public UnitOfWork(EcommercedbContext context)
        {
            _context = context;

            ProductoRepository = new ProductoRepository(_context);
            CategoriaRepository = new CategoriaRepository(_context);
            PublicacionRepository = new PublicacionRepository(_context);
        }

        public IProductoRepository ProductoRepository { get; set; }
        public ICategoriaRepository CategoriaRepository { get; set; }
        public IPublicacionRepository PublicacionRepository { get; set; }
        

        public async Task BeginTransactionAsync()
        {
            try
            {
                if (_transaction == null)
                {
                    _transaction = await _context.Database.BeginTransactionAsync();
                }

            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                _transaction?.Dispose();
                _transaction = null;
                
            }


        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                _transaction?.Commit();
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }


        public void Dispose()
        {
            //el dispose es para liberar recursos

            // Dispose of the context

            _context.Dispose();
        }

        public async Task RollbackAsync()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }



    }
}