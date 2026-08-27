using MediatR;
using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Stocks.Commands.UpdateStock;

/// <summary>
/// Ajuste administrativo: define a quantidade em estoque de um livro para um
/// valor absoluto (diferente de AddStock, que soma um delta ao estoque
/// existente - ver Stock.SetQuantity no dominio).
/// </summary>
public sealed record UpdateStockCommand(Guid BookId, int Quantity) : ICommand<Unit>;
