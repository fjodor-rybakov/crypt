namespace crypt
{
    public class Counter
    {
        private int _counter;
        private readonly int _noMore;

        public Counter(int noMore, int initValue = 0)
        {
            _noMore = noMore;
            _counter = initValue;
        }

        public int GetNewValue()
        {
            if (_counter <= _noMore) return _counter++;
            while (_counter > _noMore)
            {
                _counter -= _noMore;
            }

            return _counter;
        }
    }
}