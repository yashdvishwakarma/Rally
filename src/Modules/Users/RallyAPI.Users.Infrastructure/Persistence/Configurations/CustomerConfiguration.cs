using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RallyAPI.Users.Domain.Entities;
using RallyAPI.Users.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RallyAPI.Users.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Table mapping
            builder.ToTable("customers");

            // Primary Key
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedNever(); // We generate GUIDs in domain

            // Phone - Value Object conversion
            builder.Property(c => c.Phone)
                .HasConversion(
                    phone => phone.Value,                      // C# → DB
                    value => PhoneNumber.Create(value).Value   // DB → C#
                )
                .HasColumnName("phone")
                .HasMaxLength(15)
                .IsRequired();

            // Name
            builder.Property(c => c.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired(false);

            // Email - Value Object conversion (nullable)
            builder.Property(c => c.Email)
                .HasConversion(
                    email => email == null ? null : email.Value,  // C# → DB
                    value => value == null ? null : Email.Create(value).Value  // DB → C#
                )
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired(false);

            // IsActive
            builder.Property(c => c.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .IsRequired();

            // Base Entity properties
            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            builder.Property(c => c.DeletedAt)
                .HasColumnName("deleted_at")
                .IsRequired(false);

            // Version for optimistic concurrency
            builder.Property(c => c.Version)
                .HasColumnName("version")
                .IsConcurrencyToken();

            // Relationship - One Customer has Many Addresses
            builder.HasMany(c => c.Addresses)
                .WithOne()
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(c => c.Phone)
                .IsUnique()
                .HasDatabaseName("idx_customers_phone");

            // Query Filter - Soft delete (exclude deleted records by default)
            builder.HasQueryFilter(c => c.DeletedAt == null);

            // Ignore domain events (not persisted)
            builder.Ignore(c => c.DomainEvents);
        }
    }
}
