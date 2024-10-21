using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLabs.Exceptions
{
  /// <summary>
  /// Исключение для синтаксических ошибок
  /// </summary>
  internal class SyntaxException : Exception
  {
    public SyntaxException(string message) : base(message) { }
  }
}
