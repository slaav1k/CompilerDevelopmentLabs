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
    public string Type { get; }

    /// <summary>
    /// Порядок токена для переменных
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Описание токена
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="type">Вид токена</param>
    /// <param name="description">Описание токена</param>
    /// <param name="value">Порядок токена для перемнных</param>
    public Token(string type, string description, string value = null)
    {
      Type = type;
      Value = value;
      Description = description;
    }

    /// <summary>
    /// Переопредленный метод toString()
    /// </summary>
    /// <returns>Форматированная строка</returns>
    public override string ToString()
    {
      return Value != null ? $"<{Type},{Value}> - {Description}" : $"<{Type}> - {Description}";
    }
  }
}
