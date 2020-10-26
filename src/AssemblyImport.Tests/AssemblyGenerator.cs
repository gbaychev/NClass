using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AssemblyImport.Tests
{
    public static class AssemblyGenerator
    {
        public static Assembly Generate(string sourceFile)
        {
            var code = ReadFile(sourceFile);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var assemblyName = Path.GetFileNameWithoutExtension(sourceFile);
            var assemblyPath = Path.Combine(Path.GetTempPath(), $"{assemblyName}.dll");
            
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            if (File.Exists(assemblyPath))
            {
                File.Delete(assemblyPath);
            }

            using (var stream = File.Open(assemblyPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var compilation = CSharpCompilation.Create(
                    assemblyName,
                    syntaxTrees: new[] { syntaxTree },
                    references: references,
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
                
                var result = compilation.Emit(stream);
                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic => 
                            diagnostic.IsWarningAsError || 
                            diagnostic.Severity == DiagnosticSeverity.Error)
                        .Select(diagnostic => new Exception($"{diagnostic.Id}: {diagnostic.GetMessage()}"))
                        .ToArray();

                    throw new AggregateException(failures);
                }
            }
            
            return Assembly.LoadFile(assemblyPath);
        }

        private static string ReadFile(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var baseDirectory = Path.GetDirectoryName(assembly.Location);
            var path = Path.Combine(baseDirectory, "Resources", fileName);
            
            return File.ReadAllText(path);
        }
    }
}
