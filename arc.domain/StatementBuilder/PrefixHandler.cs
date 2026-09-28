using System.Collections.Generic;

namespace arc.domain.StatementBuilder
{
    internal class PrefixHandler
    {
        private char _firstLetter = 'a';
        private char _secondLetter = 'a';
        private readonly Dictionary<string,string> _tablePrefixes = [];
        //private readonly Dictionary<string, string> _testPrefixes = [];

        internal string GetPrefix(string tableName)
        {
            tableName = tableName.ToLower();
            string returnPrefix;
            if (! _tablePrefixes.TryGetValue(tableName, out _))
            {
                returnPrefix = GetNewPrefix();
                if (tableName != "listitem" && tableName != "testres" && tableName != "culturetestres")
                {
                    _tablePrefixes.Add(tableName, returnPrefix);
                }
            } else
            {
                returnPrefix = _tablePrefixes[tableName];
            }
            return returnPrefix;
        }

        internal string GetNewPrefix(string currentPrefix)
        {
            var charArray = currentPrefix.ToCharArray();
            _firstLetter = charArray[0];
            if (charArray.Length > 1)
            {
                _secondLetter = charArray[1];
            }
            return GetNewPrefix();
        }

        internal string GetNewPrefix()
        {
            if (_secondLetter == 'z')
            {
                _secondLetter = 'a';
                _firstLetter++;
            }
            else
            {
                _secondLetter++;
            }

            switch(_firstLetter.ToString() + _secondLetter.ToString())
            {
                case "as":
                    _secondLetter++;
                    _secondLetter++;
                    break;
                case "at":
                case "by":
                case "db":
                case "do":
                case "fs":
                case "go":
                    _secondLetter++;
                    break;
                case "ast":
                    _secondLetter++;
                    break;
                default:
                    break;
            }

            return _firstLetter.ToString() + _secondLetter.ToString();
        }

        internal void SetInitialPrefix(string table, string prefix)
        {
            _tablePrefixes.Add(table, prefix);
        }
    }


}
