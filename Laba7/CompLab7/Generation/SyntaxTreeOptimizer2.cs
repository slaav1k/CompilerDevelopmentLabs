﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClassLiberty.Enums;
using ClassLiberty.Entities;
using System;

namespace CompLabs.Generation
{
  internal class SyntaxTreeOptimizer2
  {
    public static SyntaxTree OptimizeTree(SyntaxTree tree)
    {
      // Рекурсивный вызов с корня дерева
      OptimizeNode(tree?.Root);
      return tree;
    }

    private static void OptimizeNode(SyntaxTreeNode node)
    {
      // Если узел null, сразу выходим
      if (node == null) return;

      // Рекурсивно обрабатываем дочерние узлы
      foreach (var child in node.Children)
      {
        OptimizeNode(child);
      }

      // Оптимизация текущего узла
      OptimizeExpression(node);
    }

    private static void OptimizeExpression(SyntaxTreeNode node)
    {
      // 1. Проверка и оптимизация выражений с константами
      if (IsOperator(node.Token) && node.Children.Count == 2)
      {
        var leftChild = node.Children[0];
        var rightChild = node.Children[1];

        // Если оба операнда константы, считаем выражение заранее
        if (IsConstant(leftChild.Token) && IsConstant(rightChild.Token))
        {
          double leftValue = Convert.ToDouble(leftChild.Token.Value);
          double rightValue = Convert.ToDouble(rightChild.Token.Value);
          double result = 0;
          LexicalTypesEnum resType = LexicalTypesEnum.IntegerConstant;

          // Определение типа результата операции
          if (leftChild.Token.Type == LexicalTypesEnum.RealConstant || rightChild.Token.Type == LexicalTypesEnum.RealConstant)
          {
            resType = LexicalTypesEnum.RealConstant;
          }

          // Операции
          switch (node.Token.Type)
          {
            case LexicalTypesEnum.Addition:
              result = leftValue + rightValue;
              break;
            case LexicalTypesEnum.Subtraction:
              result = leftValue - rightValue;
              break;
            case LexicalTypesEnum.Multiplication:
              result = leftValue * rightValue;
              break;
            case LexicalTypesEnum.Division:
              // Добавляем проверку деления на 0
              if (rightValue != 0)
              {
                result = leftValue / rightValue;
                if (result.GetType() == typeof(double))
                {
                  resType = LexicalTypesEnum.RealConstant;
                }
              }
              else
              {
                // Обработка деления на 0 (можно, например, выбросить ошибку или заменить на индикацию ошибки)
                result = double.NaN;
                resType = LexicalTypesEnum.Unknown;
              }
              break;
            default:
              break;
          }

          // Если результат операции является вещественным, создаем токен типа RealConstant
          if (resType == LexicalTypesEnum.RealConstant)
          {
            node.Token = new Token(LexicalTypesEnum.RealConstant, result, -1, LexicalTypesEnum.FloatType);
          }
          // Если результат операции целочисленный, приводим его к int и создаем токен типа IntegerConstant
          else
          {
            // Явное приведение типа для целочисленных операций
            node.Token = new Token(LexicalTypesEnum.IntegerConstant, (int)result, -1, LexicalTypesEnum.IntType);
          }

          // Оставляем один результат, дочерние узлы удаляем, так как они больше не нужны
          node.Children.Clear();
        }
        else if (leftChild.Token.Type == LexicalTypesEnum.Coercion && leftChild.Children.Count == 1)
        {
          leftChild = leftChild.Children[0];
        }
        else if (rightChild.Token.Type == LexicalTypesEnum.Coercion && rightChild.Children.Count == 1)
        {
          rightChild = rightChild.Children[0];
        }

      }

      // 2. Оптимизация преобразований int2float
      if (node.Token.Type == LexicalTypesEnum.Coercion && node.Children.Count == 1)
      {
        var child = node.Children[0];
        if (IsConstant(child.Token) && child.Token.Type == LexicalTypesEnum.IntegerConstant)
        {
          double floatValue = Convert.ToDouble(child.Token.Value);
          node.Token = new Token(LexicalTypesEnum.RealConstant, floatValue + 0.0, -1, LexicalTypesEnum.FloatType);
          node.Children = child.Children;
        }
      }

      // 3. Оптимизация тривиальных операций
      if ((node.Token.Type == LexicalTypesEnum.Addition && node.Children.Count == 2) || (node.Token.Type == LexicalTypesEnum.Subtraction && node.Children.Count == 2))
      {
        var leftChild = node.Children[0];
        var rightChild = node.Children[1];

        // A +/- 0 -> A
        if (IsConstant(rightChild.Token) && Convert.ToDouble(rightChild.Token.Value) == 0)
        {
          node.Token = leftChild.Token;
          node.Children = leftChild.Children;
        }
        else if (IsConstant(leftChild.Token) && Convert.ToDouble(leftChild.Token.Value) == 0)
        {
          node.Token = rightChild.Token;
          node.Children = rightChild.Children;
        }
      }
      else if (node.Token.Type == LexicalTypesEnum.Multiplication && node.Children.Count == 2)
      {
        var leftChild = node.Children[0];
        var rightChild = node.Children[1];

        // A * 0 -> 0
        if (IsConstant(rightChild.Token) && Convert.ToDouble(rightChild.Token.Value) == 0)
        {
          node.Token = new Token(rightChild.Token.Type, 0, -1, rightChild.Token.SubType);
          node.Children.Clear();
        }
        else if (IsConstant(leftChild.Token) && Convert.ToDouble(leftChild.Token.Value) == 0)
        {
          node.Token = new Token(leftChild.Token.Type, 0, -1, leftChild.Token.SubType);
          node.Children.Clear();
        }
        // A * 1 -> A
        else if (IsConstant(rightChild.Token) && Convert.ToDouble(rightChild.Token.Value) == 1)
        {
          node.Token = leftChild.Token;
          node.Children = leftChild.Children;
        }
        else if (IsConstant(leftChild.Token) && Convert.ToDouble(leftChild.Token.Value) == 1)
        {
          node.Token = rightChild.Token;
          node.Children = rightChild.Children;
        }
      }
      else if (node.Token.Type == LexicalTypesEnum.Division && node.Children.Count == 2)
      {
        var leftChild = node.Children[0];
        var rightChild = node.Children[1];

        // A / 1 -> A
        if (IsConstant(rightChild.Token) && Convert.ToDouble(rightChild.Token.Value) == 1)
        {
          node.Token = leftChild.Token;
          node.Children = leftChild.Children;
        }
      }
    }



    private static bool IsOperator(Token token)
    {
      return token.Type == LexicalTypesEnum.Addition ||
             token.Type == LexicalTypesEnum.Subtraction ||
             token.Type == LexicalTypesEnum.Multiplication ||
             token.Type == LexicalTypesEnum.Division;
    }

    private static bool IsConstant(Token token)
    {
      return token.Type == LexicalTypesEnum.IntegerConstant || token.Type == LexicalTypesEnum.RealConstant;
    }
  }
}



