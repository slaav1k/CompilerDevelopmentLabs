using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompLabs.Lexical
{
    /// <summary>
    /// Класс для добавления методов расширения к перечислению LexicalTypesEnum
    /// </summary>
    public static class LexicalTypesEnumExtensions
    {
        /// <summary>
        /// Метод расширения для получения детального описания каждого типа токена
        /// </summary>
        /// <param name="type">Тип токена</param>
        /// <returns>Детальное строковое описание</returns>
        public static string ToDetailedString(this LexicalTypesEnum type)
        {
            return type switch
            {
                LexicalTypesEnum.Identifier => "идентификатор с именем",
                LexicalTypesEnum.IntegerConstant => "целочисленная константа",
                LexicalTypesEnum.RealConstant => "вещественная константа",
                LexicalTypesEnum.Addition => "оператор сложения",
                LexicalTypesEnum.Subtraction => "оператор вычитания",
                LexicalTypesEnum.Multiplication => "операция умножения",
                LexicalTypesEnum.Division => "оператор деления",
                LexicalTypesEnum.OpenParenthesis => "открывающая скобка",
                LexicalTypesEnum.CloseParenthesis => "закрывающая скобка",
                LexicalTypesEnum.Equal => "знак равенства",
                LexicalTypesEnum.Unknown => "Неизвестный символ",
                _ => "Неопределенный тип"
            };
        }

        /// <summary>
        /// Метод расширения для получения краткого описания каждого типа токена
        /// </summary>
        /// <param name="type">Тип токена</param>
        /// <returns></returns>
        public static string ToDescribString(this LexicalTypesEnum type)
        {
            return type switch
            {
                LexicalTypesEnum.Identifier => "Id",
                LexicalTypesEnum.IntegerConstant => "Int",
                LexicalTypesEnum.RealConstant => "Real",
                LexicalTypesEnum.Addition => "+",
                LexicalTypesEnum.Subtraction => "-",
                LexicalTypesEnum.Multiplication => "*",
                LexicalTypesEnum.Division => "/",
                LexicalTypesEnum.OpenParenthesis => "(",
                LexicalTypesEnum.CloseParenthesis => ")",
                LexicalTypesEnum.Equal => "=",
                LexicalTypesEnum.Unknown => "Неизвестный символ",
                _ => "Неопределенный тип"
            };
        }
    }
}

