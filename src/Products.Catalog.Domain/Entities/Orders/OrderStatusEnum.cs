﻿namespace Products.Catalog.Domain.Entities.Orders
{
    /// <summary>
    /// Define posible order status.
    /// </summary>
    public enum OrderStatusEnum
    {
        Created = 1,
        Confirmed = 2,
        Canceled = 3
    }
}