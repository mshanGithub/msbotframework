using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SchemaManifestTests
{
    public class ValidateSchemaTests
    {
        private static string GetContent(string manifesResourcetName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.EndsWith(manifesResourcetName, StringComparison.InvariantCulture));
            using (var f = new System.IO.StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                return f.ReadToEnd();
            }
        }

        private static Task<JsonSchema> GetSchema(string schemaName)
        {
            var skillManifestSchema = GetContent(schemaName);

            return JsonSchema.FromJsonAsync(skillManifestSchema, null, (x) =>
            {
                var schemaResolver = new JsonSchemaResolver(x, new JsonSchemaGeneratorSettings());
                var referenceResolver = new JsonReferenceResolver(schemaResolver);
                referenceResolver.AddDocumentReference("http://json-schema.org/draft-07/schema", JsonSchema.CreateAnySchema());

                return referenceResolver;
            });
        }

        [Theory]
        [InlineData("2._0.skill-manifest.json", "2._0.Samples.complex-skillmanifest.json")]
        [InlineData("2._0.skill-manifest.json", "2._0.Samples.echo-skillmanifest.json")]
        [InlineData("2._0.skill-manifest.json", "2._0.Samples.simple-skillmanifest.json")]
        [InlineData("2._1.skill-manifest.json", "2._1.Samples.complex-pva-manifest.json")]
        [InlineData("2._1.skill-manifest.json", "2._1.Samples.complex-skillmanifest.json")]
        [InlineData("2._1.skill-manifest.json", "2._1.Samples.echo-skillmanifest.json")]
        [InlineData("2._1.skill-manifest.json", "2._1.Samples.simple-skillmanifest.json")]
        [InlineData("2._2.skill-manifest.json", "2._2.Samples.relativeUris.complex-skillmanifest.json")]
        [InlineData("2._2.skill-manifest.json", "2._2.Samples.complex-pva-manifest.json")]
        [InlineData("2._2.skill-manifest.json", "2._2.Samples.complex-skillmanifest.json")]
        [InlineData("2._2.skill-manifest.json", "2._2.Samples.echo-skillmanifest.json")]
        [InlineData("2._2.skill-manifest.json", "2._2.Samples.simple-skillmanifest.json")]
        public async Task VerifySchemas(string schema, string manifest)
        {
            var rawManifest = JsonConvert.DeserializeObject<JObject>(GetContent(manifest));

            var schemaErrors = (await GetSchema(schema)).Validate(rawManifest);

            Assert.Empty(schemaErrors);
        }


    }
}
