namespace Infrastructer.Confegrations
{
    public class BarberConf : IEntityTypeConfiguration<Barber>
    {
        public void Configure(EntityTypeBuilder<Barber> builder)
        {
            builder.ToTable("Barbers");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired(true);
            builder.Property(x => x.HisPercent).IsRequired(false);
            builder.Property(x => x.DailyPrice).IsRequired(false);
            builder.Property(x => x.StartDate).HasDefaultValueSql("getdate()").IsRequired(true);

        }

    }
}
