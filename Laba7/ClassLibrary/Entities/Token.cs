using ClassLiberty.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLiberty.Entities
{
  /// <summary>
  /// Класс для описания токена
  /// </summary>

  //[MessagePackObject]
  //internal class Token
  //{
  //  /// <summary>
  //  /// Вид токена
  //  /// </summary>
  //  [Key(0)]
  //  public LexicalTypesEnum Type { get; set; }

  //  /// <summary>
  //  /// Значение токена для переменных
  //  /// </summary>
  //  //public string Value { get; }
  //  //[MessagePack.IgnoreMember]
  //  [Key(1)]
  //  public dynamic Value { get; set; }



  //  //public SymbolsDic Symbols { get; set; }
  //  /// <summary>
  //  /// Порядок токена для переменных
  //  /// </summary>
  //  [Key(2)]
  //  public int IdentifierID { get; set; }

  //  /// <summary>
  //  /// Тип идентификатора
  //  /// </summary>
  //  [Key(3)]
  //  public LexicalTypesEnum? SubType { get; set; }

  //  /// <summary>
  //  /// Конструктор
  //  /// </summary>
  //  /// <param name="type">Вид токена</param>
  //  /// <param name="value">Порядок токена для перемнных</param>
  //  public Token(LexicalTypesEnum type, dynamic value, int identifierID = -1, LexicalTypesEnum? subType = null/*SymbolsDic symbolsDic = null*/)
  //  {
  //    Type = type;
  //    Value = value;
  //    //Symbols = symbolsDic;
  //    IdentifierID = identifierID;
  //    SubType = subType;
  //  }



  //  /// <summary>
  //  /// Переопредленный метод toString()
  //  /// </summary>
  //  /// <returns>Форматированная строка</returns>

  //  public override string ToString()
  //  {
  //    string formattedValue = Value is double || Value is float
  //        ? string.Format("{0:0.0####################}", Value)
  //        : Value.ToString();

  //    string subTypeString = SubType.HasValue ? SubType.Value.ToDetailedString() : "нет типа";
  //    return Type == LexicalTypesEnum.Identifier
  //        ? $"<{Type.ToDescribString()},{IdentifierID}> - {Type.ToDetailedString()} {formattedValue} {subTypeString}"
  //        : $"<{formattedValue}> - {Type.ToDetailedString()}";
  //  }

  //}



  internal class Token
  {
    public LexicalTypesEnum Type { get; set; }
    public dynamic Value { get; set; }
    public int IdentifierID { get; set; }
    public LexicalTypesEnum? SubType { get; set; }

    // Конструктор
    public Token(LexicalTypesEnum type, dynamic value, int identifierID = -1, LexicalTypesEnum? subType = null)
    {
      Type = type;
      Value = value;
      IdentifierID = identifierID;
      SubType = subType;
    }

    // Метод для сериализации в бинарный формат
    public void Serialize(BinaryWriter writer)
    {
      writer.Write((int)Type); 
      WriteValue(writer, Value); 
      writer.Write(IdentifierID); 
    }

    // Метод для десериализации из бинарного формата
    public static Token Deserialize(BinaryReader reader)
    {
      LexicalTypesEnum type = (LexicalTypesEnum)reader.ReadInt32();
      dynamic value = ReadValue(reader); 
      int identifierID = reader.ReadInt32(); 
      return new Token(type, value, identifierID); 
    }

    // Метод для записи значения (динамического типа)
    private static void WriteValue(BinaryWriter writer, dynamic value)
    {
      if (value is int)
      {
        writer.Write((byte)0); 
        writer.Write((int)value);
      }
      else if (value is double)
      {
        writer.Write((byte)1);
        writer.Write((double)value);
      }
      else if (value is string)
      {
        writer.Write((byte)2);
        writer.Write((string)value);
      }
      else
      {
        throw new InvalidCastException($"Unsupported value type: {value.GetType()}");
      }
    }

    // Метод для чтения значения (динамического типа)
    private static dynamic ReadValue(BinaryReader reader)
    {
      byte typeIndicator = reader.ReadByte(); // Читаем признак типа

      switch (typeIndicator)
      {
        case 0:
          return reader.ReadInt32(); // Читаем int
        case 1:
          return reader.ReadDouble(); // Читаем double
        case 2:
          return reader.ReadString(); // Читаем string
        default:
          throw new InvalidCastException($"Unsupported value type indicator: {typeIndicator}");
      }
    }

    // Переопределенный метод ToString()
    public override string ToString()
    {
      string formattedValue = Value is double || Value is float
          ? string.Format("{0:0.0####################}", Value)
          : Value.ToString();

      string subTypeString = SubType.HasValue ? SubType.Value.ToDetailedString() : "нет типа";
      return Type == LexicalTypesEnum.Identifier
          ? $"<{Type.ToDescribString()},{IdentifierID}> - {Type.ToDetailedString()} {formattedValue} {subTypeString}"
          : $"<{formattedValue}> - {Type.ToDetailedString()}";
    }
  }
}
