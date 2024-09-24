using System;
using System.Collections.Generic;
using System.IO; 

namespace CompLab1
{
  /// <summary>
  /// Основной класс для генерации и обработки арифметических операндов 
  /// </summary>
  internal class ArithmeticGenerator
  {
    /// <summary>
    /// Генератор случайных чисел
    /// </summary>
    private static readonly Random _random = new();

    /// <summary>
    /// Операнды
    /// </summary>
    private static readonly char[] _operands = { '+', '-', '*', ':' }; 


    /// <summary>
    /// Генерация файла с арифметическими выражениями
    /// </summary>
    /// <param name="parNameFile">Имя файла</param>
    /// <param name="parNumLines">Количество строк</param>
    /// <param name="parMinOper">Минимальное количество операндов</param>
    /// <param name="parMaxOper">Максимальное количество операндов</param>
    public static void generateFile(string parNameFile, int parNumLines, int parMinOper, int parMaxOper)
    {
      using (StreamWriter writer = new StreamWriter(parNameFile)) 
      {
        for (int i = 0; i < parNumLines; i++)
        {
          string expression = generateExpression(parMinOper, parMaxOper);
          writer.WriteLine(expression);
        }
      }

      Console.WriteLine($"Файл {parNameFile} успешно создан с {parNumLines} строками.");
    }

    /// <summary>
    /// Генерация одного арифметического выражения
    /// </summary>
    /// <param name="parMinOperands">Минимальное количество операндов</param>
    /// <param name="parMaxOperands">Максимальное количество операндов</param>
    /// <returns>Строка с выражением</returns>
    private static string generateExpression(int parMinOperands, int parMaxOperands)
    {
      int numOperands = _random.Next(parMinOperands, parMaxOperands + 1); 
      List<string> expression = new List<string>();

      for (int i = 0; i < numOperands; i++)
      {
        int number = _random.Next(1, 10);
        expression.Add(number.ToString());

        if (i < numOperands - 1)
        {
          char operand = _operands[_random.Next(_operands.Length)];
          expression.Add(operand.ToString());
        }
      }

      return string.Join(" ", expression);
    }

  }
}
