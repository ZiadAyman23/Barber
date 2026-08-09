using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructer.Confegrations
{
    public class SalarieConf : IEntityTypeConfiguration<Salaries>
    {
        public void Configure(EntityTypeBuilder<Salaries> builder)
        {
            builder.ToTable("Salaries");
            builder.HasIndex(s => s.BarberID);
            builder.HasKey(s => s.Id);
            builder.Property(s => s.SalaryAmount)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");
            builder.Property(s => s.EffectiveDate) .IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(s => s.StartDate).IsRequired();
            builder.Property(s => s.BarberID).IsRequired();
            builder.Property(s => s.DeductionAmount).IsRequired().HasColumnType("decimal(18, 2)");
            builder.Property(s => s.TradeAmount).IsRequired().HasColumnType("decimal(18, 2)");
            builder.HasOne(s => s.Barber)
                .WithMany(b => b.Salaries)
                .HasForeignKey(s => s.BarberID)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
