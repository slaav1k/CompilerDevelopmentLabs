
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
  /// Класс для хранения информации об идентификаторах
  /// </summary>
  internal class SymbolsDicItem
  {
    /// <summary>
    /// Название идентификатора
    /// </summary>
    public string Name {  get; set; }


    /// <summary>
    /// Тип идентификатора
    /// </summary>
    public LexicalTypesEnum Type { get; set; }


    public dynamic Value { get; set; }


    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name">Название идентификатора</param>
    /// <param name="type">Тип идентификатора</param>
    public SymbolsDicItem(string name, LexicalTypesEnum type, dynamic value = null)
    {
      Name = name;
      Type = type;
      Value = value;
    }


    /// <summary>
    /// Метод сериализации
    /// </summary>
    /// <param name="writer">Бинарный писатель</param>
    public void Serialize(BinaryWriter writer)
    {
      writer.Write(Name);
      writer.Write((int)Type);
    }

    /// <summary>
    /// Метод десериализации
    /// </summary>
    /// <param name="reader">Бинарный читатель</param>
    /// <returns>Объект SymbolsDicItem</returns>
    public static SymbolsDicItem Deserialize(BinaryReader reader)
    {
      string name = reader.ReadString();
      LexicalTypesEnum type = (LexicalTypesEnum)reader.ReadInt32();
      return new SymbolsDicItem(name, type);
    }


    /// <summary>
    /// Переопределенный метод ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      //return Name;
      return $"{Name} [{Type}]";
    }


  }
}
