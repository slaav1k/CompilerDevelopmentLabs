using CompLabs.CompLabs;
using ClassLiberty.Enums;
using ClassLiberty.Entities;
using CompLabs.Exceptions;
using CompLabs.Generation;
using CompLabs.Semantical;
using CompLabs.Syntaxical;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            WorkerWithFiles.WriteToFile("syntax_tree_mod_opt.txt", optimizeTree);


            GenereticHandler3 genereticHandler3 = new GenereticHandler3(optimizeTree, symbolsDic);
            List<Instruction> lisOptimizetInstructions2 = genereticHandler3.GenerateThreeAddressCode(true);

            WorkerWithFiles.WriteThreeAddressCodeToFile(lisOptimizetInstructions2, "OPT" + generationFileName);
            WorkerWithFiles.WriteSymbolsFile("OPT" + symbolsFileName, symbolsDic);


            // Запись в один файл: сначала таблица символов, затем список инструкций
            string combinedFilePath = "post_code.bin";

            using (FileStream fs = new FileStream(combinedFilePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
              // Сериализация таблицы символов (SymbolsDic)
              symbolsDic.Serialize(writer);
              Console.WriteLine("SymbolsDic serialized to post_code.bin");

              // Сериализация списка инструкций (List<Instruction>)
              writer.Write(lisOptimizetInstructions2.Count); // Записываем количество инструкций
              foreach (var instruction in lisOptimizetInstructions2)
              {
                instruction.Serialize(writer); // Сериализуем каждую инструкцию
              }
              Console.WriteLine("List of Instructions serialized to post_code.bin");
            }
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
        else if (mode == AnalysisMode.GEN3)
        {
          generationFileName = "portable_code.txt";
          symbolsFileName = "symbols.txt";

          semanticFileName = "syntax_tree_mod.txt";
          SyntaxHandler syntaxHandler = new SyntaxHandler(tokens);
          SyntaxTree syntaxTree = syntaxHandler.Analyze();

          SymbolsDic symbolsDic = LexicalHandler.GetSymbolDictionary();

          SemanticHandler semanticHandler = new SemanticHandler(syntaxTree, symbolsDic);
          WorkerWithFiles.WriteToFile(semanticFileName, syntaxTree);

          // оптимизатор дерева
          SyntaxTree optimizeTree = SyntaxTreeOptimizer2.OptimizeTree(syntaxTree);
          //Console.WriteLine(optimizeTree);
          WorkerWithFiles.WriteToFile("syntax_tree_mod_opt.txt", optimizeTree);


          GenereticHandler3 genereticHandler3 = new GenereticHandler3(optimizeTree, symbolsDic);
          List<Instruction> lisOptimizetInstructions2 = genereticHandler3.GenerateThreeAddressCode(true);

          SymbolsDicOptimizer symbolsDicOptimizer = new SymbolsDicOptimizer();
          symbolsDic = symbolsDicOptimizer.Optimize(symbolsDic, lisOptimizetInstructions2);

          WorkerWithFiles.WriteThreeAddressCodeToFile(lisOptimizetInstructions2, "OPT" + generationFileName);
          WorkerWithFiles.WriteSymbolsFile("OPT" + symbolsFileName, symbolsDic);

          string combinedFilePath = "post_code.bin";
          WorkerWithFiles.CreateBinFile(symbolsDic, lisOptimizetInstructions2, combinedFilePath);
          
        }
        else
        {
          throw new InvalidArgumentsException("Ошибка: некорректный режим работы. Используйте LEX | SYN | SEM | GEN1 (OPT) | GEN2 (OPT) | GEN3.");
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
