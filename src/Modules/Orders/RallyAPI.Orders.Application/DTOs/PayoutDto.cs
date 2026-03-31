using RallyAPI.Orders.Domain.Enums;

namespace RallyAPI.Orders.Application.DTOs;

public sealed record PayoutDto
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public DateOnly PeriodStart { get; init; }
    public DateOnly PeriodEnd { get; init; }
    public int OrderCount { get; init; }
    public decimal GrossOrderAmount { get; init; }
    public decimal TotalGstCollected { get; init; }
    public decimal TotalCommission { get; init; }
    public decimal TotalCommissionGst { get; init; }
    public decimal TotalTds { get; init; }
    public decimal NetPayoutAmount { get; init; }
    public PayoutStatus Status { get; init; }
    public string? TransactionReference { get; init; }
    public DateTime? PaidAt { get; init; }
    public string? Notes { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed record PayoutLedgerDto
{
    public Guid Id { get; init; }
    public Guid OutletId { get; init; }
    public Guid OrderId { get; init; }
    public decimal OrderAmount { get; init; }
    public decimal GstAmount { get; init; }
    public decimal CommissionPercentage { get; init; }
    public decimal CommissionAmount { get; init; }
    public decimal CommissionGst { get; init; }
    public decimal TdsAmount { get; init; }
    public decimal NetAmount { get; init; }
    public PayoutLedgerStatus Status { get; init; }
    public Guid? PayoutId { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed record EarningsSummaryDto
{
    public int OrderCount { get; init; }
    public decimal GrossRevenue { get; init; }
    public decimal TotalCommission { get; init; }
    public decimal TotalTds { get; init; }
    public decimal NetEarnings { get; init; }
    public DateOnly PeriodStart { get; init; }
    public DateOnly PeriodEnd { get; init; }
    public IReadOnlyList<PayoutLedgerDto> LedgerEntries { get; init; } = [];
}
