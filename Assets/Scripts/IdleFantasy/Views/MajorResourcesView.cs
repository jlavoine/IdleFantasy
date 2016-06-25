using MyLibrary;

namespace IdleFantasy {
    public class MajorResourcesView : GroupView {
       
        void Start() {
            PlayerData player = (PlayerData)PlayerManager.Data;
            SetModel( player.GetViewModel() );
        }
    }
}