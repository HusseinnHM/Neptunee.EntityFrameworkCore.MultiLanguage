public static class SmapleLanguages
{
    public static IEnumerable<string> List => new[] { Ar, Fr };

    public static readonly string En = nameof(En);
    public static readonly string Ar = new(nameof(Ar));
    public static readonly string Fr = new(nameof(Fr));

}