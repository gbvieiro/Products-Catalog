﻿using Products.Catalog.Domain.Entities.Base;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Books
{
    /// <summary>
    /// Represents a car in the product catalog.
    /// </summary>
    public class Book : Product
    {
        /// <summary>
        /// Title of the book.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Author of the book.
        /// </summary>
        public string Author { get; private set; }

        /// <summary>
        /// The genre of the book.
        /// </summary>
        public BookGenre Genre { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        /// <param name="stockQuantity"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="genre"></param>
        public Book(
            Guid id, double price, int stockQuantity,
            string title, string author, BookGenre genre
            ) : base(id, price, stockQuantity)
        {
            Title = title;
            Author = author;
            Genre = genre;

            ValidateBookDomain();
        }

        /// <summary>
        /// Define rules for a valid book.
        /// </summary>
        /// <param name="name"></param>
        private void ValidateBookDomain()
        {
            // Title
            DomainExceptionValidation.When(string.IsNullOrEmpty(Title), "title is required.");
            DomainExceptionValidation.When(Title.Length > 30, "Invalid title, too long, maximum 30 characters.");

            // Author
            DomainExceptionValidation.When(string.IsNullOrEmpty(Author), "Author is required.");
            DomainExceptionValidation.When(Author.Length < 3, "Invalid author name, too short, minimum 3 characters.");
            DomainExceptionValidation.When(Author.Length > 30, "Invalid author name, too long, maximum 3 characters.");
        }
        
        /// <summary>
        /// Reserve items of this item from stock.
        /// </summary>
        /// <param name="numberOfItems">Number of items to get.</param>
        public void ReserveItemsFromStock(int numberOfItems)
        {
            DomainExceptionValidation.When(
                numberOfItems > StockQuantity,
                $"Not enough stock of {Title}, available stock: {StockQuantity}."
            );

            StockQuantity -= numberOfItems;
        }

        public void AddItemsToStock(int numberOfItems)
        {
            StockQuantity += numberOfItems;
        }
    }
}