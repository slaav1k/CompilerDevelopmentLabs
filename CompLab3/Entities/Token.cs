using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompLabs.Lexical;

namespace CompLabs.Entities
{
    /// <summary>
    /// Класс для описания токена
    /// </summary>
    internal class Token
    {
        /// <summary>
        /// Вид токена
        /// </summary>
        public LexicalTypesEnum Type { get; }

        /// <summary>
        /// Значение токена для переменных
        /// </summary>
        //public string Value { get; }
        public dynamic Value { get; }


        //public SymbolsDic Symbols { get; set; }
        /// <summary>
        /// Порядок токена для переменных
        /// </summary>
        public int IdentifierID { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type">Вид токена</param>
        /// <param name="value">Порядок токена для перемнных</param>
        public Token(LexicalTypesEnum type, dynamic value, int identifierID = -1/*SymbolsDic symbolsDic = null*/)
        {
            Type = type;
            Value = value;
            //Symbols = symbolsDic;
            IdentifierID = identifierID;
        }



        /// <summary>
        /// Переопредленный метод toString()
        /// </summary>
        /// <returns>Форматированная строка</returns>
        public override string ToString()
        {
            return Type == LexicalTypesEnum.Identifier
          ? $"<{Type.ToDescribString()},{IdentifierID}> - {Type.ToDetailedString()} {Value}"
          : $"<{Value}> - {Type.ToDetailedString()}";
        }
    }
}
