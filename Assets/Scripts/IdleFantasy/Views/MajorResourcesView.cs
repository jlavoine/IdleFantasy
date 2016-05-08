using MyLibrary;

namespace IdleFantasy {
    public class MajorResourcesView : GroupView {
       
        void Start() {
            PlayerData player = PlayerManager.Data;
            SetModel( player.GetViewModel() );
        }
    }
}