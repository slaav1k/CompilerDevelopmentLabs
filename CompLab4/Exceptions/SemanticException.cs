using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLabs.Exceptions
{
  /// <summary>
  /// Исключения для семантических ошибок
  /// </summary>
  internal class SemanticException : Exception
  {
    public SemanticException(string message) : base(message) { }
  }
}
