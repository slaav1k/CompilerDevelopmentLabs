using CompLabs.Entities;
using CompLabs.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompLabs.Generation
{
  /// <summary>
  /// Класс для оптимизации трехадресных команд
  /// </summary>
  internal class ThreeAddressCodeOptimizer
  {
    private readonly SymbolsDic symbolsDic;

    public ThreeAddressCodeOptimizer(SymbolsDic symbolsDic)
    {
      this.symbolsDic = symbolsDic;
    }

    private static bool IsTempIdentifier(Token token)
    {
      return token.Type == LexicalTypesEnum.Identifier && token.Value.ToString().StartsWith("#T");
    }

    /// <summary>
    /// Оптимизирует список трехадресных команд
    /// </summary>
    /// <param name="instructions">Список трехадресных команд</param>
    /// <returns>Оптимизированный список трехадресных команд</returns>
    public List<Instruction> Optimize(List<Instruction> instructions)
    {
      var optimizedInstructions = new List<Instruction>();
      var tmpVarMapping = new Dictionary<Token, Token>(); // какую на какую заменил, по айди временной переменой

      //foreach (var instruction in instructions)
      for (int i = 0; i < instructions.ToList().Count; i++)
      {
        var instruction = instructions.ToList()[i];
        if (instruction.Manual == GenerationInstructionsEnum.i2f)
        {
          if (IsTempIdentifier(instruction.Tokens[0]))
          {
            Token tmpToken = instruction.Tokens[0];
            int idTempVar = symbolsDic.AddSymbol(tmpToken.Value, LexicalTypesEnum.Unknown);
            int idResVar = symbolsDic.AddSymbol(instruction.ResToken.Value, LexicalTypesEnum.Unknown);
            tmpVarMapping[instruction.ResToken] = tmpToken;
            instruction.ResToken = instruction.Tokens[0];    
            
            for (int j = i + 1; j < instructions.Count; j++)
            {
              var resVar = instructions[j].ResToken;
              if (tmpVarMapping.ContainsKey(resVar)) {
                instructions[j].ResToken = tmpVarMapping[resVar];
              }
              for (int k = 0; k < instructions[j].Tokens.Count; k++)
              {
                var tmpTokenIter = instructions[j].Tokens[k];
                if (tmpVarMapping.ContainsKey(tmpTokenIter))
                {
                  instructions[j].Tokens[k] = tmpVarMapping[tmpTokenIter];
                }
              }
            }
          };
        } 
        else
        {
          if (IsTempIdentifier(instruction.Tokens[0]))
          {
            Token tmpToken = instruction.Tokens[0];
            int idTempVar = symbolsDic.AddSymbol(tmpToken.Value, LexicalTypesEnum.Unknown);
            int idResVar = symbolsDic.AddSymbol(instruction.ResToken.Value, LexicalTypesEnum.Unknown);
            tmpVarMapping[instruction.ResToken] = tmpToken;
            instruction.ResToken = instruction.Tokens[0];

            for (int j = i + 1; j < instructions.Count; j++)
            {
              var resVar = instructions[j].ResToken;
              if (tmpVarMapping.ContainsKey(resVar))
              {
                instructions[j].ResToken = tmpVarMapping[resVar];
              }
              for (int k = 0; k < instructions[j].Tokens.Count; k++)
              {
                var tmpTokenIter = instructions[j].Tokens[k];
                if (tmpVarMapping.ContainsKey(tmpTokenIter))
                {
                  instructions[j].Tokens[k] = tmpVarMapping[tmpTokenIter];
                }
              }
            }
          }
          else if (!IsTempIdentifier(instruction.Tokens[0]) && IsTempIdentifier(instruction.Tokens[1]))
          {
            Token tmpToken = instruction.Tokens[1];
            int idTempVar = symbolsDic.AddSymbol(tmpToken.Value, LexicalTypesEnum.Unknown);
            int idResVar = symbolsDic.AddSymbol(instruction.ResToken.Value, LexicalTypesEnum.Unknown);
            tmpVarMapping[instruction.ResToken] = tmpToken;
            instruction.ResToken = instruction.Tokens[1];

            for (int j = i + 1; j < instructions.Count; j++)
            {
              var resVar = instructions[j].ResToken;
              if (tmpVarMapping.ContainsKey(resVar))
              {
                instructions[j].ResToken = tmpVarMapping[resVar];
              }
              for (int k = 0; k < instructions[j].Tokens.Count; k++)
              {
                var tmpTokenIter = instructions[j].Tokens[k];
                if (tmpVarMapping.ContainsKey(tmpTokenIter))
                {
                  instructions[j].Tokens[k] = tmpVarMapping[tmpTokenIter];
                }
              }
            }
          }
        }


        

        //// Добавляем оптимизированную инструкцию
        optimizedInstructions.Add(instruction);
      }

      return optimizedInstructions;
    }
  }
}
