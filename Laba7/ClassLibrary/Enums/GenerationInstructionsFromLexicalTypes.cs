
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLiberty.Enums
{
  /// <summary>
  /// Класс для добавления методов расширения к перечислению GenerationInstructionsEnum
  /// </summary>
  public static class GenerationInstructionsFromLexicalTypes
  {
    /// <summary>
    /// Метод расширения для получения команды по лексическому типу оператора
    /// </summary>
    /// <param name="type">Тип токена</param>
    /// <returns></returns>
    public static GenerationInstructionsEnum ToGetInstruction(this LexicalTypesEnum type)
    {
      return type switch
      {
        LexicalTypesEnum.Addition => GenerationInstructionsEnum.add,
        LexicalTypesEnum.Subtraction => GenerationInstructionsEnum.sub,
        LexicalTypesEnum.Multiplication => GenerationInstructionsEnum.mul,
        LexicalTypesEnum.Division => GenerationInstructionsEnum.div,
        LexicalTypesEnum.Coercion => GenerationInstructionsEnum.i2f,
        _ => GenerationInstructionsEnum.unknown,
      };
    }


    public static LexicalTypesEnum ToSetInstruction(this GenerationInstructionsEnum type)
    {
      return type switch
      {
        GenerationInstructionsEnum.add => LexicalTypesEnum.Addition,
        GenerationInstructionsEnum.sub => LexicalTypesEnum.Subtraction,
        GenerationInstructionsEnum.mul => LexicalTypesEnum.Multiplication,
        GenerationInstructionsEnum.div => LexicalTypesEnum.Division,
        GenerationInstructionsEnum.i2f => LexicalTypesEnum.Coercion,
        _ => LexicalTypesEnum.Unknown,
      };
    }


  }
}
