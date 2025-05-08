// See https://aka.ms/new-console-template for more information

#if DEBUG
using System.Diagnostics;

if (!Debugger.IsAttached)
{
    Debugger.Launch();
}

Debugger.Break();

#endif

Console.WriteLine("Hello, World!");
