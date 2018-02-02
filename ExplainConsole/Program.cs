using System;

namespace ExplainConsole
{
    using Explainer.Core;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test parser\n\n");
            ExplainParser parser = new ExplainParser();

            parser.ReadFile("explain.txt");
            Console.Read();
        }
    }
}
