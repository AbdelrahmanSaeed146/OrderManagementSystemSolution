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
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {


            builder.Property(o => o.Status)
                   .HasConversion
                   (
                        (OStatus) => OStatus.ToString(),
                        (OStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
                   );

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders);

            builder.Property(O => O.TotalAmount)
             .HasColumnType("decimal(12,2)");

        }
    }
}
