
namespace MyLibrary {
    public class QueuedInfoPopupData {

        public string PrefabName;
        public ViewModel ViewModel;

        public QueuedInfoPopupData( string i_prefabName, ViewModel i_viewModel ) {
            PrefabName = i_prefabName;
            ViewModel = i_viewModel;
        }
    }
}
