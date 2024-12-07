using ClassLiberty.Enums;
using ClassLiberty.Entities;

namespace Interpreter
{
  internal class RunnerInterpreteter
  {
    static void Main(string[] args)
    {
      SymbolsDic deserializedSymbolsDic;
      List<Instruction> deserializedInstructions;

      string combinedFilePath = "../../../../CompLab7/bin/Debug/net8.0/post_code.bin";
      using (FileStream fs = new FileStream(combinedFilePath, FileMode.Open))
      using (BinaryReader reader = new BinaryReader(fs))
      {
        // Десериализация SymbolsDic
        deserializedSymbolsDic = SymbolsDic.Deserialize(reader);
        Console.WriteLine("Deserialized SymbolsDic:");
        foreach (var pair in deserializedSymbolsDic.GetSymbols())
        {
          Console.WriteLine($"{pair.Key}: {pair.Value}");
        }

        // Десериализация списка инструкций
        int instructionCount = reader.ReadInt32(); // Читаем количество инструкций
        deserializedInstructions = new List<Instruction>();
        for (int i = 0; i < instructionCount; i++)
        {
          deserializedInstructions.Add(Instruction.Deserialize(reader)); // Десериализуем каждую инструкцию
        }

        Console.WriteLine("Deserialized List of Instructions:");
        foreach (var instruction in deserializedInstructions)
        {
          Console.WriteLine(instruction);
        }
      }



      foreach (var symbol in deserializedSymbolsDic.GetSymbols())
      {
        // Пропускаем временные переменные (имя начинается с #T)
        if (!symbol.Value.Name.StartsWith("#T"))
        {
          bool validInput = false;
          while (!validInput)
          {
            Console.WriteLine($"Введите значение для переменной {symbol.Value.Name} (тип: {symbol.Value.Type}):");
            string input = Console.ReadLine();

            try
            {
              // Преобразуем ввод в нужный тип
              if (symbol.Value.Type == LexicalTypesEnum.IntType)
              {
                if (int.TryParse(input, out int intValue))
                {
                  symbol.Value.Value = intValue;
                  validInput = true;  // Ввод корректен, выходим из цикла
                }
                else
                {
                  Console.WriteLine("Некорректный ввод! Пожалуйста, введите целое число.");
                }
              }
              else if (symbol.Value.Type == LexicalTypesEnum.FloatType || symbol.Value.Type == LexicalTypesEnum.RealConstant)
              {
                if (double.TryParse(input, out double doubleValue))
                {
                  symbol.Value.Value = doubleValue;
                  validInput = true;  
                }
                else
                {
                  Console.WriteLine("Некорректный ввод! Пожалуйста, введите число с плавающей точкой.");
                }
              }
              else
              {
                symbol.Value.Value = input; 
                validInput = true; 
              }
            }
            catch (Exception ex)
            {
              Console.WriteLine($"Ошибка: {ex.Message}. Попробуйте снова.");
            }
          }

          Console.WriteLine($"Для {symbol.Value.Name} установлено значение: {symbol.Value.Value} (тип: {symbol.Value.Value.GetType().Name})");
        }
      }

      //// Пример для подсчета суммы, предполагая, что все значения теперь числа:
      //double sum = 0;
      //foreach (var symbol in deserializedSymbolsDic.GetSymbols())
      //{
      //  if (!symbol.Value.Name.StartsWith("#T"))
      //  {
      //    sum += Convert.ToDouble(symbol.Value.Value); // Конвертируем в double для подсчета суммы
      //  }
      //}

      //Console.WriteLine($"Сумма всех значений: {sum}");

      Calculator calculator = new Calculator(deserializedSymbolsDic, deserializedInstructions);

      calculator.ExecuteInstructions();



    }
  }
}
