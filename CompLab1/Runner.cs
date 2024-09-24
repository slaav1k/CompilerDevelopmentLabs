namespace CompLab1
{
  /// <summary>
  /// Основной класс
  /// </summary>
  internal class Runner
  {
    static void Main(string[] args)
    {
      try
      {
        // Проверка наличия аргументов
        if (args.Length == 0)
          throw new InvalidArgumentsException("Ошибка: недостаточно аргументов.");

        // Режим работы
        string mode = args[0].ToUpper(); 

        
        if (mode == "G")
        {
          if (args.Length != 5)
            throw new InvalidArgumentsException("Ошибка: неверное количество аргументов для режима генерации.");

          string outputFile = args[1];
          int numberOfLines;
          int minOperands;
          int maxOperands;

          if (!int.TryParse(args[2], out numberOfLines) || !int.TryParse(args[3], out minOperands) || !int.TryParse(args[4], out maxOperands))
            throw new InvalidArgumentsException("Ошибка: аргументы должны быть целыми числами.");

          if (minOperands < 2 || maxOperands > 10 || minOperands > maxOperands)
            throw new InvalidArgumentsException("Ошибка: минимальное и максимальное количество операндов заданы неверно.");

          Console.WriteLine($"Генерация {numberOfLines} выражений в файл {outputFile} с количеством операндов от {minOperands} до {maxOperands}.");
          
          ArithmeticGenerator.generateFile(outputFile, numberOfLines, minOperands, maxOperands);
        }
        else if (mode == "T")
        {
          if (args.Length != 3)
            throw new InvalidArgumentsException("Ошибка: неверное количество аргументов для режима трансляции.");

          string inputFile = args[1];
          string outputFile = args[2];

          if (!File.Exists(inputFile))
            throw new FileException($"Ошибка: файл {inputFile} не существует.");

          Console.WriteLine($"Трансляция файла {inputFile} в файл {outputFile}.");

          ArithmeticTranslater.translateFile(inputFile, outputFile);
        }
        else
        {
          throw new InvalidArgumentsException("Ошибка: неизвестный режим работы. Используйте 'G' для генерации или 'T' для трансляции.");
        }
      }
      catch (InvalidArgumentsException ex)
      {
        Console.WriteLine(ex.Message);
      }
      catch (FileException ex)
      {
        Console.WriteLine(ex.Message);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
      }
    }
  }
}
