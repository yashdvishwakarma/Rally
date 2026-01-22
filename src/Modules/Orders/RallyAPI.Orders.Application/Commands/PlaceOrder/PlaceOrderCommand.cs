using MediatR;
using RallyAPI.Orders.Application.DTOs;
using RallyAPI.Orders.Application.DTOs.Requests;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Orders.Application.Commands.PlaceOrder;

/// <summary>
/// Command to create a paid order.
/// Called AFTER payment is successful.
/// </summary>
public sealed record PlaceOrderCommand : IRequest<Result<OrderDto>>
{
    // Customer info
    public Guid CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string? CustomerPhone { get; init; }
    public string? CustomerEmail { get; init; }

    // Payment info (REQUIRED - order is already paid)
    public string PaymentId { get; init; } = string.Empty;
    public string? PaymentTransactionId { get; init; }

    // Delivery quote (from Delivery Module)
    public string? DeliveryQuoteId { get; init; }

    // Request data
    public PlaceOrderRequest Request { get; init; } = new();

    public static PlaceOrderCommand Create(
        Guid customerId,
        string customerName,
        string paymentId,
        PlaceOrderRequest request,
        string? paymentTransactionId = null,
        string? deliveryQuoteId = null,
        string? customerPhone = null,
        string? customerEmail = null)
    {
        return new PlaceOrderCommand
        {
            CustomerId = customerId,
            CustomerName = customerName,
            PaymentId = paymentId,
            PaymentTransactionId = paymentTransactionId,
            DeliveryQuoteId = deliveryQuoteId,
            CustomerPhone = customerPhone,
            CustomerEmail = customerEmail,
            Request = request
        };
    }
}