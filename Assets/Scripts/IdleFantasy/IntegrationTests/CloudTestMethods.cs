
// The enums here are actually method names in the cloudscript used for integration tests.
// I did it like this so I could have another integration test automatically test every method,
// and fail if the method isn't "protected" by the isTesting() method.
// i.e. non-test users should fail the calls.
namespace IdleFantasy {
    public enum CloudTestMethods {
        setSaveData,
        setPlayerCurrency,
        testUpdateUnitCount,
        testUpgrade,
        testChangeTraining,
        getCapacityForUnit,
        getTrainTimeForUnit,
        getTotalPointsToUpgrade,
        getMaxLevelForUpgrade,
        getAvailableTrainers,
        getInternalData,
        getReadOnlyData,
        getTrainerCount,
        getProgressData,
        getUnitCount,
        addPointsToUpgrade,
        addProgressToUpgrade,
        getTestMissions,
        createMapForTesting,
        getDefaultMapAreaWeights,
        deleteAllPlayerReadOnlyData,
        addMissingPlayerData,
        getAllPlayerReadOnlySaveData
    }
}