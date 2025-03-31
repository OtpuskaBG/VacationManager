﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Models.Abstractions;

namespace VacationManager.Data.Configurations.Extentions
{
    internal static class EntityConfigurationExtensions
    {
        internal static void ConfigureEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IEntity
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.LastModifiedAt)
                .IsRequired();

            builder.Property(e => e.IsDeleted)
                .IsRequired();

            builder.HasQueryFilter(e => !e.IsDeleted);
        }

        internal static void ConfigureUserResource<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, IUserResource
        {
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
