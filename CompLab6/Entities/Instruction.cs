using CompLabs.Generation;
using CompLabs.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CompLabs.Entities
{
  internal class Instruction
  {
    public GenerationInstructionsEnum Manual {  get; set; }

    public Token ResToken { get; set; }

    public List<Token> Tokens { get; set; }

    public Instruction(LexicalTypesEnum manual, Token res, List<Token> tokens) {
      Manual = GenerationInstructionsFromLexicalTypes.ToGetInstruction(manual);
      ResToken = res;
      Tokens = tokens;
    }

    public override string ToString()
    {
      String res = ResToken.ToString().Split(" - ")[0];
      String tok0 = Tokens[0].ToString().Split(" - ")[0];
      if (Tokens.Count == 1)
      {
        return $"{Manual} {res} {tok0}";
      }
      else
      {
        String tok1 = Tokens[1].ToString().Split(" - ")[0];
        return $"{Manual} {res} {tok0} {tok1}";
      }
    }
  }
}
