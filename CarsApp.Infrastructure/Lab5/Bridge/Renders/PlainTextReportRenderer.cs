using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Bridge;

namespace CarsApp.Infrastructure.Lab5.Bridge.Renderers;

// Renderer pentru fișiere .txt / arhivă — compact, fără simboluri
public class PlainTextReportRenderer : IReportRenderer
{
    public string RendererName => "Plain Text (Fișier .txt)";

    public string RenderTitle(string title)
        => $"[{title}]";

    public string RenderSection(string sectionName, IReadOnlyList<string> lines)
    {
        var content = string.Join("; ", lines);
        return $"{sectionName}: {content}";
    }

    public string RenderKeyValue(string key, string value)
        => $"{key}={value}";

    public string RenderSeparator()
        => "|";

    public string Finalize(IReadOnlyList<string> parts)
        => string.Join(" | ", parts.Where(p => p != "|"));
}