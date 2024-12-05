﻿using CompLabs.CompLabs;
using CompLabs.Entities;
using CompLabs.Exceptions;
using CompLabs.Generation;
using System.Diagnostics.Metrics;
using System.Text;

namespace CompLabs
{
  /// <summary>
  /// Основной класс
  /// </summary>
  internal class Runner
  {
    private static void Main(string[] args)
    {
      try
      {
        // Проверка наличия аргументов
        if (args.Length < 2)
          throw new InvalidArgumentsException("Ошибка: недостаточно аргументов.");

        AnalysisMode mode;
        if (!Enum.TryParse(args[0].ToUpper(), out mode))
        {
          throw new InvalidArgumentsException("Ошибка: некорректный режим работы. Используйте LEX или SYN.");
        }

        string inputFileName = args.Length == 3 && args[1].ToUpper() == "OPT" ? args[2] : args[1];
        string tokensFileName;
        string symbolsFileName;
        string syntaxFileName;
        string semanticFileName;
        string generationFileName;

        if (!WorkerWithFiles.IsFileExit(inputFileName))
          throw new FileException($"Ошибка: {inputFileName} не существует");

        if (!(WorkerWithFiles.IsValidFileName(inputFileName)))
          throw new FileException("Ошибка: некоректное название одного из файла");

        // Анализ содержимого
        List<Token> tokens = LexicalHandler.Analyze(WorkerWithFiles.ReadInputFile(inputFileName));

        if (mode == AnalysisMode.SYN)
        {
          syntaxFileName = "syntax_tree.txt";
          SyntaxHandler syntaxHandler = new SyntaxHandler(tokens);
          SyntaxTree syntaxTree = syntaxHandler.Analyze();
          WorkerWithFiles.WriteToFile(syntaxFileName, syntaxTree);
        }
        else if (mode == AnalysisMode.LEX)
        {
          tokensFileName = "tokens.txt";
          symbolsFileName = "symbols.txt";
          WorkerWithFiles.WriteTokensFile(tokensFileName, tokens);
          WorkerWithFiles.WriteSymbolsFile(symbolsFileName, LexicalHandler.GetSymbolDictionary());
        }
        else if (mode == AnalysisMode.SEM)
        {
          semanticFileName = "syntax_tree_mod.txt";
          SyntaxHandler syntaxHandler = new SyntaxHandler(tokens);
          SyntaxTree syntaxTree = syntaxHandler.Analyze();

          SemanticHandler semanticHandler = new SemanticHandler(syntaxTree, LexicalHandler.GetSymbolDictionary());
          WorkerWithFiles.WriteToFile(semanticFileName, syntaxTree);
        }
        else if (mode == AnalysisMode.GEN1)
        {
          generationFileName = "portable_code.txt";
          symbolsFileName = "symbols.txt";

          semanticFileName = "syntax_tree_mod.txt";
          SyntaxHandler syntaxHandler = new SyntaxHandler(tokens);
          SyntaxTree syntaxTree = syntaxHandler.Analyze();

          SymbolsDic symbolsDic = LexicalHandler.GetSymbolDictionary();

          SemanticHandler semanticHandler = new SemanticHandler(syntaxTree, symbolsDic);
          WorkerWithFiles.WriteToFile(semanticFileName, syntaxTree);

          if (args.Length == 3 && args[1].ToUpper() == "OPT")
          {
            // оптимизатор дерева
            SyntaxTree optimizeTree = SyntaxTreeOptimizer2.OptimizeTree(syntaxTree);
            //Console.WriteLine(optimizeTree);
            //WorkerWithFiles.WriteToFile("syntax_tree_mod_opt.txt", optimizeTree);
            //GenereticHandler2 genereticOptimizeHandler = new GenereticHandler2(optimizeTree, symbolsDic, true);

            //List<Instruction> lisOptimizetInstructions = genereticOptimizeHandler.GenerateThreeAddressCode();
            //WorkerWithFiles.WriteThreeAddressCodeToFile(lisOptimizetInstructions, generationFileName);
            //WorkerWithFiles.WriteSymbolsFile(symbolsFileName, symbolsDic);


            //ThreeAddressCodeOptimizer threeAddressCodeOptimizer = new ThreeAddressCodeOptimizer(symbolsDic);
            //List<Instruction> lisOptimizetInstructions2 = threeAddressCodeOptimizer.Optimize(lisOptimizetInstructions);

            GenereticHandler3 genereticHandler3 = new GenereticHandler3(optimizeTree, symbolsDic);
            List<Instruction> lisOptimizetInstructions2 = genereticHandler3.GenerateThreeAddressCode(true);

            WorkerWithFiles.WriteThreeAddressCodeToFile(lisOptimizetInstructions2, "OPT" + generationFileName);
            WorkerWithFiles.WriteSymbolsFile("OPT" + symbolsFileName, symbolsDic);
          }
          else
          {
            GenereticHandler3 genereticHandler = new GenereticHandler3(syntaxTree, symbolsDic);
            List<Instruction> listInstructions = genereticHandler.GenerateThreeAddressCode(false);

            WorkerWithFiles.WriteThreeAddressCodeToFile(listInstructions, generationFileName);
            WorkerWithFiles.WriteSymbolsFile(symbolsFileName, symbolsDic);
          }

          
        }
        else if (mode == AnalysisMode.GEN2)
        {
          generationFileName = "postfix.txt";
          symbolsFileName = "symbols.txt";

          semanticFileName = "syntax_tree_mod.txt";
          SyntaxHandler syntaxHandler = new SyntaxHandler(tokens);
          SyntaxTree syntaxTree = syntaxHandler.Analyze();

          SymbolsDic symbolsDic = LexicalHandler.GetSymbolDictionary();

          SemanticHandler semanticHandler = new SemanticHandler(syntaxTree, symbolsDic);
          WorkerWithFiles.WriteToFile(semanticFileName, syntaxTree);

          if (args.Length == 3 && args[1].ToUpper() == "OPT")
          {
            syntaxTree = SyntaxTreeOptimizer2.OptimizeTree(syntaxTree);
            generationFileName = "OPT" + generationFileName;
          }

            GenereticHandler3 genereticHandler = new GenereticHandler3(syntaxTree, symbolsDic);

          List<Token> posfixForm = genereticHandler.GeneratePostfixNotation();

          WorkerWithFiles.WritePostfixFormToFile(posfixForm, generationFileName);
          //Console.WriteLine(symbolsDic);
          WorkerWithFiles.WriteSymbolsFile(symbolsFileName, symbolsDic);
        }
        else
        {
          throw new InvalidArgumentsException("Ошибка: некорректный режим работы. Используйте LEX | SYN | SEM | GEN1 (OPT) | GEN2 (OPT).");
        }

        //if (!WorkerWithFiles.IsValidFile(inputFileName))
        //  throw new FileException("Ошибка: некоректное содержимое файла");

        Console.WriteLine("Анализ завершён успешно.");

      }
      catch (InvalidArgumentsException ex)
      {
        Console.WriteLine(ex.Message);
      }
      catch (FileException ex)
      {
        Console.WriteLine(ex.Message);
      }
      catch (LexicalException ex)
      {
        Console.WriteLine(ex.Message);
      }
      catch (SyntaxException ex)
      {
        Console.WriteLine(ex.Message);
      }
      catch (SemanticException ex)
      {
        Console.WriteLine(ex.Message);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
      }
    }
  }
}