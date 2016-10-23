using MyLibrary;

namespace IdleFantasy {
    public class BuildingView : GroupView {

        public void Init( Building i_building, int i_index ) {        
            SetModel( i_building.GetViewModel() );

            InitBuildingController( i_building );
            InitTrainingController( i_building.Unit );
            InitUnitInfoButton( i_index );        
        }

        private void InitBuildingController( Building i_building ) {
            BuildingController controller = GetComponent<BuildingController>();
            IPlayerData player = PlayerManager.Data;

            controller.Init( i_building, (IResourceInventory) player );
        }

        private void InitTrainingController( IUnit i_unit ) {
            TrainerAssignmentController controller = GetComponentInChildren<TrainerAssignmentController>();

            controller.Init( i_unit );
        }

        private void InitUnitInfoButton( int i_index ) {
            OpenUnitInfo openButton = GetComponentInChildren<OpenUnitInfo>();
            openButton.SetUnitIndex( i_index );
        }
    }
}