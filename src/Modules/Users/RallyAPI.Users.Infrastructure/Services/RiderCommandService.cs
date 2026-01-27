using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RallyAPI.SharedKernel.Abstractions.Riders;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Infrastructure.Persistence;

namespace RallyAPI.Users.Infrastructure.Services;

/// <summary>
/// Implementation of IRiderCommandService.
/// Modifies rider state for the Delivery module.
/// </summary>
public sealed class RiderCommandService : IRiderCommandService
{
    private readonly UsersDbContext _dbContext;
    private readonly ILogger<RiderCommandService> _logger;

    public RiderCommandService(
        UsersDbContext dbContext,
        ILogger<RiderCommandService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result> AssignDeliveryToRiderAsync(
        Guid riderId,
        Guid deliveryRequestId,
        CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Assigning delivery {DeliveryId} to rider {RiderId}",
            deliveryRequestId, riderId);

        var rider = await _dbContext.Riders
            .FirstOrDefaultAsync(r => r.Id == riderId, ct);

        if (rider is null)
        {
            _logger.LogWarning("Rider {RiderId} not found", riderId);
            return Result.Failure(Error.NotFound("Rider", riderId));
        }

        var result = rider.AssignDelivery(deliveryRequestId);

        if (result.IsFailure)
        {
            _logger.LogWarning(
                "Failed to assign delivery {DeliveryId} to rider {RiderId}: {Error}",
                deliveryRequestId, riderId, result.Error.Message);
            return result;
        }

        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Successfully assigned delivery {DeliveryId} to rider {RiderId}",
            deliveryRequestId, riderId);

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> ClearRiderDeliveryAsync(
        Guid riderId,
        Guid deliveryRequestId,
        CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Clearing delivery {DeliveryId} from rider {RiderId}",
            deliveryRequestId, riderId);

        var rider = await _dbContext.Riders
            .FirstOrDefaultAsync(r => r.Id == riderId, ct);

        if (rider is null)
        {
            _logger.LogWarning("Rider {RiderId} not found", riderId);
            return Result.Failure(Error.NotFound("Rider", riderId));
        }

        var result = rider.ClearDelivery(deliveryRequestId);

        if (result.IsFailure)
        {
            _logger.LogWarning(
                "Failed to clear delivery {DeliveryId} from rider {RiderId}: {Error}",
                deliveryRequestId, riderId, result.Error.Message);
            return result;
        }

        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Successfully cleared delivery {DeliveryId} from rider {RiderId}",
            deliveryRequestId, riderId);

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> UpdateRiderLocationAsync(
        Guid riderId,
        double latitude,
        double longitude,
        CancellationToken ct = default)
    {
        _logger.LogDebug(
            "Updating location for rider {RiderId}: ({Lat}, {Lng})",
            riderId, latitude, longitude);

        var rider = await _dbContext.Riders
            .FirstOrDefaultAsync(r => r.Id == riderId, ct);

        if (rider is null)
        {
            _logger.LogWarning("Rider {RiderId} not found", riderId);
             return Result.Failure(Error.NotFound("Rider", riderId));
        }

        var result = rider.UpdateLocation((decimal)latitude, (decimal)longitude);

        if (result.IsFailure)
        {
            _logger.LogWarning(
                "Failed to update location for rider {RiderId}: {Error}",
                riderId, result.Error.Message);
            return result;
        }

        await _dbContext.SaveChangesAsync(ct);

        _logger.LogDebug(
            "Successfully updated location for rider {RiderId}",
            riderId);

        return Result.Success();
    }
}