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
    /// Конструктор
    /// </summary>
    /// <param name="name">Название идентификатора</param>
    public SymbolsDicItem(string name) {
      Name = name;
    }


    /// <summary>
    /// Переопределенный метод ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return Name;
    }


  }
}
