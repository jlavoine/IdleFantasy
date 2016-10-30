using System.Collections.Generic;

namespace MyLibrary.Achievements {
    public class AchievementManager : IAchievementManager {
        private List<IAchievement> mAllAchievements;

        private List<IAchievement> mUnearnedAchievements;
        public List<IAchievement> UnearnedAchievements { get { return mUnearnedAchievements; } private set { mUnearnedAchievements = value; } }

        private IInfoPopupManager mPopupManager;

        private string mAchievementPopupPrefabName;

        public AchievementManager( List<IAchievement> i_previouslyEarnedAchievements, List<IAchievement> i_allAchievements, IInfoPopupManager i_popupManager, string i_achievementPopupPrefabName ) {
            mAchievementPopupPrefabName = i_achievementPopupPrefabName;
            mAllAchievements = i_allAchievements;
            mPopupManager = i_popupManager;

            CreateListOfUnearnedAchievements( i_previouslyEarnedAchievements );            
        }

        private void CreateListOfUnearnedAchievements( List<IAchievement> i_previouslyEarnedAchievements ) {
            UnearnedAchievements = new List<IAchievement>();

            foreach ( IAchievement achievement in mAllAchievements ) {
                if ( !i_previouslyEarnedAchievements.Contains( achievement ) ) {
                    UnearnedAchievements.Add( achievement );
                }
            }
        }

        public void CheckForNewAchievements() {
            foreach ( IAchievement achievement in UnearnedAchievements ) {
                if ( achievement.IsEarned() ) {
                    QueueAchievementPopup( achievement );
                }
            }

            UpdateListOfUnearnedAchievements();
        }

        private void QueueAchievementPopup( IAchievement i_achievement ) {
            EarnedAchievementPM earnedPM = new EarnedAchievementPM( i_achievement );
            mPopupManager.QueueInfoPopup( mAchievementPopupPrefabName, earnedPM.ViewModel );
        }

        private void UpdateListOfUnearnedAchievements() {
            UnearnedAchievements = new List<IAchievement>();

            foreach ( IAchievement achievement in mAllAchievements ) {
                if ( !achievement.IsEarned() ) {
                    UnearnedAchievements.Add( achievement );
                }
            }
        }
    }
}
