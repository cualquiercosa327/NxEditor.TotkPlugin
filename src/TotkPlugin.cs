using NxEditor.Plugin;

namespace NxEditor.TotkPlugin;

public class TotkPlugin : IServiceExtension
{
    public string Name { get; } = "TotK Plugin";

    public void RegisterExtension(IServiceLoader serviceLoader)
    {
        serviceLoader.Register(nameof(TotkZstd), new TotkZstd());
    }
}
