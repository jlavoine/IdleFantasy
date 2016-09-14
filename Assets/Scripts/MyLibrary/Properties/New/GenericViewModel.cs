
namespace MyLibrary {
    public class GenericViewModel {
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        public GenericViewModel() {
            mModel = new ViewModel();
        }
    }
}
