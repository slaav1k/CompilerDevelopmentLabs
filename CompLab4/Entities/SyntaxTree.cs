using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompLabs.Entities
{
  /// <summary>
  /// Синтаксическое дерево
  /// </summary>
  internal class SyntaxTree
  {
    /// <summary>
    /// Корневой узел дерева
    /// </summary>
    public SyntaxTreeNode Root { get; set; }

    /// <summary>
    /// Конструктор дерева с корневым узлом
    /// </summary>
    /// <param name="root">Корневой узел</param>
    public SyntaxTree(SyntaxTreeNode root)
    {
      Root = root;
    }

    /// <summary>
    /// Вывод дерева в виде строки
    /// </summary>
    /// <returns>Текстовое представление дерева</returns>
    public override string ToString()
    {
      return Root != null ? Root.PrintTree() : string.Empty;
    }
  }
}
