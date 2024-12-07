using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLabs
{
  /// <summary>
  /// Перечисление для определения режимов работы утилиты
  /// </summary>
  internal enum AnalysisMode
  {
    LEX,  // Режим лексического анализа
    SYN,  // Режим синтаксического анализа
    SEM,  // Режим семантического анализа
    GEN1, // Режим генерации трехадресного кода
    GEN2, // Режим генерации постфиксной записи
    GEN3  // Режим генерации бинарного файла
  }
}
