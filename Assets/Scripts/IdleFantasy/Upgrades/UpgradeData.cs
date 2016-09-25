using System.Collections.Generic;

namespace IdleFantasy {
    public class UpgradeData  {
        public string PropertyName;

        public int MaxLevel;
        public int BaseXpToLevel;
        public double Coefficient;

        public Dictionary<string, int> ResourcesToUpgrade;
    }
}
