namespace MySuperDuperPasswordManager
{
    using System;
    using System.Collections.Generic;

    public class Application
    {
        private readonly DumyPasswordManager _passwordManager = new DumyPasswordManager();
        private readonly Dictionary<string, Action> _commands = new Dictionary<string, Action>();

        private static class Commnads
        {
            internal const string Add = "ADD";
            internal const string Get = "GET";
            internal const string List = "LIST";
            internal const string Help = "HELP";

        }
        public Application() : this(Array.Empty<string>()){}
        public Application(string[] args)// аргументы на будущее
        {
            foreach (var arg in args)
            { }

            _commands = new Dictionary<string, Action>()
            {
                {
                    Commnads.Add, () => _passwordManager.AddPassword()
                },
                {
                    Commnads.Get, () => _passwordManager.GetPassword()
                },
                {
                    Commnads.List, () => _passwordManager.ListAllPasswords()
                },
                {
                    Commnads.Help, () => 
                    {
                        string help = $"{Commnads.Add} - Добавить новый пароль для хранения\n" +
                        $"{Commnads.Get} - получить пароль по имени, если такой есть\n" +
                        $"{Commnads.List} - вывести все пароли сохраненные пароли\n" +
                        $"{Commnads.Help} - вывести эту справку\n";
                        Console.WriteLine(help);
                    }
                }
            };
        }

        public void Run()
        {
            Console.WriteLine("Сtrl+c - выйти из программы");
            while (true)
            {
                ReadCommand(out Action command);
                if(command is null)
                {
                    Console.WriteLine($"Неизвестная комманда. Используйте {Commnads.Help} чтобы получить справку");
                }
                else
                {
                    command();
                }
            }
        }

        private void ReadCommand(out Action command)
        {
            var commandName = Console.ReadLine().ToUpperInvariant();
            _commands.TryGetValue(commandName, out command);
        }
    }
}
