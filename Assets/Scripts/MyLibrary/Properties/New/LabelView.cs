
namespace MyLibrary {
    public class LabelView : BaseLabelView {
        public override void UpdateView() {
            object propertyValue = GetValue<object>();
            string label = propertyValue.ToString();

            SetText( label );
        }
    }
}
