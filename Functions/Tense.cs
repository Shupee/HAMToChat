namespace HR.Functions
{
    public class Tense
    {
        int _minValue;
        int _maxValue;
        public Tense(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }
        public int GetStress(int CurrentBPM)
        {
            if (CurrentBPM < _minValue)
                _minValue = CurrentBPM;
            if (CurrentBPM > _maxValue)
                _maxValue = CurrentBPM;
            return (int)Math.Round((decimal)(CurrentBPM - _minValue) / (_maxValue - _minValue) * 100);
        }
    }
}
