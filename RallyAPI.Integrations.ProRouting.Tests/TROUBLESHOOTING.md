# RallyAPI E2E Troubleshooting Guide

The 7 most common failures you'll hit during the E2E test, with diagnostic queries and fixes.

---

## 1. "Handler not found" / Event handler never fires

**Symptom:** You confirm an order but no DeliveryRequest is created. Or delivery events fire but Order status never updates.

**Diagnosis:**
```sql
-- Check if DeliveryRequest was created after order confirmation
SELECT * FROM delivery.delivery_requests 
WHERE order_id = '{orderId}';
-- If empty → the bridge handler (Phase 1) isn't firing
```

**Fix checklist:**
- Verify `AddMediatR()` scans the correct assemblies:
  ```csharp
  // In your DI setup, ensure BOTH are scanned:
  cfg.RegisterServicesFromAssembly(typeof(OrderConfirmedDomainEventHandler).Assembly);
  cfg.RegisterServicesFromAssembly(typeof(DeliveryRiderAssignedEventHandler).Assembly);
  ```
- Confirm the handler class implements `INotificationHandler<T>` (not `IRequestHandler`)
- Confirm the event class implements `INotification`
- Add a breakpoint or log at the FIRST line of the handler

---

## 2. Connection string mismatch

**Symptom:** `Npgsql.NpgsqlException: connection refused` or `InvalidOperationException: No database provider`

**Diagnosis:**
```bash
# Search your codebase for all connection string keys
grep -r "GetConnectionString" --include="*.cs" .
```

**Fix:**
- Pick ONE key (recommend `"Database"`)
- Update all modules to use it
- Verify `appsettings.Development.json`:
  ```json
  {
    "ConnectionStrings": {
      "Database": "Host=localhost;Port=5432;Database=rallydb;Username=postgres;Password=yourpassword"
    }
  }
  ```

---

## 3. Order state machine rejection

**Symptom:** `InvalidOperationException: Cannot transition from X to Y` or a `Result.Failure` with validation error

**Diagnosis:**
```sql
-- Check current order status
SELECT id, status, updated_at FROM orders.orders WHERE id = '{orderId}';
```

**The valid state transitions are:**
```
Placed → Confirmed → ReadyForPickup → PickedUp → Delivered
Placed → Cancelled (customer cancels)
Placed → Rejected (restaurant rejects)
Any → Failed (delivery failure)
```

**Common mistakes:**
- Trying to mark PickedUp before the order is Confirmed + ReadyForPickup
- Trying to confirm after the customer already cancelled
- Skipping the "ReadyForPickup" step (Step 15 in the .http file)

---

## 4. Rider not found during dispatch

**Symptom:** Dispatch returns error or falls back to 3PL (ProRouting) instead of assigning your test rider

**Diagnosis:**
```sql
-- Is the rider online, KYC'd, and available?
SELECT id, name, is_online, is_kyc_approved, is_on_delivery,
       current_latitude, current_longitude
FROM users.riders  -- or delivery.riders depending on your schema
WHERE id = '00000000-0000-0000-0000-000000000050';

-- Check dispatch configuration
-- Default search radius is usually 3-5km
-- Rider at (12.9370, 77.6250) is ~200m from restaurant at (12.9352, 77.6245)
```

**Fix:**
- Make sure rider `is_online = true`
- Make sure `is_kyc_approved = true`
- Make sure `is_on_delivery = false`
- Make sure rider coordinates are within the configured `DispatchOptions.SearchRadiusKm`
- Check which table the `RiderDispatchOrchestrator` queries — it might be `delivery.riders` not `users.riders`

---

## 5. Integration event missing properties

**Symptom:** `NullReferenceException` in event handler, or Order updates with null rider name/phone

**Diagnosis:**
```sql
-- Check if rider info was saved on the order
SELECT id, status, rider_id, rider_name, rider_phone 
FROM orders.orders WHERE id = '{orderId}';
-- If rider_id is set but rider_name is NULL → the integration event doesn't carry those fields
```

**Fix:**
Check `DeliveryRiderAssignedIntegrationEvent` in SharedKernel:
```csharp
// It MUST have these properties:
public Guid OrderId { get; init; }
public Guid RiderId { get; init; }
public string RiderName { get; init; }    // ← Often missing!
public string RiderPhone { get; init; }   // ← Often missing!
```

If missing, add them and update the publisher in Delivery module to populate them.

---

## 6. DomainEventInterceptor not wired up

**Symptom:** Domain events are raised (e.g., `Order.Confirm()` calls `RaiseDomainEvent()`) but nothing happens — no handlers fire.

**Diagnosis:**
Add this temporary log to `DomainEventInterceptor.SavedChangesAsync()`:
```csharp
_logger.LogWarning("DomainEventInterceptor: Publishing {Count} events", domainEvents.Count);
```

If you never see this log, the interceptor isn't registered.

**Fix:**
```csharp
// In each module's Infrastructure DI registration:
services.AddDbContext<OrdersDbContext>((sp, options) =>
{
    options.UseNpgsql(connectionString);
    options.AddInterceptors(sp.GetRequiredService<DomainEventInterceptor>());
    //                      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    //                      This line is critical!
});
```

Verify this exists for **every** module's DbContext (Orders, Delivery, Catalog).

---

## 7. Unit of Work / SaveChanges not called

**Symptom:** Handler runs (you can see logs) but DB doesn't update. Order status stays the same.

**Diagnosis:**
```sql
-- Check if status changed
SELECT id, status, updated_at FROM orders.orders WHERE id = '{orderId}';
-- If updated_at hasn't changed → SaveChanges was never called
```

**Fix:**
Check your event handlers. They MUST call SaveChanges:
```csharp
public async Task Handle(DeliveryCompletedIntegrationEvent notification, CancellationToken ct)
{
    var order = await _orderRepository.GetByIdAsync(notification.OrderId, ct);
    if (order is null) return;
    
    order.MarkDelivered();
    await _unitOfWork.SaveChangesAsync(ct);  // ← Don't forget this!
}
```

Also check: does your Orders module use `IUnitOfWork` or does the repository handle saves directly? Match whatever pattern the existing `ConfirmOrderCommandHandler` uses.

---

## Quick Diagnostic SQL Cheat Sheet

```sql
-- Full order lifecycle audit
SELECT id, status, rider_id, rider_name, created_at, updated_at
FROM orders.orders ORDER BY created_at DESC LIMIT 5;

-- All delivery requests
SELECT id, order_id, status, rider_id, created_at
FROM delivery.delivery_requests ORDER BY created_at DESC LIMIT 5;

-- Domain events published (if you have an outbox table)
SELECT * FROM shared.outbox_messages ORDER BY created_at DESC LIMIT 10;

-- Check all test users exist
SELECT id, name, role, phone_number FROM users.users 
WHERE phone_number LIKE '+91987650%' OR phone_number = '+919876543210';

-- Rider status
SELECT * FROM users.riders WHERE id = '00000000-0000-0000-0000-000000000050';

-- Menu items available
SELECT mi.id, mi.name, mi.price, mi.is_available 
FROM catalog.menu_items mi
JOIN catalog.menus m ON mi.menu_id = m.id
WHERE m.restaurant_id = '00000000-0000-0000-0000-000000000010';
```
