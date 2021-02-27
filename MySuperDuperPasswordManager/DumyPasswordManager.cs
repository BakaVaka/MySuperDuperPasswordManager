namespace MySuperDuperPasswordManager
{
    using System;
    using System.IO;
    using System.Linq;
    public class DumyPasswordManager
    {
        //путь к файлику с паролями)))
        private const string _path = "./passwords";

        #region Основные методы класса
        //------------------------------------------------------------------------------------------

        //Добавим новый пароль если 
        public void AddPassword()
        {
            UserInput("Name", out string passwordName, IsValidStringFormat, "Нельзя использовать ',' в имени");
            UserInput("Password", out string password, IsValidStringFormat, "Нельзя использовать ',' в пароле");
            AddPasswordToTFile(passwordName, password);
        }

        //Достанем пароль,если такой есть
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

        // Покажем все пароли какие имеем
        public void ListAllPasswords()
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
        //------------------------------------------------------------------------------------------
        #endregion
        #region Дополнительные методы
        //------------------------------------------------------------------------------------------

        // сохранение нового пароля в файл, если име уникально
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

        // получение пароля по имени
        private string[] GetPaswordString(string passwordName)
        {
            return File.ReadLines(_path)
                .Select(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries))
                .FirstOrDefault(x => x[0].Equals(passwordName));
        }

        // проверочка что нет запятушек
        private bool IsValidStringFormat(string passwordName)
        {
            return passwordName.Contains(",") == false;
        }

        // проверочка что имена не повторяются
        private bool IsUniqeuPasswordName(in string passwordName)
        {
            string[] passwordStrings = GetPaswordString(passwordName);
            return passwordStrings is null;
        }

        // создание файла если такого нет
        private void CreatePasswordsFileIfNotExist()
        {
            if (!File.Exists(_path))
            {
                using var fs = File.Create(_path);
                fs.Close();
            }
        }
        //------------------------------------------------------------------------------------------
        #endregion
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
