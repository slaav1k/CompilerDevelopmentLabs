using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLab2
{
  /// <summary>
  /// Класс для описания токена
  /// </summary>
  internal class Token
  {
    /// <summary>
    /// Вид токена
    /// </summary>
    public LexicalTypesEnum Type { get; }

    /// <summary>
    /// Порядок токена для переменных
    /// </summary>
    public string Value { get; }


    public SymbolsDic Symbols { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="type">Вид токена</param>
    /// <param name="value">Порядок токена для перемнных</param>
    public Token(LexicalTypesEnum type, string value = null, SymbolsDic symbolsDic = null)
    {
      Type = type;
      Value = value;
      Symbols = symbolsDic;
    }

  

    /// <summary>
    /// Переопредленный метод toString()
    /// </summary>
    /// <returns>Форматированная строка</returns>
    public override string ToString()
    {
      return Type == LexicalTypesEnum.Identifier 
    ? $"<{Type},{Value}> - {Type.ToDetailedString()} {(Symbols != null ? Symbols.GetSymbolName(int.Parse(Value)) : "Символ не найден")}"
    : $"<{Value}> - {Type.ToDetailedString()}";
    }
  }
}
