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
    /// Словарь: идентификатор - порядковый номер
    /// </summary>
    private Dictionary<string, int> symbols;

    /// <summary>
    /// Счетчик
    /// </summary>
    private int counter;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    public SymbolsDic()
    {
      symbols = new Dictionary<string, int>();
      counter = 1;
    }

    /// <summary>
    /// Добаваление символа в словарь
    /// </summary>
    /// <param name="name">Идентификатор</param>
    /// <returns>Порядковый номер</returns>
    public int AddSymbol(string name)
    {
      if (!symbols.ContainsKey(name))
      {
        symbols[name] = counter++;
      }

      return symbols[name];
    }

    /// <summary>
    /// Вернуть коллекцию
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, int> GetSymbols()
    {
      return symbols;
    }
  }
}
