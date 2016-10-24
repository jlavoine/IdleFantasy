using System.Collections.Generic;

namespace MyLibrary {
    public class Achievement : IAchievement {
        public string Key;
        public List<IAchievementRequirement> Requirements;

        public Achievement( string i_key, List<IAchievementRequirement> i_requirements ) {
            Key = i_key;
            Requirements = i_requirements;
        }

        public bool IsEarned() {
            foreach ( IAchievementRequirement requirement in Requirements ) {
                if ( !requirement.DoesPass() ) {
                    return false;
                }
            }

            return true;
        }
    }
}