//using CompLabs.Entities;
//using CompLabs.Lexical;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace CompLabs.Generation
//{
//  /// <summary>
//  /// Класс для оптимизации трехадресных команд
//  /// </summary>
//  internal class ThreeAddressCodeOptimizer2
//  {
//    private readonly SymbolsDic symbolsDic;
//    private Stack<Token> reusableTemps = new(); // Стек доступных временных переменных
//    private int tempVarCounter = 1; // Счетчик новых временных переменных

//    public ThreeAddressCodeOptimizer2(SymbolsDic symbolsDic)
//    {
//      this.symbolsDic = symbolsDic;
//    }

//    // Метод для получения временной переменной
//    private Token GetTempVariable(LexicalTypesEnum? type)
//    {
//      if (reusableTemps.Count > 0)
//      {
//        var tempVar = reusableTemps.Pop();
//        tempVar.Type = type; // Обновляем тип переменной
//        return tempVar;
//      }

//      // Если в стеке нет переменных, создаем новую
//      var newTempVar = new Token(LexicalTypesEnum.Identifier, "#T" + tempVarCounter++, -1);
//      newTempVar.Type = type;
//      return newTempVar;
//    }

//    // Метод для освобождения временной переменной
//    private void ReleaseTempVariable(Token tempVar)
//    {
//      reusableTemps.Push(tempVar); // Возвращаем переменную в стек
//    }

//    // Оптимизация команд с переиспользованием временных переменных
//    public List<Instruction> Optimize(List<Instruction> instructions)
//    {
//      var optimizedInstructions = new List<Instruction>();

//      foreach (var instruction in instructions)
//      {
//        // Для i2f, если возможно переиспользовать, то используем старую временную переменную
//        if (instruction.Manual == GenerationInstructionsEnum.i2f)
//        {
//          if (instruction.Tokens[0].Type == LexicalTypesEnum.Identifier && instruction.Tokens[0].Value.ToString().StartsWith("#T"))
//          {
//            Token tempToken = instruction.Tokens[0];
//            var existingTempVar = reusableTemps.FirstOrDefault(t => t.Value.ToString() == tempToken.Value.ToString());

//            if (existingTempVar != null)
//            {
//              instruction.ResToken = existingTempVar; // Переиспользуем временную переменную
//            }
//            else
//            {
//              // Если нет подходящей переменной, создаем новую
//              instruction.ResToken = GetTempVariable(LexicalTypesEnum.Float);
//            }
//          }
//        }

//        // Для других операций (например, add, sub, mul), переиспользуем переменные, если возможно
//        if (instruction.ResToken.Type == LexicalTypesEnum.Identifier && instruction.ResToken.Value.ToString().StartsWith("#T"))
//        {
//          string tempVarName = instruction.ResToken.Value.ToString();
//          Token tempVar = reusableTemps.FirstOrDefault(t => t.Value.ToString() == tempVarName);

//          if (tempVar != null)
//          {
//            // Заменяем на уже использованную временную переменную, если возможно
//            instruction.ResToken = tempVar;
//          }
//          else
//          {
//            // Если еще не использована, создаем новую
//            instruction.ResToken = GetTempVariable(LexicalTypesEnum.Unknown);
//          }
//        }

//        // Освобождаем временные переменные после использования
//        if (instruction.Tokens.Any(t => t.Type == LexicalTypesEnum.Identifier && t.Value.ToString().StartsWith("#T")))
//        {
//          foreach (var token in instruction.Tokens)
//          {
//            if (token.Type == LexicalTypesEnum.Identifier && token.Value.ToString().StartsWith("#T"))
//            {
//              ReleaseTempVariable(token);
//            }
//          }
//        }

//        // Добавляем оптимизированную инструкцию в результат
//        optimizedInstructions.Add(instruction);
//      }

//      return optimizedInstructions;
//    }
//  }
//}
