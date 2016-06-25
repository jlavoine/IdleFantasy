using MyLibrary;

namespace IdleFantasy {
    public class Guild {
        private ViewModel mModel;
        public ViewModel GetViewModel() {
            return mModel;
        }

        private GuildData mData;
        public GuildData Data {
            get { return mData; }
        }

        private Upgradeable mLevel;
        public Upgradeable Level {
            get { return mLevel; }
        }

        public Guild( GuildProgress i_progress ) {
            mModel = new ViewModel();
            mData = GenericDataLoader.GetData<GuildData>( GenericDataLoader.GUILDS, i_progress.ID );
                        
            mLevel = new Upgradeable();
            mLevel.SetPropertyToUpgrade( mModel, mData.GuildLevel );           
            Level.Value = i_progress.Level;
        }

        public float GetUnitStatBonus( IUnit i_unit, string i_stat ) {
            float bonus = 0f;
            foreach ( UnitModificationData mod in Data.Modifications ) {
                bonus += mod.GetBonus( i_unit, i_stat, Level.Value );
            }

            return bonus;
        }       
    }
}
