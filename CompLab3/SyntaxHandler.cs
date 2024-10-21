using CompLabs.Entities;
using CompLabs.Exceptions;
using CompLabs.Lexical;
using System;
using System.Collections.Generic;

namespace CompLabs
{
  /// <summary>
  /// Класс для выполнения синтаксического анализа
  /// </summary>
  internal class SyntaxHandler
  {
    /// <summary>
    /// Список токенов
    /// </summary>
    private readonly List<Token> tokens;

    /// <summary>
    /// Текущий токен
    /// </summary>
    private int position;

    public SyntaxHandler(List<Token> tokens)
    {
      this.tokens = tokens;
      position = 0;
    }

    /// <summary>
    /// Выполнение синтаксического анализа и построение синтаксического дерева
    /// </summary>
    /// <returns>Синтаксическое дерево</returns>
    public SyntaxTree Analyze()
    {
      if (tokens == null || tokens.Count == 0)
      {
        throw new SyntaxException("Ошибка: Пустая последовательность токенов.");
      }
      SyntaxTreeNode root;

      // Запускаем метод, который проверит наличие оператора присваивания
      if (HasAssignmentOperator())
      {
        root = ParseAssignment(); // Если есть оператор присваивания, парсим присваивание
      }
      else
      {
        root = ParseExpression(); // Если оператора присваивания нет, просто парсим выражение
      }

      // Проверяем, остались ли лишние токены после анализа
      if (position < tokens.Count)
      {
        throw new SyntaxException($"Синтаксическая ошибка на позиции {position}: Лишние токены после выражения.");
      }

      return new SyntaxTree(root);
    }

    /// <summary>
    /// Проверка на наличие оператора присваивания
    /// </summary>
    /// <returns>True, если оператор присваивания найден</returns>
    private bool HasAssignmentOperator()
    {
      for (int i = 0; i < tokens.Count; i++)
      {
        if (tokens[i].Type == LexicalTypesEnum.Equal)
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Парсинг присваивания (например, id = expression)
    /// </summary>
    /// <returns>Узел синтаксического дерева</returns>
    private SyntaxTreeNode ParseAssignment()
    {
      var left = ParseTerm();

      // Проверяем наличие оператора присваивания
      if (position < tokens.Count && tokens[position].Type == LexicalTypesEnum.Equal)
      {
        var assignToken = tokens[position++];
        var right = ParseExpression();

        var assignNode = new SyntaxTreeNode(assignToken);
        assignNode.AddChild(left);
        assignNode.AddChild(right);

        return assignNode; // возвращаем узел с оператором присваивания
      }

      return left; // Если нет присваивания, возвращаем левую часть
    }

    /// <summary>
    /// Парсинг выражения
    /// </summary>
    /// <returns>Узел синтаксического дерева</returns>
    private SyntaxTreeNode ParseExpression()
    {
      var left = ParseTerm(); // Сначала разбираем терм

      while (position < tokens.Count && IsAdditionOrSubtraction(tokens[position]))
      {
        var operatorToken = tokens[position++];
        var right = ParseTerm(); // Обрабатываем следующий терм

        var operatorNode = new SyntaxTreeNode(operatorToken);
        operatorNode.AddChild(left);
        operatorNode.AddChild(right);

        left = operatorNode; // Обновляем левую часть для следующего цикла
      }

      return left; // Возвращаем корень выражения
    }

    /// <summary>
    /// Парсинг терма (например, числа или идентификатора)
    /// </summary>
    /// <returns>Узел синтаксического дерева</returns>
    private SyntaxTreeNode ParseTerm()
    {
      var left = ParseFactor(); // Разбираем "фактор" (идентификаторы, константы и выражения в скобках)

      while (position < tokens.Count && IsMultiplicationOrDivision(tokens[position]))
      {
        var operatorToken = tokens[position++];
        var right = ParseFactor(); // Обрабатываем следующий фактор

        var operatorNode = new SyntaxTreeNode(operatorToken);
        operatorNode.AddChild(left);
        operatorNode.AddChild(right);

        left = operatorNode; // Обновляем левую часть для следующего цикла
      }

      return left; // Возвращаем корень терма
    }


    private SyntaxTreeNode ParseFactor()
    {
      if (position >= tokens.Count)
      {
        throw new SyntaxException($"Синтаксическая ошибка: Ожидался операнд на позиции {position}.");
      }

      var currentToken = tokens[position];

      if (currentToken.Type == LexicalTypesEnum.Identifier || currentToken.Type == LexicalTypesEnum.IntegerConstant
          || currentToken.Type == LexicalTypesEnum.RealConstant)
      {
        position++; // Переходим к следующему токену
        return new SyntaxTreeNode(currentToken);
      }
      else if (currentToken.Type == LexicalTypesEnum.OpenParenthesis)
      {
        position++; // Пропускаем открывающую скобку
        var innerExpression = ParseExpression(); // Разбираем выражение в скобках

        if (position >= tokens.Count || tokens[position].Type != LexicalTypesEnum.CloseParenthesis)
        {
          throw new SyntaxException($"Синтаксическая ошибка: Ожидалась закрывающая скобка на позиции {position}.");
        }

        position++; // Пропускаем закрывающую скобку
        return innerExpression;
      }
      else
      {
        throw new SyntaxException($"Синтаксическая ошибка: Неправильный операнд на позиции {position}.");
      }
    }


    /// <summary>
    /// Проверка, является ли токен оператором
    /// </summary>
    /// <param name="token">Токен</param>
    /// <returns>True, если токен — оператор</returns>
    private bool IsOperator(Token token)
    {
      return token.Type == LexicalTypesEnum.Addition ||
             token.Type == LexicalTypesEnum.Subtraction ||
             token.Type == LexicalTypesEnum.Multiplication ||
             token.Type == LexicalTypesEnum.Division;
    }

    private bool IsAdditionOrSubtraction(Token token)
    {
      return token.Type == LexicalTypesEnum.Addition || token.Type == LexicalTypesEnum.Subtraction;
    }

    private bool IsMultiplicationOrDivision(Token token)
    {
      return token.Type == LexicalTypesEnum.Multiplication || token.Type == LexicalTypesEnum.Division;
    }
  }
}
