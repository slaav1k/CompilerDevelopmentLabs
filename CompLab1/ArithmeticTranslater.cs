using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLab1
{
  /// <summary>
  /// Класс отвечающий за перевод содержимого из символьного языка в русский
  /// </summary>
  internal class ArithmeticTranslater
  {
    /// <summary>
    /// Переводчик с символьного на русский
    /// </summary>
    private static readonly Dictionary<char, string> _mapOperands = new()
        {
            {'+', "плюс" },
            {'-', "минус" },
            {'*', "умножить на" },
            {':', "делить на" }
        };

    /// <summary>
    /// Переводчик с числового на русский
    /// </summary>
    private static readonly Dictionary<char, string> _mapDigits = new()
        {
            {'1', "один" },
            {'2', "два" },
            {'3', "три" },
            {'4', "четыре" },
            {'5', "пять" },
            {'6', "шесть" },
            {'7', "семь" },
            {'8', "восемь" },
            {'9', "девять" }
        };


    /// <summary>
    /// Проверка файла на нужный формат
    /// </summary>
    /// <param name="parNameFile">Название файла</param>
    /// <returns></returns>
    private static bool checkFile(string parNameFile)
    {
      try
      {
        using (StreamReader f = new StreamReader(parNameFile))
        {
          string line;
          while ((line = f.ReadLine()) != null)
          {
            foreach (char ch in line)
            {

              if (!(_mapDigits.ContainsKey(ch) || _mapOperands.ContainsKey(ch) || char.IsWhiteSpace(ch)))
              {
                return false;
              }
            }
            return true;
          }
        }

      }
      catch (FileException)
      {
        Console.WriteLine($"Ошибка: файл {parNameFile} не найден.");
        return false;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Произошла ошибка: {ex.Message}");
        return false;
      }
      return true;
    }


    /// <summary>
    /// Перевод содержимого файла из математического стиля в словесный
    /// </summary>
    /// <param name="parNameInputFile">Имя входного файла</param>
    /// <param name="parNameOutputFile">Имя выходного файла</param>
    public static void translateFile(string parNameInputFile, string parNameOutputFile)
    {
      if (checkFile(parNameInputFile))
      {
        try
        {
          using (StreamReader reader = new StreamReader(parNameInputFile))
          using (StreamWriter writer = new StreamWriter(parNameOutputFile))
          {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
              string translatedLine = translateLine(line);
              writer.WriteLine(translatedLine);
            }
          }

          Console.WriteLine($"Файл {parNameOutputFile} успешно создан.");
        }
        catch (FileException)
        {
          Console.WriteLine($"Ошибка: файл {parNameInputFile} не найден.");
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
      }
      else
      {
        throw new FileException("Ошибка: файл содержит символы неверного формата.");
      }

      
    }

    /// <summary>
    /// Перевод арифметической строки в словесное представление
    /// </summary>
    /// <param name="parLine">Арифметическая строка</param>
    /// <returns>Строка со словесным представлением</returns>
    private static string translateLine(string parLine)
    {
      var translatedExpression = new List<string>();

      foreach (char ch in parLine)
      {
        if (_mapDigits.ContainsKey(ch))
        {
          translatedExpression.Add(_mapDigits[ch]);
        }
        else if (_mapOperands.ContainsKey(ch))
        {
          translatedExpression.Add(_mapOperands[ch]);
        }
        else if (ch == ' ')
        {
          continue;
        }
        else
        {
          translatedExpression.Add(ch.ToString());
        }
      }

      return string.Join(" ", translatedExpression);
    }
  }
}
