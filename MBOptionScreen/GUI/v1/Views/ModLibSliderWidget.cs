using TaleWorlds.GauntletUI;

namespace MBOptionScreen.GUI.v1.Views
{
    internal class ModLibSliderWidget_v1 : SliderWidget
    {
        //private float oldFloatValue;
        //private int oldIntValue;
        private float _finalizedFloatValue;
        private int _finalizedIntValue;

        public float FinalizedFloatValue
        {
            get => _finalizedFloatValue;
            set
            {
                _finalizedFloatValue = value;
                OnPropertyChanged(value, nameof(FinalizedFloatValue));
            }
        }
        public int FinalizedIntValue
        {
            get => _finalizedIntValue;
            set
            {
                _finalizedIntValue = value;
                OnPropertyChanged(value, nameof(FinalizedIntValue));
            }
        }

        public ModLibSliderWidget_v1(UIContext context) : base(context)
        {
        }

        protected override void OnValueFloatChanged(float value)
        {
            base.OnValueFloatChanged(value);
            FinalizedFloatValue = value;
        }

        protected override void OnValueIntChanged(int value)
        {
            base.OnValueIntChanged(value);
            FinalizedIntValue = value;
        }
    }
}
