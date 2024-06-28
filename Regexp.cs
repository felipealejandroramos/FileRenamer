using System.Text.RegularExpressions;

namespace FindAndRename
{
    internal partial class Regexp
    {
        [GeneratedRegex("_[1-9][0-9]{3}-[0-1][0-9]-[0-3][0-9]")]
        private static partial Regex HasBeenDated();
        /// <summary>
        /// controlla se una stringa ha una data nel formato _yyyy-MM-dd
        /// </summary>
        /// <param name="text"></param>
        /// <returns> booleano vero se la data c'e nel formato giusto falso se non c'e </returns>
        public static bool CheckIfDated(string text)
        {
            if (HasBeenDated().IsMatch(text))
            {
                return true;
            }
            return false;
        }

    }
}
