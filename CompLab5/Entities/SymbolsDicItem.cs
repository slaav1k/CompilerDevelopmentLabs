using CompLabs.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLabs.Entities
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


    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name">Название идентификатора</param>
    /// <param name="type">Тип идентификатора</param>
    public SymbolsDicItem(string name, LexicalTypesEnum type) {
      Name = name;
      Type = type;
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
