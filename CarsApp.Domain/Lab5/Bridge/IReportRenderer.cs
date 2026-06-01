using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Domain.Lab5.Bridge;

// IMPLEMENTOR — definește cum se randează, nu ce se randează
// Schimbarea acestuia nu afectează deloc ierarhia de rapoarte
public interface IReportRenderer
{
    string RendererName { get; }
    string RenderTitle(string title);
    string RenderSection(string sectionName, IReadOnlyList<string> lines);
    string RenderKeyValue(string key, string value);
    string RenderSeparator();
    string Finalize(IReadOnlyList<string> parts); // asamblează totul
}