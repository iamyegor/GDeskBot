using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users").HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
        builder.Property(x => x.TelegramId).HasColumnName("telegram_id");
        builder.Property(x => x.TopicId).HasColumnName("topic_id").IsRequired(false);
        builder.Property(x => x.IsBanned).HasColumnName("is_banned");
        builder
            .Property(x => x.TelegramUsername)
            .HasColumnName("telegram_username")
            .IsRequired(false);
    }
}
