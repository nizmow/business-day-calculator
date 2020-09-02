using System;
using System.IO;
using System.Reflection;

namespace BusinessDaysBetween.Business.Tests.TestHelpers
{
    public static class ResourceHelpers
    {
        public static string ReadEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new InvalidOperationException($"Unable to find requested resource {resourceName}");
            }
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}