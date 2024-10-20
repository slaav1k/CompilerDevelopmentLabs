using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompLab2
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Text.RegularExpressions;
  using System.Threading.Tasks;

  namespace CompLab2
  {
    /// <summary>
    /// Класс для точного анализа содержимого файла
    /// </summary>
    internal class LexicalHandler
    {
      /// <summary>
      /// Коллекция токенов
      /// </summary>
      private static List<Token> tokens = new List<Token>();

      /// <summary>
      /// Словарь идентификаторов
      /// </summary>
      private static SymbolsDic symbolsDic = new SymbolsDic();

      /// <summary>
      /// Указатель на текущий символ
      /// </summary>
      private static int position;


      /// <summary>
      /// Общий анализ содержимого файла
      /// </summary>
      /// <param name="input">Содержимое файла</param>
      /// <returns>Коллекция токенов</returns>
      /// <exception cref="Exception"></exception>
      public static List<Token> Analyze(string input)
      {
        position = 0;

        while (position < input.Length)
        {
          char currentChar = input[position];

          if (char.IsWhiteSpace(currentChar))
          {
            position++;
            continue;
          }

          if (char.IsLetter(currentChar) || currentChar == '_')
          {
            HandleIdentifier(input);
          }
          else if (char.IsDigit(currentChar) || currentChar == '.')
          {
            HandleNumber(input);
          }
          else if ("+-*/()".Contains(currentChar))
          {
            HandleOperatorOrParenthesis(currentChar);
          }
          else
          {
            throw new LexicalException($"Лексическая ошибка! Недопустимый символ '{currentChar}' на позиции {position + 1}");
          }
        }
        foreach (var token in tokens)
        {
          token.Symbols = symbolsDic;
        }
        return tokens;
      }

      /// <summary>
      /// Обработчик идентификаторов
      /// </summary>
      /// <param name="input">Содержимое файла</param>
      /// <exception cref="Exception"></exception>
      private static void HandleIdentifier(string input)
      {
        int start = position;
        while (position < input.Length && (char.IsLetterOrDigit(input[position]) || input[position] == '_'))
        {
          position++;
        }

        string identifier = input.Substring(start, position - start);

        if (char.IsDigit(identifier[0]))
        {
          throw new LexicalException($"Лексическая ошибка! Идентификатор '{identifier}' не может начинаться с цифры на позиции {start + 1}");
        }

        int id = symbolsDic.AddSymbol(identifier);
        tokens.Add(new Token(LexicalTypesEnum.Identifier, id.ToString()));
      }

      /// <summary>
      /// Обработчик числовых значений
      /// </summary>
      /// <param name="input">Содержимое файла</param>
      /// <exception cref="Exception"></exception>
      private static void HandleNumber(string input)
      {
        int start = position;
        bool hasDot = false;

        // Проверяем, не идет ли за числом символ, который недопустим в числах
        while (position < input.Length && (char.IsDigit(input[position]) || (input[position] == '.' && !hasDot)))
        {
          if (input[position] == '.')
          {
            hasDot = true;
          }
          position++;
        }

        // Проверяем, не начинается ли идентификатор с цифры (например, 1var1)
        if (position < input.Length && char.IsLetter(input[position]))
        {
          throw new LexicalException($"Лексическая ошибка! Идентификатор не может начинаться с цифры на позиции {start + 1}");
        }

        string number = input.Substring(start, position - start);

        if (Regex.IsMatch(number, @"^\d+(\.\d+)?$"))
        {
          string description = hasDot ? "константа вещественного типа" : "константа целого типа";
          var type = hasDot ? LexicalTypesEnum.RealConstant : LexicalTypesEnum.IntegerConstant;
          tokens.Add(new Token(type, number));
        }
        else
        {
          throw new LexicalException($"Лексическая ошибка! Неправильно задана константа '{number}' на позиции {start + 1}");
        }
      }

      /// <summary>
      /// Обработчик операторов и скобок
      /// </summary>
      /// <param name="currentChar">Текущий символ</param>
      private static void HandleOperatorOrParenthesis(char currentChar)
      {
        var type = currentChar switch
        {
          '+' => LexicalTypesEnum.Addition,
          '-' => LexicalTypesEnum.Subtraction,
          '*' => LexicalTypesEnum.Multiplication,
          '/' => LexicalTypesEnum.Division,
          '(' => LexicalTypesEnum.OpenParenthesis,
          ')' => LexicalTypesEnum.CloseParenthesis,
          _ => LexicalTypesEnum.Unknown
        };

        tokens.Add(new Token(type, currentChar.ToString()));
        position++;
      }


      /// <summary>
      /// Вернуть словарь идентификаторов
      /// </summary>
      /// <returns>Словарь идентификаторов</returns>
      public static SymbolsDic GetSymbolDictionary()
      {
        return symbolsDic;
      }
    }
  }

}
