using System;
using System.Collections.Generic;
using System.Text;
using CompLabs.Entities;
using CompLabs.Lexical;

namespace CompLabs.Generation
{
  /// <summary>
  /// Класс для генерации промежуточного кода и постфиксной нотации
  /// </summary>
  internal class GenereticHandler3
  {
    private readonly SyntaxTree syntaxTree;
    private SymbolsDic symbolsDic;
    private int tempVarCounter;
    private List<Instruction> instructions;
    private List<Token> postfixTokens;
    private Stack<Token> reusableTemps = new(); // Стек доступных временных переменных

    public GenereticHandler3(SyntaxTree syntaxTree, SymbolsDic symbolsDic)
    {
      this.syntaxTree = syntaxTree;
      this.symbolsDic = symbolsDic;
      this.instructions = new List<Instruction>();
      this.postfixTokens = new List<Token>();
      tempVarCounter = 1;  // Начинаем с временной переменной T1
    }

    /// <summary>
    /// Генерация трехадресного кода с поддержкой оптимизации
    /// </summary>
    /// <param name="isOpt">Флаг для включения/выключения оптимизации</param>
    /// <returns>Трехадресный код</returns>
    public List<Instruction> GenerateThreeAddressCode(bool isOpt)
    {
      TraverseExpression(syntaxTree.Root, isOpt);
      return this.instructions;
    }

    /// <summary>
    /// Обход выражений и возвращение результата для трехадресного кода
    /// </summary>
    /// <param name="node">Узел выражения</param>
    /// <param name="isOpt">Флаг для включения/выключения оптимизации</param>
    /// <returns>Результат вычислений</returns>
    private Token TraverseExpression(SyntaxTreeNode node, bool isOpt)
    {
      if (node == null)
        return null;

      // Если узел является операцией
      if (IsOperator(node.Token) && node.Children.Count == 2)
      {
        var leftResult = TraverseExpression(node.Children[0], isOpt);
        var rightResult = TraverseExpression(node.Children[1], isOpt);

        // Генерируем временную переменную для текущего узла-операции
        Token tempVar = GetTempVariable(node.Token.SubType);

        // Создаем инструкцию для операции
        Instruction instruction = new Instruction(node.Token.Type, tempVar, new List<Token> { leftResult, rightResult });
        this.instructions.Add(instruction);

        // Освобождаем временные переменные, если требуется
        if (isOpt)
        {
          ReleaseTemporaryVariable(leftResult);
          ReleaseTemporaryVariable(rightResult);
        }

        return tempVar;
      }
      // Если узел является преобразованием типа
      //else if (node.Token.Type == LexicalTypesEnum.Coercion && node.Children.Count == 1)
      //{
      //  // Рекурсивно вычисляем выражение для дочернего узла
      //  Token operandResult = TraverseExpression(node.Children[0], isOpt);

      //  // Генерируем временную переменную для результата преобразования
      //  Token tempVar = GetTempVariable(LexicalTypesEnum.FloatType);

      //  Instruction instruction = new Instruction(
      //      node.Token.Type,
      //      tempVar,
      //      new List<Token> { operandResult }
      //  );

      //  instructions.Add(instruction);

      //  return tempVar;
      //}
      else if (node.Token.Type == LexicalTypesEnum.Coercion && node.Children.Count == 1)
      {
        // Рекурсивно вычисляем выражение для дочернего узла
        Token operandResult = TraverseExpression(node.Children[0], isOpt);

        // Если преобразование уже выполнено в текущем идентификаторе, возвращаем его
        if (operandResult.SubType == node.Token.SubType)
        {
          return operandResult;
        }

        // Проверяем, есть ли свободная временная переменная
        Token tempVar = GetTempVariable(node.Token.SubType);

        // Создаем инструкцию для преобразования
        Instruction instruction = new Instruction(
            node.Token.Type,
            tempVar,
            new List<Token> { operandResult }
        );

        // Освобождаем использованную временную переменную
        if (isOpt)
        {
          ReleaseTemporaryVariable(operandResult);
        }

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
    /// <param name="isOpt">Флаг для включения/выключения оптимизации</param>
    /// <returns>Постфиксная запись</returns>
    public List<Token> GeneratePostfixNotation()
    {
      TraverseTreeForPostfix(syntaxTree.Root);
      return this.postfixTokens;
    }

    /// <summary>
    /// Обход дерева для постфиксной записи
    /// </summary>
    /// <param name="node">Текущий узел дерева</param>
    /// <param name="isOpt">Флаг для включения/выключения оптимизации</param>
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

    /// <summary>
    /// Получение временной переменной (с возможностью оптимизации)
    /// </summary>
    /// <param name="type">Тип переменной</param>
    /// <returns>Токен временной переменной</returns>
    private Token GetTempVariable(LexicalTypesEnum? type)
    {
      if (reusableTemps.Count > 0)
      {
        var tempVar = reusableTemps.Pop();

        // Убедимся, что переменная действительно временная
        if (tempVar.Value is string name && name.StartsWith("#T"))
        {
          tempVar.SubType = type; // Обновляем тип переменной
          return tempVar;
        }
      }

      // Если в стеке нет временных переменных, создаем новую
      string tempVarName = $"#T{tempVarCounter++}";
      LexicalTypesEnum resolvedType = type ?? LexicalTypesEnum.IntType;
      int idTempVar = symbolsDic.AddSymbol(tempVarName, resolvedType);
      var newTempVar = new Token(LexicalTypesEnum.Identifier, tempVarName, idTempVar, type);
      return newTempVar;
    }

    //private Token GetTempVariable(LexicalTypesEnum? type)
    //{
    //  if (reusableTemps.Count > 0)
    //  {
    //    var tempVar = reusableTemps.Pop();
    //    tempVar.SubType = type; // Обновляем тип переменной
    //    return tempVar;
    //  }

    //  // Создаем новую временную переменную
    //  string tempVarName = $"#T{tempVarCounter++}";
    //  LexicalTypesEnum resolvedType = type ?? LexicalTypesEnum.IntType;
    //  int idTempVar = symbolsDic.AddSymbol(tempVarName, resolvedType);
    //  var newTempVar = new Token(LexicalTypesEnum.Identifier, tempVarName, idTempVar, type);
    //  return newTempVar;
    //}



    /// <summary>
    /// Освобождение временной переменной
    /// </summary>
    /// <param name="tempVar">Временная переменная</param>
    private void ReleaseTemporaryVariable(Token tempVar)
    {
      if (tempVar != null &&
          tempVar.Type == LexicalTypesEnum.Identifier &&
          tempVar.Value is string name &&
          name.StartsWith("#T")) // Проверяем, что это временная переменная
      {
        reusableTemps.Push(tempVar);
      }
    }


  }
}
