using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLab2
{
  /// <summary>
  /// Класс для описания символов, идентификаторов
  /// </summary>
  internal class SymbolsDic
  {
    /// <summary>
    /// Словарь: порядковый номер - идентификатор
    /// </summary>
    private Dictionary<int, string> symbols;

    /// <summary>
    /// Счетчик
    /// </summary>
    private int counter;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    public SymbolsDic()
    {
      symbols = new Dictionary<int, string>();
      counter = 1;
    }

    /// <summary>
    /// Добаваление символа в словарь
    /// </summary>
    /// <param name="name">Идентификатор</param>
    /// <returns>Порядковый номер</returns>
    public int AddSymbol(string name)
    {
      foreach (var pair in symbols)
      {
        if (pair.Value == name)
        {
          return pair.Key; 
        }
      }

      symbols[counter] = name; 
      return counter++;

    }

    /// <summary>
    /// Вернуть коллекцию
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, string> GetSymbols()
    {
      return symbols;
    }

  }
}
