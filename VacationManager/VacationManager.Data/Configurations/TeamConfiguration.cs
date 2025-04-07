using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Models;
using VacationManager.Data.Configurations.Extentions;

namespace VacationManager.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ConfigureEntity();

        builder.HasOne(t => t.Project)
               .WithMany(p => p.Teams)
               .HasForeignKey(t => t.ProjectId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.User)
               .WithMany()
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.TeamLead)
                .WithMany()
                .HasForeignKey(t => t.TeamLeadId)
                .OnDelete(DeleteBehavior.Restrict);



    }
}
