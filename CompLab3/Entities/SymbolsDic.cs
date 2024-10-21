using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLabs.Entities
{
  /// <summary>
  /// Класс для описания символов, идентификаторов
  /// </summary>
  internal class SymbolsDic
  {
    /// <summary>
    /// Словарь: порядковый номер - идентификатор
    /// </summary>
    private Dictionary<int, SymbolsDicItem> symbols;

    /// <summary>
    /// Счетчик
    /// </summary>
    private int counter;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    public SymbolsDic()
    {
        symbols = new Dictionary<int, SymbolsDicItem>();
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
        if (pair.Value.Name == name)
        {
            return pair.Key;
        }
      }

      symbols[counter] = new SymbolsDicItem(name);
      return counter++;

    }

    /// <summary>
    /// Вернуть коллекцию
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, SymbolsDicItem> GetSymbols()
    {
      return symbols;
    }


    /// <summary>
    /// Преобразование словаря символов в строку для записи в файл
    /// </summary>
    /// <returns>Строковое представление словаря</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      foreach (var symbol in symbols)
      {
        sb.AppendLine($"{symbol.Key} – {symbol.Value.Name}");
      }
      return sb.ToString();
    }

  }
}
