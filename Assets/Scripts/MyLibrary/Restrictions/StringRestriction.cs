
namespace MyLibrary {
    public class StringRestriction {
        public string BaseValue;
        public StringComparators Comparator;

        public bool DoesPass( string i_value ) {
            bool passes = false;

            switch ( Comparator ) {
                case StringComparators.Ignore:
                    passes = true;
                    break;
                case StringComparators.Match:
                    passes = i_value == BaseValue;
                    break;
                case StringComparators.Mismatch:
                    passes = i_value != BaseValue;
                    break;
            }

            return passes;
        }
    }
}
