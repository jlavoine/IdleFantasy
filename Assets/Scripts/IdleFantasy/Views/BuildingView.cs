using MyLibrary;

namespace IdleFantasy {
    public class BuildingView : GroupView {

        public void Init( Building i_building ) {
            BuildingView viewToTest = GetComponent<BuildingView>();
            BuildingController controllerToTest = GetComponent<BuildingController>();

            IPlayerData player = PlayerManager.Data;

            viewToTest.SetModel( i_building.GetViewModel() );
            controllerToTest.Init( i_building, (IResourceInventory)player );
        }
    }
}