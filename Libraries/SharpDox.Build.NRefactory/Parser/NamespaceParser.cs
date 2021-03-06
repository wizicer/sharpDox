﻿using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using SharpDox.Build.NRefactory.Loader;
using SharpDox.Model.Repository;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SharpDox.Build.NRefactory.Parser
{
    internal class NamespaceParser : BaseParser
    {
        private readonly List<string> _descriptionFiles;

        internal NamespaceParser(SDRepository repository, List<string> excludedIdentifiers, string inputFile) : base(repository, excludedIdentifiers)
        {
            _descriptionFiles = Directory.EnumerateFiles(Path.GetDirectoryName(inputFile), "*.sdnd", SearchOption.AllDirectories).ToList();
        }

        internal void ParseProjectNamespaces(CSharpProject project)
        {
            var types = project.Compilation.MainAssembly.TopLevelTypeDefinitions.ToList();
            for (int i = 0; i < types.Count; i++)
            {
                HandleOnItemParseStart(types[i].Namespace, i, types.Count);
                if (!_excludedIdentifiers.Contains(types[i].Namespace))
                {
                    _repository.AddNamespace(GetParsedNamespace(types[i]));
                }
            }
        }

        internal SDNamespace GetParsedNamespace(IType type)
        {
            var descriptionFiles = _descriptionFiles.Where(d => Path.GetFileName(d).ToLower().Contains(type.Namespace.ToLower() + ".sdnd"));

            var descriptions = new Dictionary<string, string>();
            foreach (var file in descriptionFiles)
            {
                var splitted = Path.GetFileName(file).ToLower().Replace(type.Namespace.ToLower(), " ").Split('.');
                if (splitted.Length > 0 && splitted[0].Length == 2 && CultureInfo.GetCultures(CultureTypes.AllCultures).Any(c => c.TwoLetterISOLanguageName == splitted[0]))
                {
                    descriptions.Add(splitted[0], File.ReadAllText(file));
                    ExecuteOnDocLanguageFound(splitted[0].ToLower());
                }
                else if (splitted.Length > 0 && string.IsNullOrEmpty(splitted[0].Trim()))
                {
                    descriptions.Add("default", File.ReadAllText(file));
                }
            }

            return new SDNamespace(type.Namespace)
            {
                Assemblyname = ((DefaultResolvedTypeDefinition)type).ParentAssembly.FullAssemblyName,
                Description = descriptions
            };
        }
    }
}