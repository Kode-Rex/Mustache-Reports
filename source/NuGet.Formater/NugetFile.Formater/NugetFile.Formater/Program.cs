using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NugetFile.Formater
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileLocation = @"C:\Users\travis\.nuget\packages\mustache.reports.data\1.1.1\mustache.reports.data.nuspec";
            var fileoutput = @"D:\Systems\Mustache-Reports-Example\nuget-packages\.targets.txt";
            var data = File.ReadAllText(fileLocation);

            var itemGroupTemplate = "<ItemGroup> <Content Include=\"$(MSBuildThisFileDirectory)..\\..\\content\\{0}\"><CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory><Link>{0}</Link><Visible>false</Visible></Content></ItemGroup>";

            Regex regex = new Regex(@"(?<=\binclude="")[^""]*");
            Match match = regex.Match(data);
            do
            {
                if (match.Success)
                {
                    var value = match.Value;
                    value = value.Replace("any/netcoreapp2.0/", "");
                    value = value.Replace("/", "\\");

                    var output = string.Format(itemGroupTemplate, value);

                    File.AppendAllText(fileoutput, output);
                    File.AppendAllText(fileoutput, Environment.NewLine);
                }
                match = match.NextMatch();
            } while (match.Length > 1);
        }
    }
}
