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
    private List<Instruction> instructions;
    private List<Token> postfixTokens;

    public GenereticHandler(SyntaxTree syntaxTree, SymbolsDic symbolsDic)
    {
      this.syntaxTree = syntaxTree;
      this.symbolsDic = symbolsDic;
      this.instructions = new List<Instruction>();
      this.postfixTokens = new List<Token>();
      tempVarCounter = 1;  // Начинаем с временной переменной T1
    }

    /// <summary>
    /// Генерация трехадресного кода (GEN1)
    /// </summary>
    /// <returns>Трехадресный код</returns>
    public List<Instruction> GenerateThreeAddressCode()
    {
      var codeBuilder = new StringBuilder();
      TraverseExpression(syntaxTree.Root);
      return this.instructions;
    }

    /// <summary>
    /// Обход выражений и возвращение результата для трехадресного кода
    /// </summary>
    /// <param name="node">Узел выражения</param>\
    /// <returns>Результат вычислений</returns>
    private Token TraverseExpression(SyntaxTreeNode node)
    {
      if (node == null)
        return null;

      // Если узел является операцией
      if (IsOperator(node.Token) && node.Children.Count == 2)
      {
        var leftResult = TraverseExpression(node.Children[0]);
        var rightResult = TraverseExpression(node.Children[1]);

        // Генерируем временную переменную для текущего узла-операции
        string tempVarName = $"#T{tempVarCounter++}";
        LexicalTypesEnum type = LexicalTypesEnum.IntType;
        if (node.Token.SubType == LexicalTypesEnum.IntType)
        {
          type = LexicalTypesEnum.IntType;
        }
        else if (node.Token.SubType == LexicalTypesEnum.FloatType)
        {
          type = LexicalTypesEnum.FloatType;
        }

        int idTempVar = symbolsDic.AddSymbol(tempVarName, type);

        Token tempVar = new Token(LexicalTypesEnum.Identifier, tempVarName, idTempVar, type);

        Instruction instruction = new Instruction(node.Token.Type, tempVar, new List<Token> { leftResult, rightResult });
        this.instructions.Add(instruction);
        return tempVar;
      }
      // Если узел является преобразованием типа
      else if (node.Token.Type == LexicalTypesEnum.Coercion && node.Children.Count == 1)
      {
        // Рекурсивно вычисляем выражение для дочернего узла
        Token operandResult = TraverseExpression(node.Children[0]);

        // Генерируем временную переменную для результата преобразования
        string tempVarName = $"#T{tempVarCounter++}";
        int idTempVar = symbolsDic.AddSymbol(tempVarName, LexicalTypesEnum.FloatType);
        
        Token tempVar = new Token(LexicalTypesEnum.Identifier, tempVarName, idTempVar, LexicalTypesEnum.FloatType);

        Instruction instruction = new Instruction(
                    node.Token.Type,
                    tempVar,
                    new List<Token> { operandResult }
                );

        instructions.Add(instruction);

        return tempVar;
      }
      else if (node.Token.Type == LexicalTypesEnum.Identifier ||
              node.Token.Type == LexicalTypesEnum.IntegerConstant ||
              node.Token.Type == LexicalTypesEnum.RealConstant)
      {
        return node.Token;
      }

      return null;
    }

    /// <summary>
    /// Генерация постфиксной записи (GEN2)
    /// </summary>
    /// <returns>Постфиксная запись</returns>
    public List<Token> GeneratePostfixNotation()
    {
      var postfixBuilder = new StringBuilder();
      TraverseTreeForPostfix(syntaxTree.Root);
      return this.postfixTokens;
    }

    /// <summary>
    /// Обход дерева для постфиксной записи
    /// </summary>
    /// <param name="node">Текущий узел дерева</param>
    private void TraverseTreeForPostfix(SyntaxTreeNode node)
    {
      if (node == null)
        return;

      // Если узел является операцией
      if (IsOperator(node.Token))
      {
        // Если узел бинарный (двухоперандный)
        if (node.Children.Count == 2)
        {
          TraverseTreeForPostfix(node.Children[0]);
          TraverseTreeForPostfix(node.Children[1]);
          postfixTokens.Add(node.Token);
        }
        // Если узел унарный (int2float)
        else if (node.Children.Count == 1)
        {
          TraverseTreeForPostfix(node.Children[0]);
          postfixTokens.Add(node.Token);
        }
      }
      else if (node.Token.Type == LexicalTypesEnum.Identifier ||
                     node.Token.Type == LexicalTypesEnum.IntegerConstant ||
                     node.Token.Type == LexicalTypesEnum.RealConstant)
      {
        postfixTokens.Add(node.Token);
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
