using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLab2
{

  /// <summary>
  /// Класс для обработки ошибок свзянных с лексичесим содержанием
  /// </summary>
  internal class LexicalException : Exception
  {
    public LexicalException(string message) : base(message) { }
  }
}
