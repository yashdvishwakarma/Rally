-- ============================================================
-- RallyAPI — E2E Test Seed Data
-- ============================================================
-- Run this BEFORE the e2e-flow.http test script.
-- 
-- Creates:
--   4 Users: Customer, Restaurant Owner, Rider, Admin
--   1 Restaurant in Koramangala with coordinates
--   1 Menu with 3 items (Butter Chicken, Paneer Tikka, Garlic Naan)
--   1 Rider who is online + KYC-approved + near the restaurant
--
-- IMPORTANT:
--   - Adjust schema names (users, catalog, orders, delivery) 
--     to match your actual EF Core schema configuration
--   - UUIDs are deterministic so the .http file can reference them
--   - Run with: psql -d rallydb -f seed-test-data.sql
-- ============================================================

BEGIN;

-- ============================================================
-- 1. USERS (users schema)
-- ============================================================
-- Clean up any previous test data
DELETE FROM users.users WHERE phone_number IN (
    '+919876543210', '+919876500001', '+919876500002', '+919876500003'
);

-- Customer
INSERT INTO users.users (id, phone_number, name, email, role, is_verified, created_at, updated_at)
VALUES (
    '00000000-0000-0000-0000-000000000001',
    '9876543210',
    'Test Customer',
    'customer@test.rally',
    'Customer',
    true,
    NOW() AT TIME ZONE 'UTC',
    NOW() AT TIME ZONE 'UTC'
);

-- Restaurant Owner
INSERT INTO users.users (id, phone_number, name, email, role, is_verified, created_at, updated_at)
VALUES (
    '00000000-0000-0000-0000-000000000002',
    '9876500001',
    'Test Restaurant Owner',
    'restaurant@test.rally',
    'RestaurantOwner',
    true,
    NOW() AT TIME ZONE 'UTC',
    NOW() AT TIME ZONE 'UTC'
);

-- Rider
INSERT INTO users.users (id, phone_number, name, email, role, is_verified, created_at, updated_at)
VALUES (
    '00000000-0000-0000-0000-000000000003',
    '9876500002',
    'Test Rider',
    'rider@test.rally',
    'Rider',
    true,
    NOW() AT TIME ZONE 'UTC',
    NOW() AT TIME ZONE 'UTC'
);

-- Admin
INSERT INTO users.users (id, phone_number, name, email, role, is_verified, created_at, updated_at)
VALUES (
    '00000000-0000-0000-0000-000000000004',
    '9876500003',
    'Test Admin',
    'admin@test.rally',
    'Admin',
    true,
    NOW() AT TIME ZONE 'UTC',
    NOW() AT TIME ZONE 'UTC'
);


-- ============================================================
-- 2. RESTAURANT (catalog schema)
-- ============================================================
DELETE FROM catalog.restaurants WHERE id = '00000000-0000-0000-0000-000000000010';

INSERT INTO catalog.restaurants (
    id, name, description, owner_id,
    address_street, address_city, address_state, address_zip_code,
    latitude, longitude,
    phone_number, email,
    is_active, is_open,
    opening_time, closing_time,
    average_prep_time_minutes,
    created_at, updated_at
) VALUES (
    '00000000-0000-0000-0000-000000000010',
    'Spice Garden Koramangala',
    'Authentic North Indian cuisine in the heart of Koramangala',
    '00000000-0000-0000-0000-000000000002',  -- Restaurant Owner
    '4th Block, Koramangala',
    'Bengaluru',
    'Karnataka',
    '560034',
    12.9352,   -- Koramangala latitude
    77.6245,   -- Koramangala longitude
    '9876500001',
    'restaurant@test.rally',
    true,
    true,
    '09:00:00',
    '23:00:00',
    25,
    NOW() AT TIME ZONE 'UTC',
    NOW() AT TIME ZONE 'UTC'
);


-- ============================================================
-- 3. MENU (catalog schema)
-- ============================================================
DELETE FROM catalog.menus WHERE restaurant_id = '00000000-0000-0000-0000-000000000010';

INSERT INTO catalog.menus (id, restaurant_id, name, description, is_active, created_at, updated_at)
VALUES (
    '00000000-0000-0000-0000-000000000020',
    '00000000-0000-0000-0000-000000000010',
    'Main Menu',
    'Our signature dishes',
    true,
    NOW() AT TIME ZONE 'UTC',
    NOW() AT TIME ZONE 'UTC'
);


-- ============================================================
-- 4. MENU ITEMS (catalog schema)
-- ============================================================
DELETE FROM catalog.menu_items WHERE menu_id = '00000000-0000-0000-0000-000000000020';

-- Butter Chicken — ₹280
INSERT INTO catalog.menu_items (
    id, menu_id, name, description, price, currency,
    category, is_available, is_vegetarian,
    prep_time_minutes, created_at, updated_at
) VALUES (
    '00000000-0000-0000-0000-000000000031',
    '00000000-0000-0000-0000-000000000020',
    'Butter Chicken',
    'Creamy tomato-based chicken curry',
    280.00,
    'INR',
    'Main Course',
    true,
    false,
    20,
    NOW() AT TIME ZONE 'UTC',
    NOW() AT TIME ZONE 'UTC'
);

