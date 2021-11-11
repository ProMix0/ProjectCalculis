using MainLibrary.Interfaces;
using System;

namespace TestWork
{
    public class Program : IWorkCode
    {
        static void Main(string[] args)
        {
            Console.WriteLine("It's Main(), dumb -_-");
        }

        public void Entrypoint(object argsObject)
        {
            Console.WriteLine("TestWork World!");
            Console.WriteLine("It's really work?");
            Console.WriteLine(argsObject);
        }
    }
}
