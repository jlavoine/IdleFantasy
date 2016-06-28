using MyLibrary;

namespace IdleFantasy {
    public abstract class UnitModifier<T> where T : BaseUnitModifierData {

        private ViewModel mModel;
        public ViewModel GetViewModel() {
            return mModel;
        }

        private Upgradeable mLevel;
        public Upgradeable Level {
            get { return mLevel; }
        }

        protected BaseUnitModifierData mData;
        public BaseUnitModifierData Data {
            get { return mData; }
        }

        public UnitModifier( ProgressBase i_progress ) {
            mModel = new ViewModel();
            mData = GenericDataLoader.GetData<T>( i_progress.ID );

            mLevel = new Upgradeable();
            mLevel.SetPropertyToUpgrade( mModel, mData.Level );
            Level.Value = i_progress.Level;
        }

        public float GetUnitStatBonus( IUnit i_unit, string i_stat ) {
            float bonus = 0f;
            foreach ( UnitModificationData mod in Data.UnitModifications ) {
                bonus += mod.GetBonus( i_unit, i_stat, Level.Value );
            }

            return bonus;
        }
    }
}
