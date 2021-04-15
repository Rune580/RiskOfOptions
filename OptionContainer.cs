using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = System.Object;

namespace RiskOfOptions
{
    /// <summary>
    /// Store options from a specific mod in one container.
    /// 
    /// Probably doesn't need to be an object tbh.
    /// </summary>
    internal class OptionContainer
    {
        public string ModGuid { get; private set; }

        public string ModName { get; internal set; }

        public string Title { get; internal set; }

        public string Name { get; internal set; }
        public string NameToken { get; internal set; }

        public Object[] Description { get; internal set; }

        public string DescriptionToken { get; internal set; }


        internal List<OptionBase> Options;

        private int _lastAmount = 0;

        private List<RiskOfOption> _modOptions;

        private List<OptionCategory> _categories;

        public OptionContainer(string modGuid, string modName)
        {
            this.ModGuid = modGuid;
            this.ModName = modName;


            this.Title = this.ModName;

            Options = new List<OptionBase>();
        }
        public string GetDescriptionAsString()
        {
            string temp = "";

            foreach (var o in Description)
            {
                switch (o)
                {
                    case string _:
                        temp += o.ToString();
                        break;
                    case Sprite sprite:
                        temp += $"<Image:{sprite.name}>";
                        break;
                }
            }

            return temp;
        }

        internal List<RiskOfOption> GetModOptionsCached()
        {
            if (_modOptions != null && Options.Count == _lastAmount)
                return _modOptions;

            _modOptions = Options.Where(a => a.GetType() == typeof(RiskOfOption)).Cast<RiskOfOption>().ToList();

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
