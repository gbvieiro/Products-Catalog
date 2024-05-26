﻿using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.RepositoriesInterfaces.Common;

namespace Products.Catalog.Domain.RepositoriesInterfaces
{
    public interface IBookRepository : IRepository<Book, Guid> 
    {
    }
}