
namespace MyLibrary {
    public class EarnedAchievementPM : PresentationModel {

        public EarnedAchievementPM( IAchievement i_achievement ) {
            SetNameProperty( i_achievement );
        }

        private void SetNameProperty( IAchievement i_achievement ) {
            ViewModel.SetProperty( InfoPopupProperties.MAIN_TEXT, i_achievement.GetName() );
        }
    }
}