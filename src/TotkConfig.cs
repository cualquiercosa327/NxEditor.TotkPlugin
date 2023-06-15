using NxEditor.Plugin.Extensions;

namespace NxEditor.TotkPlugin;

internal class TotkConfig : ConfigExtension<TotkConfig>
{
    public override string Name { get; } = "totk";
    public string GamePath { get; set; } = string.Empty;
}
