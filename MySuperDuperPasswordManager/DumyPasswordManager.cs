namespace MySuperDuperPasswordManager
{
    using System;
    using System.IO;
    using System.Linq;
    public class DumyPasswordManager
    {
        private const string _path = "./passwords";
        public void AddPassword()
        {
            UserInput("Name", out string passwordName, IsValidStringFormat, "Нельзя использовать ',' в имени");
            UserInput("Password", out string password, IsValidStringFormat, "Нельзя использовать ',' в пароле");
            AddPasswordToTFile(passwordName, password);
        }

        private static bool IsValidStringFormat(string passwordName)
        {
            return passwordName.Contains(",") == false;
        }

        public void GetPassword()
        {
            UserInput("Name", out string passwordName, IsValidStringFormat, "Нельзя использовать ',' в имени");
            string[] passwordStrings = GetPaswordString(passwordName);
            if (passwordStrings is null)
            {
                PrintErrorLine("Пароль не найден");
            }
            else
            {
                PrintLn(passwordStrings[1]);
            }

        }

        private static string[] GetPaswordString(string passwordName)
        {
            return File.ReadLines(_path)
                .Select(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries))
                .FirstOrDefault(x => x[0].Equals(passwordName));
        }

        internal void ListAllPasswords()
        {
            foreach (var line in File.ReadLines(_path))
            {
                var lineItem = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (lineItem.Length != 2)
                {
                    PrintErrorLine("Format errors");
                    continue;
                }
                PrintLn($"Name: {lineItem[0]}; Password: {lineItem[1]}");
            }
        }

        private void AddPasswordToTFile(in string passwordName, in string password)
        {
            CreatePasswordsFileIfNotExist();
            if (IsUniqeuPasswordName(in passwordName))
            {
                var text = $"{passwordName},{password}\n";
                File.AppendAllText(_path, text);
            }
            else
            {
                PrintErrorLine("Имя пароля должно быть уникально");
            }
        }

        private bool IsUniqeuPasswordName(in string passwordName)
        {
            string[] passwordStrings = GetPaswordString(passwordName);
            return passwordStrings is null;
        }
        private void CreatePasswordsFileIfNotExist()
        {
            if (!File.Exists(_path))
            {
                using var fs = File.Create(_path);
                fs.Close();
            }
        }

        #region UTILS 
        private static void PrintLn(string str) => Console.WriteLine($">> {str}");
        private static void ReadLn(out string str) => str = Console.ReadLine();
        private static void UserInput(string messageForPrint, out string str, Func<string, bool> validator, string errorMessage)
        {
            while (true)
            {
                PrintLn(messageForPrint);
                ReadLn(out str);
                if (!validator(str))
                {
                    PrintErrorLine(errorMessage);
                }
                else
                {
                    break;
                }
            }
        }
        private static void PrintErrorLine(string error)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            PrintLn(error);
            Console.ForegroundColor = color;
        }
        #endregion
    }
}
