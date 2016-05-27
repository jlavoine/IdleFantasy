using MyLibrary;

namespace IdleFantasy {
    public class BuildingView : GroupView {

        public void Init( Building i_building ) {
            BuildingController controller = GetComponent<BuildingController>();
            IPlayerData player = PlayerManager.Data;

            SetModel( i_building.GetViewModel() );
            controller.Init( i_building, (IResourceInventory)player );
        }
    }
}