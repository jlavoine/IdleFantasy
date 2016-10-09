using System.Collections.Generic;
using MyLibrary;
using System;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMapMissions {
        private int mMapLevel;
        private float mGoldModAmount;

        public TestMapMissions( List<MapAreaData> i_areas, List<MapModification> i_modifications, int i_mapLevel ) {
            mMapLevel = i_mapLevel;
            mGoldModAmount = i_modifications.GetModAmount( BackendConstants.BASE_GOLD_MOD );

            foreach ( MapAreaData areaData in i_areas ) {
                TestMission( areaData.Mission, areaData.AreaType );    
            }
        }

        private void TestMission( MissionData i_mission, MapAreaTypes i_areaType ) {
            TestMissionTaskLength( i_mission );
            TestMissionTaskContent( i_mission, i_areaType );
            TestMissionReward( i_mission.GoldReward );
        }

        private void TestMissionTaskContent( MissionData i_mission, MapAreaTypes i_areaType ) {
            foreach ( MissionTaskData taskData in i_mission.Tasks ) {
                TestPowerForMission( taskData.PowerRequirement );
            }
        }

        private void TestPowerForMission( int i_powerRequirement ) {
            int expectedPower = Constants.GetConstant<int>( ConstantKeys.BASE_MISSION_STAT_POWER ) * ( mMapLevel + 1 );

            if ( i_powerRequirement != expectedPower ) {
                IntegrationTest.Fail( "Mission expecting " + expectedPower + " power but was " + i_powerRequirement );
            }
        }

        private void TestMissionTaskLength( MissionData i_mission ) {
            // for alpha, length is always 1
            if ( i_mission.Tasks.Count != 1 ) {
                IntegrationTest.Fail( "Mission task length should be 1, but is " + i_mission.Tasks.Count );
            }
        }

        private void TestMissionReward( int i_goldReward ) {
            int baseGold = Constants.GetConstant<int>( ConstantKeys.BASE_GOLD_REWARD );
            double goldCoefficient = Constants.GetConstant<double>( ConstantKeys.GOLD_REWARD_COEFFICIENT );
            int expectedGold = (int)Math.Ceiling(baseGold * Math.Pow( goldCoefficient, 0));
            expectedGold += expectedGold * (int)Math.Ceiling( expectedGold * mGoldModAmount );

            if ( expectedGold != i_goldReward ) {
                IntegrationTest.Fail( "Expecting " + expectedGold + " gold reward but got " + i_goldReward );
            }
        }
    }
}
