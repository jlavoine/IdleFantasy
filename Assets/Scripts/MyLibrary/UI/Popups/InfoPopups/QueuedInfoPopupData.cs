
namespace MyLibrary {
    public class QueuedInfoPopupData {

        public string PrefabName;
        public IViewModel ViewModel;

        public QueuedInfoPopupData( string i_prefabName, IViewModel i_viewModel ) {
            PrefabName = i_prefabName;
            ViewModel = i_viewModel;
        }
    }
}
