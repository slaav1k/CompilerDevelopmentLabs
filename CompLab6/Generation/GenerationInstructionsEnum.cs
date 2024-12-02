using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLabs.Generation
{
  /// <summary>
  /// Перечисление для хранения команд
  /// </summary>
  public enum GenerationInstructionsEnum
  {
    add,    // команда сложения
    sub,    // команда вычитания
    mul,    // команда умножения
    div,    // команда деления
    i2f,    // команда преобразования типов
    unknown // неизвестная команда
  }
}
