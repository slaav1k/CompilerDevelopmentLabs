
using ClassLiberty.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassLiberty.Entities
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

    // Метод сериализации
    public void Serialize(BinaryWriter writer)
    {
      writer.Write((int)Manual);
      ResToken.Serialize(writer);
      writer.Write(Tokens.Count);
      foreach (var token in Tokens)
      {
        token.Serialize(writer);
      }
    }

    // Метод десериализации
    public static Instruction Deserialize(BinaryReader reader)
    {
      GenerationInstructionsEnum manual = (GenerationInstructionsEnum)reader.ReadInt32();
      Token resToken = Token.Deserialize(reader);

      int tokenCount = reader.ReadInt32();
      List<Token> tokens = new List<Token>();
      for (int i = 0; i < tokenCount; i++)
      {
        tokens.Add(Token.Deserialize(reader));
      }

      return new Instruction(GenerationInstructionsFromLexicalTypes.ToSetInstruction(manual), resToken, tokens);
    }

    //public override string ToString()
    //{
    //  String res = ResToken.ToString().Split(" - ")[0];
    //  String tok0 = Tokens[0].ToString().Split(" - ")[0];
    //  if (Tokens.Count == 1)
    //  {
    //    return $"{Manual} {res} {tok0}";
    //  }
    //  else
    //  {
    //    String tok1 = Tokens[1].ToString().Split(" - ")[0];
    //    return $"{Manual} {res} {tok0} {tok1}";
    //  }
    //}

    public override string ToString()
    {
      String res = ResToken.ToString().Split(new[] { " - " }, StringSplitOptions.None)[0];
      String tok0 = Tokens[0].ToString().Split(new[] { " - " }, StringSplitOptions.None)[0];
      if (Tokens.Count == 1)
      {
        return $"{Manual} {res} {tok0}";
      }
      else
      {
        String tok1 = Tokens[1].ToString().Split(new[] { " - " }, StringSplitOptions.None)[0];
        return $"{Manual} {res} {tok0} {tok1}";
      }
    }

  }
}
