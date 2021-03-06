﻿using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestNormalUnitGeneration : TestUnitGeneration {     
        private const float EXPECTED_COUNT = 1f;

        private long mTimeElapsedForOneUnit;

        protected override IEnumerator RunAllTests() {
            yield return Test_NormalUnitGeneration();
        }

        private IEnumerator Test_NormalUnitGeneration() {
            yield return SetDataForTestPrep();
            yield return SetElapsedTime();

            yield return UpdateUnitCounts( mTimeElapsedForOneUnit );

            yield return FailTestIfUnitCountDoesNotEqual( EXPECTED_COUNT );
        }

        private IEnumerator SetElapsedTime() {
            yield return GetNumberFromCloudCall( CloudTestMethods.getTrainTimeForUnit.ToString(),
                new Dictionary<string, string>() { { BackendConstants.TARGET_ID, UNIT_BEING_COUNTED },
                                                   { BackendConstants.CHANGE, EXPECTED_COUNT.ToString() } },
                ( result ) => {
                    mTimeElapsedForOneUnit = (long)result;
                } );
        }
    }
}
