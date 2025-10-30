namespace Assecor.Backend.Domain.Maybe
{
    public readonly struct Maybe<T>
    {
        private readonly T _value;
        public bool HasValue { get; }

        public T Value => HasValue 
            ? _value 
            : throw new InvalidOperationException("Maybe has no value");

        public string GetTypeName() => typeof(T).Name;

        private Maybe(T value)
        {
            _value = value;
            HasValue = true;
        }

        /// <summary>
        /// get a maybe of given type with given value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Maybe<T> From(T? value) => value != null ? new Maybe<T>(value) : None();

        private static Maybe<T> None() => new();

        public Maybe<TOut> Map<TOut>(Func<T, TOut> mappingFunc)
        {
            return mappingFunc == null 
                ? throw new ArgumentNullException(nameof(mappingFunc)) 
                : HasValue 
                    ? new Maybe<TOut>(mappingFunc(_value)) 
                    : Maybe<TOut>.None();
        }
    }

    public static class Maybe
    {
        /// <summary>
        /// get a maybe of given type with given value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Maybe<T> From<T>(T? value) => Maybe<T>.From(value);
    }
}
