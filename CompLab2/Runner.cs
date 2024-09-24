using CompLab2.CompLab2;
using System.Diagnostics.Metrics;

namespace CompLab2
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
        if (args.Length < 3)
          throw new InvalidArgumentsException("Ошибка: недостаточно аргументов.");

        string inputFileName = args[0];
        string tokensFileName = args[1];
        string symbolsFileName = args[2];

        // Проверка входного файла
        if (!(WorkerWithFiles.IsValidFileName(inputFileName) && WorkerWithFiles.IsValidFileName(tokensFileName)
          && WorkerWithFiles.IsValidFileName(symbolsFileName)))
          throw new FileException("Ошибка: некоректное название одного из файла");

        if (!WorkerWithFiles.IsFileExit(inputFileName))
          throw new FileException($"Ошибка: {inputFileName} не существует");

        //if (!WorkerWithFiles.IsValidFile(inputFileName))
        //  throw new FileException("Ошибка: некоректное содержимое файла");

        // Анализ содержимого
        List<Token> tokens = LexicalHandler.Analyze(WorkerWithFiles.ReadInputFile(inputFileName));

        WorkerWithFiles.WriteTokensFile(tokensFileName, tokens);
        WorkerWithFiles.WriteSymbolsFile(symbolsFileName, LexicalHandler.GetSymbolDictionary());

        Console.WriteLine("Анализ завершён успешно.");

      }
      catch (InvalidArgumentsException ex)
      {
        Console.WriteLine(ex.Message);
      }
      catch (FileException ex)
      {
        Console.WriteLine(ex.Message);
      }
      catch (LexicalException ex)
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
