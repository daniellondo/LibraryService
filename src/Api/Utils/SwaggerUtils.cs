namespace Api.Utils
{
    using Domain.Dtos;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    public class SwaggerUtils
    {
        public class BookSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema schema, SchemaFilterContext context)
            {
                if (context.Type == typeof(AddBookCommand))
                {
                    schema.Example = new OpenApiObject()
                    {
                        ["isbn"] = new OpenApiLong(9780062316097),
                        ["editorial"] = new OpenApiObject()
                        {
                            ["name"] = new OpenApiString("HarperCollins"),
                            ["branch"] = new OpenApiString("New York")
                        },
                        ["title"] = new OpenApiString("Good Omens"),
                        ["synopsis"] = new OpenApiString("Good Omens is a hilarious and satirical fantasy novel that tells the story of an angel and a demon who form an unlikely alliance to prevent the apocalypse. With their unique perspectives on humanity, they navigate through various supernatural and comedic adventures."),
                        ["pages"] = new OpenApiInteger(432),
                        ["authors"] = new OpenApiArray()
                        {
                            new OpenApiObject()
                            {
                                ["name"] = new OpenApiString("Terry"),
                                ["surname"] = new OpenApiString("Pratchett")
                            },
                            new OpenApiObject()
                            {
                                ["name"] = new OpenApiString("Neil"),
                                ["surname"] = new OpenApiString("Gaiman")
                            }
                        }
                    };
                }
            }
        }
    }
}
