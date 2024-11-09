using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CompLabs.Entities;
using CompLabs.Lexical;

namespace CompLabs.Generation
{
  /// <summary>
  /// Класс для генерации промежуточного кода и постфиксной нотации
  /// </summary>
  internal class GenereticHandler
  {
    private readonly SyntaxTree syntaxTree;
    private SymbolsDic symbolsDic;
    private int tempVarCounter;

    public GenereticHandler(SyntaxTree syntaxTree, SymbolsDic symbolsDic)
    {
      this.syntaxTree = syntaxTree;
      this.symbolsDic = symbolsDic;
      tempVarCounter = 1;  // Начинаем с временной переменной T1
    }

    /// <summary>
    /// Генерация трехадресного кода (GEN1)
    /// </summary>
    /// <returns>Трехадресный код</returns>
    public string GenerateThreeAddressCode()
    {
      var codeBuilder = new StringBuilder();
      TraverseExpression(syntaxTree.Root, codeBuilder);
      return codeBuilder.ToString();
    }

    /// <summary>
    /// Обход выражений и возвращение результата для трехадресного кода
    /// </summary>
    /// <param name="node">Узел выражения</param>
    /// <param name="codeBuilder">Строитель кода</param>
    /// <returns>Результат вычислений</returns>
    private string TraverseExpression(SyntaxTreeNode node, StringBuilder codeBuilder)
    {
      if (node == null)
        return string.Empty;

      // Если узел является операцией
      if (IsOperator(node.Token) && node.Children.Count == 2)
      {
        var leftResult = TraverseExpression(node.Children[0], codeBuilder);
        var rightResult = TraverseExpression(node.Children[1], codeBuilder);

        if (leftResult.Any(char.IsLetter))
        {
          leftResult = $"<id,{symbolsDic.AddSymbol(leftResult, LexicalTypesEnum.Unknown)}>";
        }

        if (rightResult.Any(char.IsLetter))
        {
          rightResult = $"<id,{symbolsDic.AddSymbol(rightResult, LexicalTypesEnum.Unknown)}>";
        }

        // Генерируем временную переменную для текущего узла-операции
        string tempVar = $"#T{tempVarCounter++}";
        LexicalTypesEnum type = LexicalTypesEnum.IntType;
        if (node.Token.SubType == LexicalTypesEnum.IntType)
        {
          type = LexicalTypesEnum.IntType;
        }
        else if (node.Token.SubType == LexicalTypesEnum.FloatType)
        {
          type = LexicalTypesEnum.FloatType;
        }

        int idTempVar = symbolsDic.AddSymbol(tempVar, type);
        codeBuilder.AppendLine($"{GenerationInstructionsFromLexicalTypes.ToGetInstruction(node.Token.Type)} <id,{idTempVar}> {leftResult} {rightResult}");

        return tempVar;
      }
      // Если узел является преобразованием типа
      else if (node.Token.Type == LexicalTypesEnum.Coercion && node.Children.Count == 1)
      {
        // Рекурсивно вычисляем выражение для дочернего узла
        var operandResult = TraverseExpression(node.Children[0], codeBuilder);
        if (operandResult.Any(char.IsLetter))
        {
          operandResult = $"<id,{symbolsDic.AddSymbol(operandResult, LexicalTypesEnum.Unknown)}>";
        }

        // Генерируем временную переменную для результата преобразования
        string tempVar = $"#T{tempVarCounter++}";
        int idTempVar = symbolsDic.AddSymbol(tempVar, LexicalTypesEnum.FloatType);
        codeBuilder.AppendLine($"{GenerationInstructionsEnum.i2f} <id,{idTempVar}> {operandResult}");

        return tempVar;
      }
      else if (node.Token.Type == LexicalTypesEnum.Identifier ||
              node.Token.Type == LexicalTypesEnum.IntegerConstant ||
              node.Token.Type == LexicalTypesEnum.RealConstant)
      {
        return Convert.ToString(node.Token.Value);
      }

      return string.Empty;
    }

    /// <summary>
    /// Генерация постфиксной записи (GEN2)
    /// </summary>
    /// <returns>Постфиксная запись</returns>
    public string GeneratePostfixNotation()
    {
      var postfixBuilder = new StringBuilder();
      TraverseTreeForPostfix(syntaxTree.Root, postfixBuilder);
      return postfixBuilder.ToString();
    }

    /// <summary>
    /// Обход дерева для постфиксной записи
    /// </summary>
    /// <param name="node">Текущий узел дерева</param>
    /// <param name="postfixBuilder">Строитель постфиксной записи</param>
    private void TraverseTreeForPostfix(SyntaxTreeNode node, StringBuilder postfixBuilder)
    {
      if (node == null)
        return;

      // Если узел является операцией
      if (IsOperator(node.Token))
      {
        // Если узел бинарный (двухоперандный)
        if (node.Children.Count == 2)
        {
          TraverseTreeForPostfix(node.Children[0], postfixBuilder); // Левый операнд
          TraverseTreeForPostfix(node.Children[1], postfixBuilder); // Правый операнд
          if (node.Token.Type == LexicalTypesEnum.Coercion)
          {
            postfixBuilder.Append($"<{GenerationInstructionsEnum.i2f}>");
          } else
          {
            postfixBuilder.Append($"<{node.Token.Value}>");
          }
        }
        // Если узел унарный (int2float)
        else if (node.Children.Count == 1)
        {
          TraverseTreeForPostfix(node.Children[0], postfixBuilder); 
          postfixBuilder.Append($"<{GenerationInstructionsEnum.i2f}>");
        }
      }
      // Если узел является идентификатором или константой
      else if (node.Token.Type == LexicalTypesEnum.Identifier)      
      {
        postfixBuilder.Append($"<id,{symbolsDic.AddSymbol(node.Token.Value, LexicalTypesEnum.Unknown)}>");
      }
      else if (node.Token.Type == LexicalTypesEnum.IntegerConstant || node.Token.Type == LexicalTypesEnum.RealConstant)
      {
        postfixBuilder.Append($"<{node.Token.Value}>");
      }
    }


    /// <summary>
    /// Проверка, является ли токен оператором
    /// </summary>
    /// <param name="token">Токен</param>
    /// <returns>True, если токен — это оператор</returns>
    private bool IsOperator(Token token)
    {
      return token.Type == LexicalTypesEnum.Addition ||
            token.Type == LexicalTypesEnum.Subtraction ||
            token.Type == LexicalTypesEnum.Multiplication ||
            token.Type == LexicalTypesEnum.Division ||
            token.Type == LexicalTypesEnum.Coercion;
    }

  }
}
