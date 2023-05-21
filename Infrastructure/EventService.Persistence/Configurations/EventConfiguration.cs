using EventService.Domain.Entities;
using EventService.SharedLibrary.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Persistence.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
       
        public EventConfiguration()
        {
        }

        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Event> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(EventConstants.MaxTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(EventConstants.MaxDescriptionLength);

            builder.HasIndex(x => x.UserId);
        }
    }
}
