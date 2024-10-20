using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLab2
{
  /// <summary>
  /// Перечисление для хранения типов токенов
  /// </summary>
  public enum LexicalTypesEnum
  {
    Identifier,        // Идентификатор (переменная или имя функции)
    IntegerConstant,   // Константа целого типа
    RealConstant,      // Константа вещественного типа
    Addition,          // Оператор сложения
    Subtraction,       // Оператор вычитания
    Multiplication,    // Оператор умножения
    Division,          // Оператор деления
    OpenParenthesis,   // Открывающая скобка
    CloseParenthesis,  // Закрывающая скобка
    Unknown            // Неизвестный символ
  }
}

