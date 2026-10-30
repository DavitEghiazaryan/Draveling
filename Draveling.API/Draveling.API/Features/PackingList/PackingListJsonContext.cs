using System.Text.Json.Serialization;

namespace Draveling.API.Features.PackingList;

/// <summary>Source-generated JSON for browser storage, so it keeps working when the app is trimmed.</summary>
[JsonSerializable(typeof(PackingListState))]
public sealed partial class PackingListJsonContext : JsonSerializerContext;
