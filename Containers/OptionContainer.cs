using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;
using RiskOfOptions.Options;
using UnityEngine;
using Object = System.Object;

namespace RiskOfOptions.Containers
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

        private readonly OptionStorage _options;

        public OptionContainer(string modGuid, string modName)
        {
            this.ModGuid = modGuid;
            this.ModName = modName;


            this.Title = this.ModName;

            _options = new OptionStorage();
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
            return _options.GetModOptionsCached();
        }

        internal List<OptionCategory> GetCategoriesCached()
        {
            return _options.GetCategoriesCached();
        }

        internal void Add(ref OptionBase option)
        {
            _options.Add(ref option);
        }

        internal void Add(ref RiskOfOption option)
        {
            _options.Add(ref option);
        }
        internal void Add(ref OptionCategory option)
        {
            _options.Add(ref option);
            if (_options.Options.Count > 80)
            {
                Debug.Log($"Hey! Mod: {ModName}, has added over 80 categories to its menu! The mod options menu may not look as intended!", LogLevel.Warning);
            }
        }

        internal void Insert(ref OptionCategory option)
        {
            _options.Insert(ref option);
            if (_options.Options.Count > 80)
            {
                Debug.Log($"Hey! Mod: {ModName}, has added over 80 categories to its menu! The mod options menu may not look as intended!", LogLevel.Warning);
            }
        }
    }
}
