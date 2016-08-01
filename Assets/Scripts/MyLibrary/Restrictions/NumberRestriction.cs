
namespace MyLibrary {
    public class NumberRestriction {
        public string Key;
        public float Min;
        public float Max;

        public bool DoesPass( float i_num ) {
            return i_num >= Min && i_num <= Max;
        }
    }
}