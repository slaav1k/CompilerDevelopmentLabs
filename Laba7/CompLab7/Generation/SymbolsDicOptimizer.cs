using ClassLiberty.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CompLabs.Generation
{
  internal class SymbolsDicOptimizer
  {
    public SymbolsDicOptimizer()
    {
    }

    public SymbolsDic Optimize(SymbolsDic symbolsDic, List<Instruction> instructions)
    {
      foreach (var symbol in symbolsDic.GetSymbols())
      {
        if (!symbol.Value.Name.StartsWith("#T"))
        {
          bool used = false;
          foreach (var instruction in instructions)
          {
            if (instruction.ResToken.Value == symbol.Value.Name)
            {
              used = true;
            }

            foreach (var operand in instruction.Tokens)
            {
              if (operand.Type == ClassLiberty.Enums.LexicalTypesEnum.Identifier)
              {
                if (operand.Value == symbol.Value.Name)
                {
                  used = true;
                }
              }
              
            }
          }
          if (!used)
          {
            symbolsDic.RemoveSymbolByName(symbol.Value.Name);
          }
        }
      
        
      }
      return symbolsDic;
    }
  }
}
