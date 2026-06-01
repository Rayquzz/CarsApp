using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Lab5.Bridge;

namespace CarsApp.Infrastructure.Lab5.Bridge.Renderers;

// Renderer pentru email / web — produce HTML
public class HtmlReportRenderer : IReportRenderer
{
    public string RendererName => "HTML (Email/Web)";

    public string RenderTitle(string title)
        => $"<h2 style='color:#1e40af;border-bottom:2px solid #1e40af;padding-bottom:6px'>{title}</h2>";

    public string RenderSection(string sectionName, IReadOnlyList<string> lines)
    {
        var items = string.Join("", lines.Select(l => $"<li>{l}</li>"));
        return $"<h4 style='color:#374151;margin-bottom:4px'>{sectionName}</h4><ul style='margin:0;padding-left:1.2rem'>{items}</ul>";
    }

    public string RenderKeyValue(string key, string value)
        => $"<div style='display:flex;gap:1rem;padding:3px 0'><span style='color:#6b7280;min-width:150px'>{key}</span><strong>{value}</strong></div>";

    public string RenderSeparator()
        => "<hr style='border:none;border-top:1px solid #e5e7eb;margin:8px 0'/>";

    public string Finalize(IReadOnlyList<string> parts)
        => $"<div style='font-family:sans-serif;font-size:14px;padding:1rem;background:#f9fafb;border-radius:8px'>{string.Join("", parts)}</div>";
}