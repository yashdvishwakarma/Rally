namespace RallyAPI.Orders.Domain.Enums;

/// <summary>
/// Represents the payment status of an order.
/// Kept simple for MVP - expand as payment integration grows.
/// </summary>
public enum PaymentStatus
{
    /// <summary>Payment not yet initiated</summary>
    Pending = 0,

    /// <summary>Payment is being processed</summary>
    Processing = 5,

    /// <summary>Payment completed successfully</summary>
    Paid = 10,

    /// <summary>Payment failed</summary>
    Failed = 20,

    /// <summary>Payment refund initiated</summary>
    RefundInitiated = 30,

    /// <summary>Partial refund completed</summary>
    PartiallyRefunded = 35,

    /// <summary>Full refund completed</summary>
    Refunded = 40
}

public static class PaymentStatusExtensions
{
    public static bool IsCompleted(this PaymentStatus status) =>
        status == PaymentStatus.Paid;

    public static bool IsFailed(this PaymentStatus status) =>
        status == PaymentStatus.Failed;

    public static bool IsRefundable(this PaymentStatus status) =>
        status == PaymentStatus.Paid;

    public static string GetDisplayName(this PaymentStatus status) => status switch
    {
        PaymentStatus.Pending => "Pending",
        PaymentStatus.Processing => "Processing",
        PaymentStatus.Paid => "Paid",
        PaymentStatus.Failed => "Failed",
        PaymentStatus.RefundInitiated => "Refund Initiated",
        PaymentStatus.PartiallyRefunded => "Partially Refunded",
        PaymentStatus.Refunded => "Refunded",
        _ => "Unknown"
    };
}