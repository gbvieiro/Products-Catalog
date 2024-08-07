﻿using Product.Catalog.Infra.Data.Database;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.Interfaces;

namespace Product.Catalog.Infra.Data.Repositories
{
    public class StoksRepository : IStocksRepository
    {
        public Task DeleteAsync(Guid id)
        {
            Context.Stocks = new List<Stock>(Context.Stocks.Where(x => x.Id != id));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Stock>> GetAllAsync(string filter, int skip, int take)
        {
            if (Context.Stocks == null)
            {
                throw new Exception("Context is not initialized.");
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = filter.ToLower();

                var stocks = Context.Stocks.Where(x =>
                    x.BookId.ToString().ToLower().Contains(filter) ||
                    x.Quantity.ToString().Contains(filter)
                ).Skip(skip).Take(take);

                return Task.FromResult(stocks);
            }

            return Task.FromResult(Context.Stocks.Skip(skip).Take(take));
        }

        public Task<Stock?> GetAsync(Guid id) =>
            Task.FromResult(Context.Stocks.FirstOrDefault(x => x.Id == id));

        public Task<Stock?> GetByBookId(Guid bookId) =>
            Task.FromResult(Context.Stocks.FirstOrDefault(x => x.BookId == bookId));

        public Task SaveAsync(Stock entity)
        {
            var savedEntityIndex = Context.Stocks.FindIndex(x => x.Id == entity.Id);
            if (savedEntityIndex < 0)
            {
                Context.Stocks.Add(entity);
            }
            else
            {
                Context.Stocks[savedEntityIndex] = entity;
            }

            return Task.CompletedTask;
        }
    }
}