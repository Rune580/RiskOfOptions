using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RiskOfOptions.Options;

namespace RiskOfOptions.Containers
{
    internal class OptionStorage
    {
        private int _lastAmount = 0;

        internal List<OptionBase> Options;

        private List<RiskOfOption> _modOptions;

        private List<OptionCategory> _categories;

        internal OptionStorage()
        {
            Options = new List<OptionBase>();
        }

        internal List<RiskOfOption> GetModOptionsCached()
        {
            if (_modOptions != null && Options.Count == _lastAmount)
                return _modOptions;

            _modOptions = Options.Where(a => a.GetType() == typeof(RiskOfOption) || a.GetType().IsSubclassOf(typeof(RiskOfOption))).Cast<RiskOfOption>().ToList();

            _lastAmount = Options.Count;

            return _modOptions;
        }

        internal List<OptionCategory> GetCategoriesCached()
        {
            if (_categories != null && Options.Count == _lastAmount)
                return _categories;

            _categories = Options.Where(a => a.GetType() == typeof(OptionCategory)).Cast<OptionCategory>().ToList();

            _lastAmount = Options.Count;

            return _categories;
        }

        internal void Add(ref OptionBase option)
        {
            Options.Add(option);
        }

        internal void Add(ref RiskOfOption option)
        {
            Options.Add(option);
        }
        internal void Add(ref OptionCategory option)
        {
            Options.Add(option);
        }

        internal void Insert(ref OptionCategory option)
        {
            Options.Insert(0, option);
        }
    }
}
