using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLab2
{
  /// <summary>
  /// Класс для обработки ошибок свзянных с файлами
  /// </summary>
  internal class FileException : Exception
  {
    public FileException(string message) : base(message) { }
  }
}
