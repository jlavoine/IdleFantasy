
namespace IdleFantasy.PlayFab.IntegrationTests {
    public class UpgradeTestData {
        public string SaveKey;
        public string SaveValue;
        public string TestID;
        public string TestClass;
        public string TestUpgradeID;

        public int MaxLevel = 50;
        public int Cost = 1000;

        // points upgrades only...TODO, make a child class?
        public int Points;      
    }
}