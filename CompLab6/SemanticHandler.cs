using CompLabs.Entities;
using CompLabs.Exceptions;
using CompLabs.Lexical;
using System;

namespace CompLabs
{
  internal class SemanticHandler
  {

    private readonly SymbolsDic _symbolsDic;

    public SemanticHandler(SyntaxTree tree, SymbolsDic symbolsDic)
    {
      _symbolsDic = symbolsDic;
      AnalyzeNode(tree.Root);
    }

    /// <summary>
    /// Рекурсивный обход узлов синтаксического дерева
    /// </summary>
    /// <param name="node">Текущий узел</param>
    private void AnalyzeNode(SyntaxTreeNode node)
    {
      if (node == null) return;

      // Выполняем проверку типов и приведение, если необходимо
      EnsureTypeCompatibility(node);

      // Рекурсивно анализируем все дочерние узлы
      foreach (var child in node.Children)
      {
        AnalyzeNode(child);
      }
    }

    /// <summary>
    /// Проверка совместимости типов
    /// </summary>
    /// <param name="node">Текущий узел</param>
    private void EnsureTypeCompatibility(SyntaxTreeNode node)
    {
      if (node == null || node.Children.Count < 2) return;

      var leftNode = node.Children[0];
      var rightNode = node.Children[1];

      // Проверка на деление на ноль
      if (node.Token.Type == LexicalTypesEnum.Division && rightNode != null)
      {
        if (rightNode.Token.Type == LexicalTypesEnum.IntegerConstant && (int)rightNode.Token.Value == 0)
        {
          throw new SemanticException("Семантическая ошика: Деление на ноль обнаружено.");
        }
        else if (rightNode.Token.Type == LexicalTypesEnum.RealConstant && (double)rightNode.Token.Value == 0.0)
        {
          throw new SemanticException("Семантическая ошика: Деление на ноль обнаружено.");
        }
      }

      // Рекурсивно проверяем типы дочерних узлов
      EnsureTypeCompatibility(leftNode);
      EnsureTypeCompatibility(rightNode);

      // Определяем типы через таблицу символов, если узел — идентификатор
      var leftType = leftNode.Token.Type == LexicalTypesEnum.Identifier
          ? _symbolsDic[leftNode.Token.IdentifierID].Type
          : leftNode.Token.SubType;

      var rightType = rightNode.Token.Type == LexicalTypesEnum.Identifier
          ? _symbolsDic[rightNode.Token.IdentifierID].Type
          : rightNode.Token.SubType;

      // Если типы различаются, добавляем узел для приведения типа
      if (leftType != rightType)
      {
        SyntaxTreeNode coercionNode = new SyntaxTreeNode(new Token(LexicalTypesEnum.Coercion, LexicalTypesEnum.Coercion.ToDescribString()));

        if (leftType == LexicalTypesEnum.IntType && rightType == LexicalTypesEnum.FloatType)
        {
          coercionNode.Children.Add(leftNode);
          node.Children[0] = coercionNode;
        }
        else if (rightType == LexicalTypesEnum.IntType && leftType == LexicalTypesEnum.FloatType)
        {
          coercionNode.Children.Add(rightNode);
          node.Children[1] = coercionNode;
        }
      }

      // Устанавливаем тип узла
      node.Token.SubType = (leftType == LexicalTypesEnum.FloatType || rightType == LexicalTypesEnum.FloatType)
          ? LexicalTypesEnum.FloatType
          : LexicalTypesEnum.IntType;
    }






    /// <summary>
    /// Проверяет, является ли токен оператором.
    /// </summary>
    /// <param name="type">Тип токена</param>
    /// <returns>true, если это оператор; в противном случае false</returns>
    private bool IsOperator(LexicalTypesEnum type)
    {
      return type == LexicalTypesEnum.Addition ||
             type == LexicalTypesEnum.Subtraction ||
             type == LexicalTypesEnum.Multiplication ||
             type == LexicalTypesEnum.Division;
    }
  }
}
