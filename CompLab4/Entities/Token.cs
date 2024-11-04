using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompLabs.Lexical;

namespace CompLabs.Entities
{
  /// <summary>
  /// Класс для описания токена
  /// </summary>
  internal class Token
  {
    /// <summary>
    /// Вид токена
    /// </summary>
    public LexicalTypesEnum Type { get; set; }

    /// <summary>
    /// Значение токена для переменных
    /// </summary>
    //public string Value { get; }
    public dynamic Value { get; }


    //public SymbolsDic Symbols { get; set; }
    /// <summary>
    /// Порядок токена для переменных
    /// </summary>
    public int IdentifierID { get; set; }

    /// <summary>
    /// Тип идентификатора
    /// </summary>
    public LexicalTypesEnum? SubType { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="type">Вид токена</param>
    /// <param name="value">Порядок токена для перемнных</param>
    public Token(LexicalTypesEnum type, dynamic value, int identifierID = -1, LexicalTypesEnum? subType = null/*SymbolsDic symbolsDic = null*/)
    {
      Type = type;
      Value = value;
      //Symbols = symbolsDic;
      IdentifierID = identifierID;
      SubType = subType;
    }



    /// <summary>
    /// Переопредленный метод toString()
    /// </summary>
    /// <returns>Форматированная строка</returns>
    public override string ToString()
    {
      string subTypeString = SubType.HasValue ? SubType.Value.ToDetailedString() : "нет типа";
      return Type == LexicalTypesEnum.Identifier
      ? $"<{Type.ToDescribString()},{IdentifierID}> - {Type.ToDetailedString()} {Value} {subTypeString}"
      : $"<{Value}> - {Type.ToDetailedString()}";
    }
  }
}
