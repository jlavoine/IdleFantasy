using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestNormalMissionSuccess : TestMission {
        protected override Dictionary<int, MissionTaskProposal> GetTaskProposals() {
            return GetValidMissionProposal();
        }

        protected override string GetUnitProgressData() {
            return GetValidUnitProgressForMission();
        }

        protected override bool IsTestExpectedToFail() {
            return false;
        }

        protected override IEnumerator RunOtherFailureChecks() {
            yield return StartCoroutine(base.RunOtherFailureChecks());

            yield return FailIfMissionNotComplete();
            yield return FailIfMissionRewardNotApplied();            
        }

        private IEnumerator FailIfMissionNotComplete() {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.SAVE_KEY, BackendConstants.MISSION_PROGRESS } };
            yield return mBackend.WaitForCloudCall( CloudTestMethods.getReadOnlyData.ToString(), cloudParams, ( results ) => {
                Dictionary<string, WorldMissionProgress> worldMissionProgress = JsonConvert.DeserializeObject<Dictionary<string, WorldMissionProgress>>( results[BackendConstants.DATA] );
                WorldMissionProgress progressForWorld = worldMissionProgress[MISSION_WORLD];
                SingleMissionProgress progressForMission = progressForWorld.Missions[0];
                if ( !progressForMission.Completed ) {
                    IntegrationTest.Fail( "Mission should be complete but it was not." );
                }
            } );
        }

        private IEnumerator FailIfMissionRewardNotApplied() {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.TYPE, VirtualCurrencies.GOLD } };
            FailTestIfReturnedCallDoesNotEqual( CloudTestMethods.getPlayerCurrency.ToString(), MISSION_GOLD_REWARD, cloudParams );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}
