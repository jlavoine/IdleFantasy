
namespace IdleFantasy {
    public static class BuildingUtilsManager {
        private static IBuildingUtils mUtils;
        public static IBuildingUtils Utils {
            get {
                if ( mUtils == null ) {
                    mUtils = new BuildingUtils();
                }

                return mUtils;
            }
            set {
                mUtils = value;
            }
        }
    }
}
