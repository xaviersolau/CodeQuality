// See https://aka.ms/new-console-template for more information

#if DEBUG
using System.Diagnostics;

if (!Debugger.IsAttached && args != null && args.Contains("--debug:true"))
{
    Debugger.Launch();
}

#endif

Console.WriteLine("Hello, World!");

if (args != null)
{
    for (var i = 0; i < args.Length; i++)
    {
        var arg = args[i];

        Console.WriteLine($"given arg {i}: {arg}");
    }
}
else
{
    Console.WriteLine($"no given args");
}

