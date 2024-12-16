using ClassLiberty.Entities;
using ClassLiberty.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interpreter
{
  internal class Calculator
  {
    private SymbolsDic symbolsDic;
    private List<Instruction> instructions;

    public Calculator(SymbolsDic symbolsDic, List<Instruction> instructions)
    {
      this.symbolsDic = symbolsDic;
      this.instructions = instructions;
    }

    public void ExecuteInstructions()
    {
      foreach (var instruction in instructions)
      {
        // Если инструкция содержит результат (например, результат присваивания или операции)
        var resultToken = instruction.ResToken;

        // Определяем операнды и выполняем операцию
        if (instruction.Tokens.Count == 2) // Для бинарных операций (например, сложение, вычитание)
        {
          var operand1 = instruction.Tokens[0];
          var operand2 = instruction.Tokens[1];

          // Получаем значения операндов из символов
          double value1 = GetValue(operand1);
          double value2 = GetValue(operand2);

          // Получаем типы операндов из таблицы символов
          var type1 = operand1.Type;

          var type2 = operand2.Type;

          if (operand1.Type == LexicalTypesEnum.IntegerConstant || operand1.Type == LexicalTypesEnum.RealConstant)
          {
            type1 = LexicalTypesEnum.IntegerConstant == operand1.Type ? LexicalTypesEnum.IntType : LexicalTypesEnum.FloatType;
          } else
          {
            type1 = symbolsDic[operand1.IdentifierID].Type;
          }

          if (operand2.Type == LexicalTypesEnum.IntegerConstant || operand2.Type == LexicalTypesEnum.RealConstant)
          {
            type2 = LexicalTypesEnum.IntegerConstant == operand2.Type ? LexicalTypesEnum.IntType : LexicalTypesEnum.FloatType;
          }
          else
          {
            type2 = symbolsDic[operand2.IdentifierID].Type;
          }

          double result = 0;

          // Выполняем операцию в зависимости от типа инструкции
          switch (instruction.Manual)
          {
            case GenerationInstructionsEnum.add:
              result = value1 + value2;
              break;
            case GenerationInstructionsEnum.sub:
              result = value1 - value2;
              break;
            case GenerationInstructionsEnum.mul:
              result = value1 * value2;
              break;
            case GenerationInstructionsEnum.div:
              result = value1 / value2;
              break;
            default:
              Console.WriteLine("Неизвестная операция");
              break;
          }

          // Устанавливаем тип результата
          LexicalTypesEnum resultType = DetermineResultType(type1, type2);
          symbolsDic[resultToken.IdentifierID].Value = result;
          symbolsDic[resultToken.IdentifierID].Type = resultType;

          //Console.WriteLine($"Результат операции {instruction.Manual}: {result}, тип результата: {resultType}");
          Console.WriteLine($"Результат операции {instruction.Manual}: {Math.Round(result, 4)} ; тип результата: {resultType}");
          //Console.WriteLine($"Результат операции {instruction.Manual}: " + string.Format("{0:0.0000}", result) + $" тип результата: {resultType}");
        }
        else if (instruction.Tokens.Count == 1) // Для унарных операций (например, отрицание)
        {
          var operand = instruction.Tokens[0];

          // Получаем значение операнда
          double value = GetValue(operand);

          // Получаем тип операнда из таблицы символов

          var operandType = operand.Type;

          if (operand.Type == LexicalTypesEnum.IntegerConstant || operand.Type == LexicalTypesEnum.RealConstant)
          {
            operandType = LexicalTypesEnum.IntegerConstant == operand.Type ? LexicalTypesEnum.IntType : LexicalTypesEnum.FloatType;
          }
          else
          {
            operandType = symbolsDic[operand.IdentifierID].Type;
          }

          double result = 0;

          // Выполняем унарную операцию
          switch (instruction.Manual)
          {
            case GenerationInstructionsEnum.i2f: // Преобразование целого в вещественное число
              result = value; // Преобразуем как нужно
              break;
            default:
              Console.WriteLine("Неизвестная унарная операция");
              break;
          }

          // Устанавливаем тип результата
          LexicalTypesEnum resultType = operandType == LexicalTypesEnum.IntType
              ? LexicalTypesEnum.FloatType
              : operandType;

          symbolsDic[resultToken.IdentifierID].Value = result;
          symbolsDic[resultToken.IdentifierID].Type = resultType;

          Console.WriteLine($"Результат операции {instruction.Manual}: {Math.Round(result, 4)} ; тип результата: {resultType}");
          //Console.WriteLine($"Результат операции {instruction.Manual}: " + string.Format("{0:0.0000}", result) + $" тип результата: {resultType}");
        }
      }
      if (instructions.Count > 0) {
        var lastInstruction = instructions[instructions.Count - 1];
        var result = lastInstruction.ResToken;

        Console.WriteLine($"Итого: {Math.Round(symbolsDic[result.IdentifierID].Value, 4)}");
        //Console.WriteLine($"Итого: {string.Format("{0:0.0000}", symbolsDic[result.IdentifierID].Value)}");
      }
    }

    // Метод для получения значения токена, может быть переменной или константой
    private double GetValue(Token token)
    {
      if (token.Type == LexicalTypesEnum.IntegerConstant || token.Type == LexicalTypesEnum.RealConstant)
      {
        return Convert.ToDouble(token.Value);
      } 
      else
      {
        var symbol = symbolsDic[token.IdentifierID];

        // Получаем значение в зависимости от типа символа
        if (symbol.Type == LexicalTypesEnum.IntType || symbol.Type == LexicalTypesEnum.FloatType)
        {
          return Convert.ToDouble(symbol.Value);
        }
        else
        {
          // Для других типов данных возвращаем значение как строку или выполняем дополнительную обработку
          return Convert.ToDouble(symbol.Value);
        }
      }
      
    }

    // Метод для определения типа результата в зависимости от типов операндов
    private LexicalTypesEnum DetermineResultType(LexicalTypesEnum type1, LexicalTypesEnum type2)
    {
      // Логика для определения типа результата, в зависимости от типов операндов
      if (type1 == LexicalTypesEnum.IntType && type2 == LexicalTypesEnum.IntType)
      {
        return LexicalTypesEnum.IntType;
      }
      else
      {
        // Если хотя бы один операнд — вещественное число, результат будет вещественным
        return LexicalTypesEnum.FloatType;
      }
    }
  }
}
