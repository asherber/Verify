[UsesVerify]
public class ComparerTests
{
    [Fact]
    public async Task Instance()
    {
        var settings = new VerifySettings();
        settings.UseStringComparer(Compare);
        await Verify("TheText", settings);
        PrefixUnique.Clear();
        await Verify("thetext", settings);
    }

#if NET7_0_OR_GREATER

    [Fact]
    public async Task InstanceOverride()
    {
        var settings = new VerifySettings();
        settings.UseStringComparer(Compare);
        await Verify("TheText", "staticComparerExtMessage", settings);
        PrefixUnique.Clear();
        await Verify("thetext", "staticComparerExtMessage", settings);
    }

    [Fact]
    public async Task Instance_with_message()
    {
        var settings = new VerifySettings();
        settings.UseStringComparer(CompareWithMessage);
        settings.DisableDiff();
        var exception = await Assert.ThrowsAsync<VerifyException>(() => Verify("NotTheText", settings));
        Assert.Contains("theMessage", exception.Message);
    }

    [Fact]
    public async Task InstanceOverride_with_message()
    {
        var settings = new VerifySettings();
        settings.UseStringComparer(CompareWithOtherMessage);
        settings.DisableDiff();
        var exception = await Assert.ThrowsAsync<VerifyException>(() => Verify("NotTheText", "staticComparerExtMessage", settings));
        Assert.Contains("otherMessage", exception.Message);
    }

    [Fact]
    public async Task Instance_with_message_Fluent()
    {
        var settings = new VerifySettings();
        settings.DisableDiff();
        var exception = await Assert.ThrowsAsync<VerifyException>(() => Verify("NotTheText", settings).UseStringComparer(CompareWithMessage));
        Assert.Contains("theMessage", exception.Message);
    }

    [Fact]
    public async Task InstanceOverride_with_message_Fluent()
    {
        var settings = new VerifySettings();
        settings.DisableDiff();
        var exception = await Assert.ThrowsAsync<VerifyException>(() => Verify("NotTheText", "staticComparerExtMessage", settings).UseStringComparer(CompareWithOtherMessage));
        Assert.Contains("otherMessage", exception.Message);
    }

    [ModuleInitializer]
    public static void Static_with_messageInit()
    {
        FileExtensions.AddTextExtension("staticComparerExtMessage");
        VerifierSettings.RegisterStringComparer("staticComparerExtMessage", CompareWithMessage);
    }

    [Fact]
    public async Task Static_with_message()
    {
        var settings = new VerifySettings();
        settings.DisableDiff();
        settings.UseMethodName("Static_with_message_temp");
        await ThrowsTask(() => Verify("TheText", "staticComparerExtMessage", settings));
    }

    static Task<CompareResult> CompareWithMessage(string stream, string received, IReadOnlyDictionary<string, object> readOnlyDictionary) =>
        Task.FromResult(CompareResult.NotEqual("theMessage"));

    static Task<CompareResult> CompareWithOtherMessage(string stream, string received, IReadOnlyDictionary<string, object> readOnlyDictionary) =>
        Task.FromResult(CompareResult.NotEqual("otherMessage"));

#endif

    [ModuleInitializer]
    public static void StaticInit()
    {
        FileExtensions.AddTextExtension("staticComparerExt");
        VerifierSettings.RegisterStringComparer("staticComparerExt", Compare);
    }

    [Fact]
    public async Task Static()
    {
        await Verify("TheText", "staticComparerExt");
        PrefixUnique.Clear();
        await Verify("thetext", "staticComparerExt");
    }

    static Task<CompareResult> Compare(string received, string verified, IReadOnlyDictionary<string, object> context) =>
        Task.FromResult(new CompareResult(string.Equals(received, received, StringComparison.OrdinalIgnoreCase)));
}