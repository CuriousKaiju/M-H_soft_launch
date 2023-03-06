using UnityEngine;

namespace Obi
{
    public class ObiColorPaintBrushMode : IObiBrushMode
    {
        ObiBlueprintColorProperty property;

        public ObiColorPaintBrushMode(ObiBlueprintColorProperty property)
        {
            this.property = property;
        }

        public string name
        {
            get { return "Paint"; }
        }

        public bool needsInputValue
        {
            get { return true; }
        }

        public void ApplyStamps(ObiBrushBase brush, bool modified)
        {
            for (int i = 0; i < brush.weights.Length; ++i)
            {
                if (!property.Masked(i) && brush.weights[i] > 0)
                {
                    Color currentValue = property.Get(i);
                    Color delta = brush.weights[i] * brush.opacity * brush.speed * (property.GetDefault() - currentValue);

                    property.Set(i, currentValue + delta * (modified ? -1 : 1));
                }
            }
        }
    }
}