using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Bridge;

namespace CarsApp.Infrastructure.Lab5.Bridge.Renderers;

// Renderer pentru consolă — text simplu, fără formatare
public class ConsoleReportRenderer : IReportRenderer
{
    public string RendererName => "Consolă (Text simplu)";

    public string RenderTitle(string title)
        => $"=== {title.ToUpper()} ===";

    public string RenderSection(string sectionName, IReadOnlyList<string> lines)
    {
        var content = string.Join("\n", lines.Select(l => $"  • {l}"));
        return $"--- {sectionName} ---\n{content}";
    }

    public string RenderKeyValue(string key, string value)
        => $"  {key,-20}: {value}";

    public string RenderSeparator()
        => new string('-', 40);

    public string Finalize(IReadOnlyList<string> parts)
        => string.Join("\n", parts);
}