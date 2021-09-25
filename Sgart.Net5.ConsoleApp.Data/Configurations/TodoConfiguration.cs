using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sgart.Net5.ConsoleApp.BO;
using Sgart.Net5.ConsoleApp.BO.Entities;
using System.Text.Json;

namespace Sgart.Net5.ConsoleApp.Data.Configurations
{
    // la classe va registrata in AppDbContext.OnModelCreating
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            // definisco come il campo DataJson verrà serializzato e deserializzato in Json 
            // usando il nuovo e più efficente System.Text.Json in alternativa al precedente Newtonsoft
            builder.Property(e => e.DataJson)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, Helpers.HelperJson.GetJsonOtions()),
                    v => JsonSerializer.Deserialize<TodoData>(v, Helpers.HelperJson.GetJsonOtions())
                );
        }
    }
}