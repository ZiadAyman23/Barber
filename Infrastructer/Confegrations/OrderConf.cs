namespace Infrastructer.Confegrations
{
    public class OrderConf : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(t => t.Id);
            builder.HasIndex(x => new { x.BarberId, x.Date });
            builder.Property(x => x.TotalPrice).IsRequired(true);
            builder.Property(x => x.Date).IsRequired(true).HasDefaultValueSql("GETDATE()");
            builder.HasOne(x => x.Barber).WithMany(x => x.Orders).HasForeignKey(x => x.BarberId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
