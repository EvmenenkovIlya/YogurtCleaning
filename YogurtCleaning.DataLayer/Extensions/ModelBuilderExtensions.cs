using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace YogurtCleaning.DataLayer.Extensions;

public static class ModelBuilderExtensions
{
    public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableNameAnnotation = (ConventionAnnotation)entity.FindAnnotation(RelationalAnnotationNames.TableName);
            var configurationSource = tableNameAnnotation.GetConfigurationSource();
            if (configurationSource != ConfigurationSource.Convention)
            {
                // Set explicitly using Fluent API or has TableAttribute DataAnnotation
                continue;
            }
            var defaultTableName = entity.GetDefaultTableName();
            entity.SetTableName(defaultTableName);
        }
    }
}