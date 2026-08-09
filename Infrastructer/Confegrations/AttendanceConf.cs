namespace Infrastructer.Confegrations
{
    public class AttendanceConf : IEntityTypeConfiguration<Attendance>
    {
        void IEntityTypeConfiguration<Attendance>.Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendance");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.Date, x.BarberId });
            builder.Property(x => x.Date).IsRequired(true);
            builder.Property(x => x.IsAddent).IsRequired(true);
            builder.HasOne(x => x.Barber).WithMany(x => x.Attendances).HasForeignKey(x => x.BarberId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
