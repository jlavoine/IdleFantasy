using MyLibrary;

namespace IdleFantasy {
    public class MissionView : GroupView {

        public void Init( Mission i_mission ) {
            SetModel( i_mission.ViewModel );
        }
    }
}
