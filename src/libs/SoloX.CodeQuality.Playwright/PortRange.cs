namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Network port range definition.
    /// </summary>
    /// <param name="StartPort">Including port range start.</param>
    /// <param name="EndPort">Excluding port range end.</param>
    public record PortRange(int StartPort, int EndPort);
}
