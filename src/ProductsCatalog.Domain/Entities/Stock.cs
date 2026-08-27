using ProductsCatalog.Domain.Common;
using ProductsCatalog.Domain.Exceptions;

namespace ProductsCatalog.Domain.Entities;

public class Stock : BaseEntity
{
    public Guid BookId { get; private set; }
    public int Quantity { get; private set; }

    protected Stock()
    {
    }

    public Stock(Guid bookId, int quantity)
    {
        BookId = bookId;
        Quantity = quantity;

        Validate();
    }

    private void Validate()
    {
        DomainException.When(BookId == Guid.Empty, "A book id is required.");
        DomainException.When(Quantity < 0, "Stock quantity cannot be negative.");
    }

    /// <summary>Reserva itens do estoque (ex: ao criar um pedido).</summary>
    public void Reserve(int quantity)
    {
        DomainException.When(quantity <= 0, "Quantity to reserve must be greater than 0.");
        DomainException.When(quantity > Quantity, $"Not enough stock. Available: {Quantity}.");

        Quantity -= quantity;
        Touch();
    }

    /// <summary>Repoe itens no estoque (ex: recebimento de fornecedor ou cancelamento de pedido).</summary>
    public void Replenish(int quantity)
    {
        DomainException.When(quantity <= 0, "Quantity to add must be greater than 0.");

        Quantity += quantity;
        Touch();
    }

    /// <summary>
    /// Define a quantidade em estoque para um valor absoluto (ex: correcao/
    /// ajuste manual de inventario). Diferente de Reserve/Replenish, que
    /// representam movimentacoes incrementais de fluxos de negocio (pedido,
    /// recebimento de fornecedor), este e o "editar" administrativo usado
    /// pela tela de CRUD de estoque.
    /// </summary>
    public void SetQuantity(int quantity)
    {
        DomainException.When(quantity < 0, "Stock quantity cannot be negative.");

        Quantity = quantity;
        Touch();
    }
}
