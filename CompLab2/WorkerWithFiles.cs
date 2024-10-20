using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompLab2
{
  /// <summary>
  /// Класс для работы с файлами
  /// </summary>
  internal class WorkerWithFiles
  {

    /// <summary>
    /// Проверка названия файла
    /// </summary>
    /// <param name="filename">Название файла</param>
    /// <returns>Да/Нет</returns>
    public static bool IsValidFileName(string filename)
    {
      if (Path.GetExtension(filename).ToLower() != ".txt")
      {
        return false;
      }
      string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
      Regex regex = new Regex(@"^[a-zA-Z0-9_]+$");

      if (!regex.IsMatch(fileNameWithoutExtension))
      {
        return false;
      }
      return true;
    }

    /// <summary>
    /// Проверка файла на существование
    /// </summary>
    /// <param name="fileName">Название файла</param>
    /// <returns>Да/Нет</returns>
    public static bool IsFileExit(string fileName)
    {
      return File.Exists(fileName);
    }

    /// <summary>
    /// Открытие файла на чтение
    /// </summary>
    /// <param name="filename">Название файла</param>
    /// <returns>Содержимое файла</returns>
    public static string ReadInputFile(string filename)
    {
      //return File.ReadAllText(filename).Replace(" ", "").Replace("\t", "");
      return File.ReadAllText(filename);
    }

    /// <summary>
    /// Проверка файла на содержимое
    /// </summary>
    /// <param name="filename">Название файла</param>
    /// <returns>Да/Нет</returns>
    public static bool IsValidFile(string filename)
    {
      string infoFromFile = ReadInputFile(filename);
      foreach (char c in infoFromFile)
      {
        if (!IsAllowedCharacter(c))
        {
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Допустимый ли символ
    /// </summary>
    /// <param name="c">Символ</param>
    /// <returns>Да/Нет</returns>
    private static bool IsAllowedCharacter(char c)
    {
      return char.IsLetterOrDigit(c) || c == '+' || c == '-' || c == '*' ||
             c == '/' || c == '(' || c == ')' || c == '.' || char.IsWhiteSpace(c);
    }

    /// <summary>
    /// Запись в файл Токены
    /// </summary>
    /// <param name="filename">Название файла</param>
    /// <param name="tokens">Коллекция токенов</param>
    public static void WriteTokensFile(string filename, List<Token> tokens)
    {
      using (StreamWriter writer = new StreamWriter(filename))
      {
        foreach (var token in tokens)
        {
          writer.WriteLine(token.ToString());
        }
      }
    }

    /// <summary>
    /// Запись в файл Идентификаторы
    /// </summary>
    /// <param name="filename">Название файла</param>
    /// <param name="symbolsDic">Слворь идентификаторов</param>
    public static void WriteSymbolsFile(string filename, SymbolsDic symbolsDic)
    {
      using (StreamWriter writer = new StreamWriter(filename))
      {
        foreach (var symbol in symbolsDic.GetSymbols())
        {
          writer.WriteLine($"{symbol.Key} – {symbol.Value}");
        }
      }
    }
  }
}
