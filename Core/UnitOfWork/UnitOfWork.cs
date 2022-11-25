using System;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Persistence;
using Core.Repository;
using Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext context;
        private IDbContextTransaction transaction;

        public UnitOfWork(DatabaseContext context)
        {
            this.context = context;
        }

        private async Task StartNewTransactionAsync()
        {
            transaction ??= await context.Database.BeginTransactionAsync();
        }

        public async Task ForceBeginTransaction()
        {
            await StartNewTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await context.SaveChangesAsync();
            if (transaction != null)
                await transaction.CommitAsync();
        }

        public async Task RollbackTransaction()
        {
            if (transaction == null)
                return;

            await transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            await StartNewTransactionAsync();
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context?.Dispose();
            transaction?.Dispose();
            transaction = null;
        }


        private GenericRepository<Weather> weatherRepository;
        public IGenericRepository<Weather> WeatherRepository => weatherRepository ??= new WeatherRepository(context);
    }
}
