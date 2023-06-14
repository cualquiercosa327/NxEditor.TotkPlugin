using NxEditor.Plugin.Models;
using NxEditor.Plugin.Services;
using SarcLibrary;
using ZstdSharp;

namespace NxEditor.TotkPlugin;

internal class TotkZstd : IProcessingService
{
    private static readonly Decompressor _defaultDecompressor = new();
    private static readonly Decompressor _commonDecompressor = new();
    private static readonly Decompressor _mapDecompressor = new();
    private static readonly Decompressor _packDecompressor = new();

    private static readonly Compressor _defaultCompressor = new(16);
    private static readonly Compressor _commonCompressor = new(16);
    private static readonly Compressor _mapCompressor = new(16);
    private static readonly Compressor _packCompressor = new(16);

    static TotkZstd()
    {
        string zsDicPath = Path.Combine(TotkConfig.Shared.GamePath, "Pack", "ZsDic.pack.zs");

        Span<byte> data = _commonDecompressor.Unwrap(File.ReadAllBytes(zsDicPath));
        SarcFile sarc = SarcFile.FromBinary(data.ToArray());

        _commonDecompressor.LoadDictionary(sarc["zs.zsdic"]);
        _mapDecompressor.LoadDictionary(sarc["bcett.byml.zsdic"]);
        _packDecompressor.LoadDictionary(sarc["pack.zsdic"]);

        _commonCompressor.LoadDictionary(sarc["zs.zsdic"]);
        _mapCompressor.LoadDictionary(sarc["bcett.byml.zsdic"]);
        _packCompressor.LoadDictionary(sarc["pack.zsdic"]);
    }

    private static Span<byte> Decompress(string file, Span<byte> raw)
    {
        return
            file.EndsWith(".bcett.byml.zs") ? _mapDecompressor.Unwrap(raw) :
            file.EndsWith(".pack.zs") ? _packDecompressor.Unwrap(raw) :
            file.EndsWith(".rsizetable.zs") ? _defaultDecompressor.Unwrap(raw) :
            _commonDecompressor.Unwrap(raw);
    }

    private static Span<byte> Compress(string file, Span<byte> raw)
    {
        return
            file.EndsWith(".bcett.byml.zs") ? _mapCompressor.Wrap(raw) :
            file.EndsWith(".pack.zs") ? _packCompressor.Wrap(raw) :
            file.EndsWith(".rsizetable.zs") ? _defaultCompressor.Wrap(raw) :
            _commonCompressor.Wrap(raw);
    }

    public IFileHandle Process(IFileHandle handle)
    {
        handle.Data = Decompress(handle.Path, handle.Data).ToArray();
        return handle;
    }

    public IFileHandle Reprocess(IFileHandle handle)
    {
        handle.Data = Compress(handle.Path, handle.Data).ToArray();
        return handle;
    }

    public bool IsValid(IFileHandle handle)
    {
        return handle.Path.EndsWith(".zs");
    }
}
