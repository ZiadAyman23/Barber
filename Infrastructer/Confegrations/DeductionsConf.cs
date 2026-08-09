namespace Infrastructer.Confegrations
{
    public class DeductionsConf : IEntityTypeConfiguration<Deductions>
    {
        public void Configure(EntityTypeBuilder<Deductions> builder)
        {
            builder.ToTable("Deductions");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.Date, x.BarberId });
            builder.Property(x => x.Date).IsRequired(true).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.Amount).IsRequired(true);
            builder.Property(x => x.Reason).HasMaxLength(200).IsRequired(true);
            builder.Property(x => x.CreatedBy).HasMaxLength(50).IsRequired(true);
            builder.HasOne(x => x.Barber).WithMany(x => x.Deductions).HasForeignKey(x => x.BarberId).OnDelete(DeleteBehavior.Cascade);
        }

    }
}
