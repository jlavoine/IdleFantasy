using System.Collections.Generic;
using MyLibrary;

namespace IdleFantasy {
    public class UnitInfoPM : PresentationModel {
        private List<IUnit> mUnits;
        public List<IUnit> Units { get { return mUnits; } }

        private int mSelectedIndex;
        public int SelectedIndex { get { return mSelectedIndex; } private set { mSelectedIndex = value; } }

        private List<UnitStatPM> mStatPMs;
        public List<UnitStatPM> StatPMs { get { return mStatPMs; } private set { mStatPMs = value; } }

        public IUnit SelectedUnit { get { return Units[SelectedIndex];  } }

        private IStatCalculator mStatCalculator;

        public const string UNIT_ID_PROPERTY = "UnitID";
        public const string UNIT_NAME_PROPERTY = "UnitName";

        public UnitInfoPM( List<IUnit> i_units, int i_selectedIndex, IStatCalculator i_statCalculator ) : base() {        
            mUnits = i_units;
            mSelectedIndex = i_selectedIndex;
            mStatCalculator = i_statCalculator;

            RefreshPM();
        }

        public void GoToNextUnit() {
            ModifyUnitIndexByAmount( 1 );
            RefreshPM();
        }

        public void GoToPreviousUnit() {
            ModifyUnitIndexByAmount( -1 );
            RefreshPM();
        }

        private void ModifyUnitIndexByAmount( int i_amount ) {
            SelectedIndex += i_amount;
            if ( SelectedIndex >= Units.Count ) {
                SelectedIndex = 0;
            } else if ( SelectedIndex < 0 ) {
                SelectedIndex = Units.Count-1;
            }
        }

        private void RefreshPM() {
            SetStatPMs();
            SetUnitIdProperty();
            SetUnitNameProperty();
        }

        private void SetStatPMs() {
            StatPMs = new List<UnitStatPM>();
            foreach ( string stat in SelectedUnit.GetStats() ) {
                StatPMs.Add( new UnitStatPM( SelectedUnit, stat, mStatCalculator ) );
            }
        }

        private void SetUnitIdProperty() {
            ViewModel.SetProperty( UNIT_ID_PROPERTY, SelectedUnit.GetID() );
        }

        private void SetUnitNameProperty() {
            ViewModel.SetProperty( UNIT_NAME_PROPERTY, SelectedUnit.GetName() );
        }
    }
}