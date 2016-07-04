using MyLibrary;

namespace IdleFantasy {
    public class MissionTaskView : GroupView {
        public void Init( MissionTask i_task ) {
            SetModel( i_task.ViewModel );
        }
    }
}
