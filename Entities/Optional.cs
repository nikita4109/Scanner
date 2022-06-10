namespace Entities
{
    public class Optional<T>
    {
        private T _value;
        private static readonly Optional<T> _emptyInstance = new Optional<T>();

        public bool IsPresent { get; private set; } = false;

        private Optional()
        {
        }

        public static Optional<T> Empty()
        {
            return _emptyInstance;
        }

        public static Optional<T> Of(T value)
        {
            Optional<T> obj = new Optional<T>();
            obj.Set(value);
            return obj;
        }

        public void Set(T value)
        {
            _value = value;
            IsPresent = true;
        }

        public T Get()
        {
            return _value;
        }
    }
}