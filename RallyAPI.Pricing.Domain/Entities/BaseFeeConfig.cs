
using RallyAPI.SharedKernel.Domain;

namespace RallyAPI.Pricing.Domain.Entities;

public class BaseFeeConfig : BaseEntity
{
    public decimal Amount { get; private set; }
    public decimal? MinimumFee { get; private set; }
    public decimal? MaximumFee { get; private set; }
    public bool IsActive { get; private set; }

    private BaseFeeConfig() { }

    public static BaseFeeConfig Create(
        decimal amount,
        decimal? minFee = null,
        decimal? maxFee = null)
    {
        return new BaseFeeConfig
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            MinimumFee = minFee,
            MaximumFee = maxFee,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}