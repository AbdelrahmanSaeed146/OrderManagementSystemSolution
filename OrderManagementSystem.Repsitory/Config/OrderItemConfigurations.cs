using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repsitory.Config
{
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.HasOne(o => o.Product).WithMany(p => p.Items).HasForeignKey(o => o.ProductId);

            builder.Property(O => O.UnitPrice)
             .HasColumnType("decimal(12,2)");

    
        }
    }
}
