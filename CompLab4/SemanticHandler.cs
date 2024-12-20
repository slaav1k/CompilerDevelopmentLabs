﻿using CompLabs.Entities;
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

    //private SyntaxTreeNode? AnalyzeNode(SyntaxTreeNode node)
    //{
    //  if (node == null) return null;

    //  // Проверка на количество дочерних узлов перед доступом к ним
    //  SyntaxTreeNode? leftNode = node.Children.Count > 0 ? AnalyzeNode(node.Children[0]) : null;
    //  SyntaxTreeNode? rightNode = node.Children.Count > 1 ? AnalyzeNode(node.Children[1]) : null;

    //  // Проверка на деление на ноль
    //  if (node.Token.Type == LexicalTypesEnum.Division && rightNode != null)
    //  {
    //    if (rightNode.Token.Type == LexicalTypesEnum.IntegerConstant && (int)rightNode.Token.Value == 0)
    //    {
    //      throw new SemanticException("Семантическая ошика: Деление на ноль обнаружено.");
    //    }
    //    else if (rightNode.Token.Type == LexicalTypesEnum.RealConstant && (double)rightNode.Token.Value == 0.0)
    //    {
    //      throw new SemanticException("Семантическая ошика: Деление на ноль обнаружено.");
    //    }
    //  }

    //  // Проверка операторов и приведение типов
    //  if (IsOperator(node.Token.Type) && leftNode != null && rightNode != null)
    //  {
    //    var leftType = leftNode.Token.SubType;
    //    var rightType = rightNode.Token.SubType;

    //    // Если типы не совпадают, обрабатываем приведение типов
    //    if (leftType != rightType)
    //    {
    //      var coercionNode = new SyntaxTreeNode(new Token(LexicalTypesEnum.Coercion, LexicalTypesEnum.Coercion.ToDescribString()));
    //      HandleTypeCoercion(node, leftNode, rightNode, coercionNode);
    //    }

    //    // Установка типа возвращаемого значения
    //    if (leftType == LexicalTypesEnum.FloatType || rightType == LexicalTypesEnum.FloatType)
    //    {
    //      //node.Token.Type = LexicalTypesEnum.RealConstant; // Операция возвращает вещественное число
    //      node.Token.SubType = LexicalTypesEnum.FloatType;
    //    }
    //    else
    //    {
    //      //node.Token.Type = LexicalTypesEnum.IntegerConstant; // Операция возвращает целое число
    //      node.Token.SubType = LexicalTypesEnum.IntType;
    //    }

    //    // Установка типа для идентификаторов
    //    if (node.Children[0].Token.Type == LexicalTypesEnum.Identifier)
    //    {
    //      node.Token.SubType = node.Children[0].Token.SubType;
    //    }
    //    else if (node.Children[0].Token.Type == LexicalTypesEnum.IntegerConstant || node.Children[0].Token.Type == LexicalTypesEnum.RealConstant)
    //    {
    //      //node.Token.Type = node.Children[0].Token.Type;
    //      node.Token.SubType = node.Children[0].Token.SubType;
    //    }
    //  }

    //  return node;
    //}


    //// Вынесение логики приведения типов в отдельный метод
    //private void HandleTypeCoercion(SyntaxTreeNode node, SyntaxTreeNode leftNode, SyntaxTreeNode rightNode, SyntaxTreeNode coercionNode)
    //{
    //  // Левый операнд - целое число, правый операнд - вещественное число
    //  if (leftNode.Token.Type == LexicalTypesEnum.IntegerConstant && rightNode.Token.Type == LexicalTypesEnum.RealConstant)
    //  {
    //    leftNode.Token.Type = LexicalTypesEnum.RealConstant; 
    //    leftNode.Token.SubType = LexicalTypesEnum.FloatType;
    //    coercionNode.Children.Add(leftNode); 
    //    node.Children[0] = coercionNode; 
    //  }
    //  // Левый операнд - вещественное число, правый операнд - целое число
    //  else if (leftNode.Token.Type == LexicalTypesEnum.RealConstant && rightNode.Token.Type == LexicalTypesEnum.IntegerConstant)
    //  {
    //    rightNode.Token.Type = LexicalTypesEnum.RealConstant; 
    //    rightNode.Token.SubType = LexicalTypesEnum.FloatType;
    //    coercionNode.Children.Add(rightNode); 
    //    node.Children[1] = coercionNode; 
    //  }
    //  // Левый операнд - целое число, правый операнд - вещественный идентификатор
    //  else if (leftNode.Token.Type == LexicalTypesEnum.IntegerConstant && rightNode.Token.Type == LexicalTypesEnum.Identifier
    //           && rightNode.Token.SubType == LexicalTypesEnum.FloatType)
    //  {
    //    leftNode.Token.Type = LexicalTypesEnum.RealConstant; 
    //    leftNode.Token.SubType = LexicalTypesEnum.FloatType;
    //    coercionNode.Children.Add(leftNode); 
    //    node.Children[0] = coercionNode; 
    //  }
    //  // Левый операнд - вещественный идентификатор, правый операнд - целое число
    //  else if (rightNode.Token.Type == LexicalTypesEnum.IntegerConstant && leftNode.Token.Type == LexicalTypesEnum.Identifier
    //           && leftNode.Token.SubType == LexicalTypesEnum.FloatType)
    //  {
    //    rightNode.Token.Type = LexicalTypesEnum.RealConstant; 
    //    rightNode.Token.SubType = LexicalTypesEnum.FloatType;
    //    coercionNode.Children.Add(rightNode); 
    //    node.Children[1] = coercionNode; 
    //  }
    //  // Левый операнд - вещественное число, правый операнд - целый идентификатор
    //  else if (leftNode.Token.Type == LexicalTypesEnum.RealConstant && rightNode.Token.Type == LexicalTypesEnum.Identifier
    //           && rightNode.Token.SubType == LexicalTypesEnum.IntType)
    //  {
    //    rightNode.Token.SubType = LexicalTypesEnum.FloatType; 
    //    coercionNode.Children.Add(rightNode); 
    //    node.Children[1] = coercionNode; 
    //  }
    //  // Левый операнд - целый идентификатор, правый операнд - вещественное число
    //  else if (rightNode.Token.Type == LexicalTypesEnum.RealConstant && leftNode.Token.Type == LexicalTypesEnum.Identifier
    //           && leftNode.Token.SubType == LexicalTypesEnum.IntType)
    //  {
    //    leftNode.Token.SubType = LexicalTypesEnum.FloatType; 
    //    coercionNode.Children.Add(leftNode); 
    //    node.Children[0] = coercionNode; 
    //  }
    //  // Левый операнд - целый идентификатор, правый операнд - вещественный идентификатор
    //  else if (leftNode.Token.Type == LexicalTypesEnum.Identifier && leftNode.Token.SubType == LexicalTypesEnum.IntType
    //           && rightNode.Token.Type == LexicalTypesEnum.Identifier && rightNode.Token.SubType == LexicalTypesEnum.FloatType)
    //  {
    //    leftNode.Token.SubType = LexicalTypesEnum.FloatType; 
    //    coercionNode.Children.Add(leftNode); 
    //    node.Children[0] = coercionNode; 
    //  }
    //  // Левый операнд - вещественный идентификатор, правый операнд - целый идентификатор
    //  else if (rightNode.Token.Type == LexicalTypesEnum.Identifier && rightNode.Token.SubType == LexicalTypesEnum.IntType
    //           && leftNode.Token.Type == LexicalTypesEnum.Identifier && leftNode.Token.SubType == LexicalTypesEnum.FloatType)
    //  {
    //    rightNode.Token.SubType = LexicalTypesEnum.FloatType; 
    //    coercionNode.Children.Add(rightNode); 
    //    node.Children[1] = coercionNode; 
    //  }
    //}

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
