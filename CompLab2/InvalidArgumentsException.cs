﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLab2
{
  /// <summary>
  /// Класс для обработки ошибок связанных с количеством передаваемых аргументов
  /// </summary>
  internal class InvalidArgumentsException : Exception
  {
    public InvalidArgumentsException(string message) : base(message) { }
  }

}
