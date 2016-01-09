using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Gumbo.DefGen
{
    class Program
    {
        /// <summary>
        /// This is a .DEF file generator to produce a .DLL with exports.
        /// Without it PInvokes won't work.
        /// Setps:
        /// Build .LIB. 
        /// Put it in a project. 
        /// Generate .DEF file. 
        /// Set .DEF file path in Project properties -> Linker -> Input -> Module Definition File.
        /// Build .DLL instead of .LIB.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("PATH",
                ConfigurationManager.AppSettings["VCBinPath"]);

            string libFilePath = ConfigurationManager.AppSettings["LibFilePath"];
            string libName = ConfigurationManager.AppSettings["LibName"];
            string outputDefFilePath = ConfigurationManager.AppSettings["OutputDefFilePath"];


            var exportedNames = GetExportableNames(libFilePath);
            GenerateDefinitionFile(libName, outputDefFilePath, exportedNames);
        }

        private static IEnumerable<string> GetExportableNames(string libFilePath)
        {
            string linkermemberFileName = Path.GetTempFileName();
            Process.Start("dumpbin.exe",
                String.Format(@"/LINKERMEMBER:2 /OUT:""{0}"" ""{1}""", linkermemberFileName, libFilePath)
                ).WaitForExit();

            string[] lines = File.ReadAllLines(linkermemberFileName);
            File.Delete(linkermemberFileName);
            return lines
                .SkipWhile(x => !x.Contains("public symbols"))
                .Skip(2)
                .TakeWhile(x => !String.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().Split(' '))
                .Select(x => new { Address = x[0], Name = x[1].Substring(1) })
                .Where(x => !x.Name.StartsWith("?"))
                .Select(x => x.Name)
                .ToList();
        }

        private static void GenerateDefinitionFile(string libName,
            string outputDefFilePath, IEnumerable<string> exportedNames)
        {
            var defs = new List<string>();
            defs.Add("LIBRARY   " + libName);
            defs.Add("EXPORTS");
            defs.AddRange(exportedNames.Select((x, i) => String.Format("   {0} @{1}", x, i + 1)));

            File.WriteAllLines(outputDefFilePath, defs);
        }
    }
}
