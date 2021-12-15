namespace Lambdas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Program p = CreateProgram(args);

            p.RunAction(cfg => { Console.WriteLine($"Configuration is: {cfg.ConnectionString}"); cfg.ConnectionString = "Changed"; })
                .RunAction(ChangeConfiguration);

            p.RunFunction((msg) => { Console.WriteLine(msg); return new Configuration() { ConnectionString = "Configuration created in the Callback" }; })
                .RunFunction(CreateConfiguration);

            Expression<Func<string, bool>> exp = s => s == "Second" || s.Length == 5;
            p.RunExpression(exp);
        }

        private static Program CreateProgram(string[] args)
        {
            Program p = new Program();

            //Do something with args

            return p;
        }

        private static void ChangeConfiguration(Configuration cfg)
        {
            Console.WriteLine($"Configuration is: {cfg.ConnectionString}");
            cfg.ConnectionString = "Changed";
        }

        private static Configuration CreateConfiguration(string msg)
        {
            Console.WriteLine(msg);

            return new Configuration()
            {
                ConnectionString = "Configuration created in the Callback"
            };
        }

        private Program RunAction(Action<Configuration> configurationCallback)
        {
            //In this method we create and initialize the Configuration instance
            Configuration configuration = new() { ConnectionString = "Change me!!!" };

            //Then we send it out so it could be altered
            configurationCallback?.Invoke(configuration);

            //Do something with this altered configuration afterwards
            Console.WriteLine($"Configuration is: {configuration.ConnectionString}");

            return this;
        }

        private Program RunFunction(Func<string, Configuration> configurationCallback)
        {
            Configuration configuration = configurationCallback?.Invoke("Give me a Configuration!!!");

            //Do something with configuration
            Console.WriteLine($"Given Configuration contains: {configuration.ConnectionString}");

            return this;
        }

        private Program RunExpression(Expression<Func<string, bool>> filter)
        {
            IQueryable<string> arr = new List<string>(new string[] { "First", "Second", "Third", "Fourth", "Fifth", "Sixth" }).AsQueryable();

            IEnumerable<string> result = arr.Where(filter);

            Console.WriteLine($"Your filtered list is: {string.Join(" ", result)}");

            return this;
        }
    }

    public class Configuration
    {
        public string ConnectionString { get; set; }
    }
}
