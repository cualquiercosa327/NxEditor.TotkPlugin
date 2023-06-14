using NxEditor.Plugin.Extensions;

namespace NxEditor.TotkPlugin;

internal class TotkConfig : IConfigExtension
{
    public static TotkConfig Shared { get; } = new();

    public string Name { get; set; } = "TotK Config";
    public string GamePath { get; set; } = string.Empty;
}
