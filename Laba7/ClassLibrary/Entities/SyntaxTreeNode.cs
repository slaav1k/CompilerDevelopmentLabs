
using ClassLiberty.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLiberty.Entities
{
  /// <summary>
  /// Узел синтаксического дерева
  /// </summary>
  internal class SyntaxTreeNode
  {
    /// <summary>
    /// Сам узел - токен (например, оператор, идентификатор, число)
    /// </summary>
    public Token Token { get; set; }

    /// <summary>
    /// Дочерние узлы
    /// </summary>
    public List<SyntaxTreeNode> Children { get; set; }

    /// <summary>
    /// Конструктор узла дерева
    /// </summary>
    /// <param name="token">Узел, токен</param>
    public SyntaxTreeNode(Token token)
    {
      Token = token;
      Children = new List<SyntaxTreeNode>();
    }

    /// <summary>
    /// Добавление дочернего узла
    /// </summary>
    /// <param name="child">Дочерний узел</param>
    public void AddChild(SyntaxTreeNode child)
    {
      Children.Add(child);
    }

    /// <summary>
    /// Рекурсивная печать дерева с отступами
    /// </summary>
    /// <param name="depth">Глубина узла</param>
    /// <returns>Текстовое представление дерева</returns>
    public string PrintTree(int depth = 0)
    {
      //String tokenStr = Token.ToString().Split(" - ")[0];
      String tokenStr = Token.ToString().Split(new[] { " - " }, StringSplitOptions.None)[0];

      StringBuilder builder = new StringBuilder();

      string subTypeRepresentation = Token.SubType switch
      {
        LexicalTypesEnum.IntType => "[I]",
        LexicalTypesEnum.FloatType => "[F]",
        // Добавьте другие типы при необходимости
        _ => $"[{Token.SubType}]" // Для других типов по умолчанию
      };

      // Добавляем representation к tokenStr, если это идентификатор
      if (Token.Type == LexicalTypesEnum.Identifier)
      {
        tokenStr += subTypeRepresentation;
      }

      if (depth > 0)
      {
        builder.AppendLine(new string(' ', depth * 2 - 2) + "|-" + tokenStr);
      }
      else
      {
        builder.AppendLine(new string('-', depth * 2) + tokenStr);
      }


      foreach (var child in Children)
      {
        builder.Append(child.PrintTree(depth + 1));
      }

      return builder.ToString();
    }
  }
}
