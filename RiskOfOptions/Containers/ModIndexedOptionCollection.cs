using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RiskOfOptions.Options;

namespace RiskOfOptions.Containers
{
    internal class ModIndexedOptionCollection : IEnumerable<OptionCollection>
    {
        private readonly Dictionary<string, OptionCollection> _optionCollections = new();
        private readonly Dictionary<string, string> _identifierModGuidMap = new();

        internal void AddOption(ref BaseOption option)
        {
            if (!_optionCollections.ContainsKey(option.ModGuid))
                _optionCollections[option.ModGuid] = new OptionCollection(option.ModName, option.ModGuid);
            
            _optionCollections[option.ModGuid].AddOption(ref option);
            _identifierModGuidMap[option.Identifier] = option.ModGuid;
        }

        internal BaseOption GetOption(string identifier)
        {
            string modGuid = _identifierModGuidMap[identifier];
            return _optionCollections[modGuid].GetOption(identifier);
        }

        internal bool ContainsModGuid(string modGuid)
        {
            return _optionCollections.ContainsKey(modGuid);
        }

        internal OptionCollection this[string modGuid]
        {
            get => _optionCollections[modGuid];
            set => _optionCollections[modGuid] = value;
        }

        internal int Count => _optionCollections.Count;
        
        public IEnumerator<OptionCollection> GetEnumerator()
        {
            return new OptionCollectionEnumerator(_optionCollections.Values.ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class OptionCollectionEnumerator : IEnumerator<OptionCollection>
        {
            private readonly OptionCollection[] _optionCollections;
            private int _position = -1;
            internal OptionCollectionEnumerator(OptionCollection[] optionCollections)
            {
                _optionCollections = optionCollections;
            }
            
            public bool MoveNext()
            {
                _position++;
                return _position < _optionCollections.Length;
            }

            public void Reset()
            {
                _position = -1;
            }

            public OptionCollection Current
            {
                get
                {
                    try
                    {
                        return _optionCollections[_position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                
            }
        }
    }
}