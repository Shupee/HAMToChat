namespace HR.Functions
{
    internal class Waiter
    {
        private bool _sending;
        private readonly Action _action;
        private int _waitTimeInMs;

        public Waiter(Action action, int waitTimeInMs)
        {
            _sending = false;
            _waitTimeInMs = waitTimeInMs;
            _action = action;
        }
        public void ChangeTime(int newTimeInMs)
         => _waitTimeInMs = newTimeInMs;

        public void Start()
        {
            _sending = true;
            new Thread(new ParameterizedThreadStart(delegate
            {
                while (_sending)
                {
                    Thread.Sleep(_waitTimeInMs);
                    _action();
                }
            })).Start();
        }

        public void Stop()
        => _sending = false;
    }
}
