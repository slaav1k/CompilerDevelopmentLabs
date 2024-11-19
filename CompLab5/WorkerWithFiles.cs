using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CompLabs.Entities;

namespace CompLabs
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
      File.WriteAllText(filename, symbolsDic.ToString());
    }

    /// <summary>
    /// Запись дерева в файл
    /// </summary>
    /// <param name="filename">Имя файла</param>
    /// <param name="syntaxTree">Синтаксическое дерево</param>
    public static void WriteToFile(string filename, SyntaxTree syntaxTree)
    {
      using (var writer = new StreamWriter(filename))
      {
        writer.Write(syntaxTree.ToString());
      }
    }


    /// <summary>
    /// Записывает строку трехадресного кода в файл.
    /// </summary>
    /// <param name="threeAddressCode">Строка с трехадресным кодом.</param>
    /// <param name="fileName">Имя файла для записи.</param>
    public static void WriteThreeAddressCodeToFile(List<Instruction> threeAddressCode, string fileName)
    {
      var lines = threeAddressCode.Select(instruction => instruction.ToString()).ToList();
      File.WriteAllLines(fileName, lines);
    }


    public static void WritePostfixFormToFile(List<Token> postfixForm, string fileName)
    {
      string line = string.Join("", postfixForm.Select(token => token.ToString().Split(" - ")[0]));

      File.WriteAllText(fileName, line);
    }
  }


}
