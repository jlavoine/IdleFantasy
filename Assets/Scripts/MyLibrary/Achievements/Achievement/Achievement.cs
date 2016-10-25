using System.Collections.Generic;

namespace MyLibrary {
    public class Achievement : IAchievement {
        public string Key;
        public List<IAchievementRequirement> Requirements;

        private const string NAME_KEY = "ACHIEVEMENT_NAME_";

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

        public string GetName() {
            return StringTableManager.Get( NAME_KEY + Key );
        }
    }
}