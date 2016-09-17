using MyLibrary;
using System;

namespace IdleFantasy {
    public class TravelOption : GenericViewModel {
        private int mOptionIndex;

        public const string NAME_PROPERTY = "Name";       

        public TravelOption( IMapName i_mapName, int i_optionIndex, IWorldMissionProgress i_missionProgress ) : base() {
            mOptionIndex = i_optionIndex;

            string name = GetOptionName( i_mapName, i_missionProgress, mOptionIndex );
            SetOptionName( name );
        }

        public string GetOptionName( IMapName i_mapName, IWorldMissionProgress i_missionProgress, int i_optionIndex ) {
            bool isAvailable = IsOptionAvailable( i_missionProgress, i_optionIndex );
            
            return isAvailable ? i_mapName.GetStringName() : GetUnavailableText( i_missionProgress, i_optionIndex );
        }

        public string GetUnavailableText( IWorldMissionProgress i_missionProgress, int i_optionIndex ) {
            int numMoreClears = GetClearsUntilAvailable( i_missionProgress, i_optionIndex );
            string text = StringTableManager.Get( StringKeys.TRAVEL_OPTION_UNAVAILABLE );
            text = DrsStringUtils.Replace( text, DrsStringUtils.NUM, numMoreClears );

            return text;
        }

        public bool IsOptionAvailable( IWorldMissionProgress i_missionProgress, int i_optionIndex ) {
            int numMoreClears = GetClearsUntilAvailable( i_missionProgress, i_optionIndex );            
            return numMoreClears <= 0;
        }

        public int GetClearsUntilAvailable( IWorldMissionProgress i_missionProgress, int i_optionIndex ) {
            int requiredClearCount = GetRequiredMapClearCount( i_optionIndex );
            int clearedAreas = i_missionProgress.GetCompletedMissionCount();

            return Math.Max(0, requiredClearCount - clearedAreas);
        }

        public int GetRequiredMapClearCount( int i_optionIndex ) {
            int areasPerOption = Constants.GetConstant<int>( ConstantKeys.AREAS_PER_TRAVEL_OPTION );
            return areasPerOption * ( i_optionIndex + 1 );
        }

        public void SetOptionName( string i_name ) {
            ViewModel.SetProperty( NAME_PROPERTY, i_name );
        }

        public void TravelToOption() {
            SendTravelOptionSelectedMessage();
            SendTravelRequestToServer();            
        }

        private void SendTravelOptionSelectedMessage() {
            MyMessenger.Send( MapKeys.TRAVEL_TO_REQUEST, mOptionIndex );
        }

        private void SendTravelRequestToServer() {
            BackendManager.Backend.SendTravelRequest( BackendConstants.WORLD_BASE, mOptionIndex );
        }
    }
}