
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
    public int AddSymbol(string name, LexicalTypesEnum type)
    {
      foreach (var pair in symbols)
      {
        if (pair.Value.Name == name)
        {
            return pair.Key;
        }
      }

      symbols[counter] = new SymbolsDicItem(name, type);
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
    /// Перегруженные скобочки
    /// </summary>
    /// <param name="id">Ключ</param>
    /// <returns>Идентификатор</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public SymbolsDicItem this[int id]
    {
      get
      {
        if (symbols.ContainsKey(id))
          return symbols[id];
        throw new KeyNotFoundException("Ключ не найден в словаре.");
      }
    }


    /// <summary>
    /// Метод для получения значения из символа по его ID
    /// </summary>
    /// <param name="id">ID символа</param>
    /// <returns>Значение символа</returns>
    public dynamic GetValue(int id)
    {
      if (symbols.ContainsKey(id))
      {
        return symbols[id].Value;
      }
      else
      {
        throw new KeyNotFoundException("Идентификатор не найден.");
      }
    }

    /// <summary>
    /// Метод для установки значения для символа по его ID
    /// </summary>
    /// <param name="id">ID символа</param>
    /// <param name="value">Значение для установки</param>
    public void SetValue(int id, dynamic value)
    {
      if (symbols.ContainsKey(id))
      {
        symbols[id].Value = value;
      }
      else
      {
        throw new KeyNotFoundException("Идентификатор не найден.");
      }
    }

    // Сериализация
    public void Serialize(BinaryWriter writer)
    {
      writer.Write(symbols.Count);
      foreach (var pair in symbols)
      {
        writer.Write(pair.Key); 
        pair.Value.Serialize(writer);
      }
    }

    // Десериализация
    public static SymbolsDic Deserialize(BinaryReader reader)
    {
      var symbolsDic = new SymbolsDic();
      int count = reader.ReadInt32();

      for (int i = 0; i < count; i++)
      {
        int key = reader.ReadInt32();  
        var item = SymbolsDicItem.Deserialize(reader); 
        symbolsDic.symbols[key] = item; 
      }

      return symbolsDic;
    }

    /// <summary>
    /// Метод для удаления символа из словаря по его имени
    /// </summary>
    /// <param name="name">Имя символа для удаления</param>
    /// <returns>True, если символ был удален; False, если символ с таким именем не найден</returns>
    public bool RemoveSymbolByName(string name)
    {
      // Находим ключ по имени
      var keyToRemove = symbols.FirstOrDefault(x => x.Value.Name == name).Key;

      // Если ключ найден, удаляем запись
      if (keyToRemove != 0)
      {
        return symbols.Remove(keyToRemove);
      }

      // Если элемент с таким именем не найден
      return false;
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
        sb.AppendLine($"{symbol.Key} – {symbol.Value.Name} [{symbol.Value.Type.ToDescribString()}]");
      }
      return sb.ToString();
    }

  }
}
