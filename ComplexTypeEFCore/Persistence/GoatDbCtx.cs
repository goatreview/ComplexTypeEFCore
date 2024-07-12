using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplexTypeEFCore.Persistence;

public class GoatDbCtx : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Dev\\ComplexTypeEFCore\\ComplexTypeEFCore\\goatdb_test.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new GoatConfiguration());
        modelBuilder.ApplyConfiguration(new GoatJsonConfiguration());
    }
}

public class GoatConfiguration : IEntityTypeConfiguration<Goat>
{
    public void Configure(EntityTypeBuilder<Goat> builder)
    {
        builder.ToTable("Goats");
        builder.HasKey(g => g.Id);
    }
}

public class Goat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public required Field Field { get; set; }
}

[ComplexType]
public class Field
{
    public string Street { get; set; }
    public string City { get; set; }
    // public double Size { get; set; }
    public double Hectare { get; set; }
}

public class GoatJsonConfiguration : IEntityTypeConfiguration<GoatJson>
{
    public void Configure(EntityTypeBuilder<GoatJson> builder)
    {
        builder.ToTable("GoatsJson");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Field)
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                v => JsonSerializer.Deserialize<FieldJson>(v, new JsonSerializerOptions()))
            .HasColumnType("json"); 
    }
}

public class GoatJson
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public FieldJson Field { get; set; }
}

public class FieldJson
{
    public string Street { get; set; }
    public string City { get; set; }
    // public double Size { get; set; }
    public double Hectare { get; set; }
}