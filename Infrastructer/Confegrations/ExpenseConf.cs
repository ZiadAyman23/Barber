namespace Infrastructer.Confegrations
{
    public class ExpenseConf : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expense");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Date);
            builder.Property(x => x.Date).IsRequired(true).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.Amount).IsRequired(true);
            builder.Property(x => x.Reason).HasMaxLength(200).IsRequired(true);
            builder.Property(x => x.CreatedBy).HasMaxLength(50).IsRequired(true);
        }

    }
}