-- Paneer Tikka — ₹220
INSERT INTO catalog.menu_items (
    id, menu_id, name, description, price, currency,
    category, is_available, is_vegetarian,
    prep_time_minutes, created_at, updated_at
) VALUES (
    '00000000-0000-0000-0000-000000000032',
    '00000000-0000-0000-0000-000000000020',
    'Paneer Tikka',
    'Marinated cottage cheese grilled in tandoor',
    220.00,
    'INR',
    'Starters',
    true,
    true,
    15,
    NOW() AT TIME ZONE 'UTC',
    NOW() AT TIME ZONE 'UTC'
);

-- Garlic Naan — ₹60
INSERT INTO catalog.menu_items (
    id, menu_id, name, description, price, currency,
    category, is_available, is_vegetarian,
    prep_time_minutes, created_at, updated_at
) VALUES (
    '00000000-0000-0000-0000-000000000033',
    '00000000-0000-0000-0000-000000000020',
    'Garlic Naan',
    'Soft naan bread with garlic butter',
    60.00,
    'INR',
    'Breads',
    true,
    true,
    8,
    NOW() AT TIME ZONE 'UTC',
    NOW() AT TIME ZONE 'UTC'
);


-- ============================================================
-- 5. RIDER PROFILE (delivery or users schema)
-- ============================================================
-- The rider needs to be:
--   ✅ Online (available for delivery)
--   ✅ KYC approved
--   ✅ Located near the restaurant (within dispatch radius)
--   ✅ Not currently on another delivery

-- If riders are in the users schema:
DELETE FROM users.riders WHERE user_id = '00000000-0000-0000-0000-000000000003';

INSERT INTO users.riders (
    id, user_id, name, phone_number,
    is_online, is_kyc_approved,
    current_latitude, current_longitude,
    is_on_delivery, vehicle_type,
    created_at, updated_at
) VALUES (
    '00000000-0000-0000-0000-000000000050',
    '00000000-0000-0000-0000-000000000003',
    'Test Rider',
    '9876500002',
    true,                -- Online
    true,                -- KYC approved
    12.9370,             -- Near Koramangala (within dispatch radius)
    77.6250,
    false,               -- Not on another delivery
    'Motorcycle',
    NOW() AT TIME ZONE 'UTC',
    NOW() AT TIME ZONE 'UTC'
);

-- ============================================================
-- If riders are in the delivery schema instead, uncomment this:
-- ============================================================
-- DELETE FROM delivery.riders WHERE id = '00000000-0000-0000-0000-000000000050';
-- INSERT INTO delivery.riders (
--     id, user_id, name, phone_number,
--     is_online, is_kyc_approved,
--     current_latitude, current_longitude,
--     is_on_delivery, vehicle_type,
--     created_at, updated_at
-- ) VALUES (
--     '00000000-0000-0000-0000-000000000050',
--     '00000000-0000-0000-0000-000000000003',
--     'Test Rider',
--     '+919876500002',
--     true, true,
--     12.9370, 77.6250,
--     false, 'Motorcycle',
--     NOW() AT TIME ZONE 'UTC',
--     NOW() AT TIME ZONE 'UTC'
-- );


-- ============================================================
-- 6. CLEANUP OLD TEST ORDERS (optional safety)
-- ============================================================
-- DELETE FROM delivery.delivery_requests WHERE order_id IN (
--     SELECT id FROM orders.orders WHERE special_instructions = 'Extra spicy please'
-- );
-- DELETE FROM orders.order_items WHERE order_id IN (
--     SELECT id FROM orders.orders WHERE special_instructions = 'Extra spicy please'
-- );
-- DELETE FROM orders.orders WHERE special_instructions = 'Extra spicy please';


COMMIT;

-- ============================================================
-- VERIFICATION QUERIES — Run these to confirm seed worked
-- ============================================================
SELECT 'Users' AS entity, COUNT(*) AS count FROM users.users 
WHERE phone_number IN ('9876543210', '9876500001', '9876500002', '9876500003');

SELECT 'Restaurant' AS entity, COUNT(*) AS count FROM catalog.restaurants 
WHERE id = '00000000-0000-0000-0000-000000000010';

SELECT 'Menu Items' AS entity, COUNT(*) AS count FROM catalog.menu_items 
WHERE menu_id = '00000000-0000-0000-0000-000000000020';

-- Verify rider is online and near restaurant
SELECT name, is_online, is_kyc_approved, current_latitude, current_longitude, is_on_delivery
FROM users.riders WHERE id = '00000000-0000-0000-0000-000000000050';

-- ============================================================
-- EXPECTED OUTPUT:
--   Users:      4
--   Restaurant: 1
--   Menu Items: 3
--   Rider:      online=true, kyc=true, near Koramangala
-- ============================================================
