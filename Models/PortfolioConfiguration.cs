using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Models
{
    public class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
    {
        public void Configure(EntityTypeBuilder<Portfolio> builder)
        {
            builder.HasKey(p => new { p.AppUserId, p.StockId });
            builder.HasOne(p => p.Stock)
                    .WithMany(s => s.Portfolios)
                    .HasForeignKey(p => p.StockId);
            builder.HasOne(p => p.AppUser)
                    .WithMany(s => s.Portfolios)
                    .HasForeignKey(p => p.AppUserId);
        }
    }
}