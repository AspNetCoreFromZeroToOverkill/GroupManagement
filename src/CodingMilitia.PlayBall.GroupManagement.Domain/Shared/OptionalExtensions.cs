using System;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Shared
{
    public static class OptionalExtensions
    {
        public static Optional<TOut> Map<TIn, TOut>(this Optional<TIn> maybeValue, Func<TIn, TOut> mapper)
        {
            Ensure.NotNull(mapper, nameof(mapper));

            return maybeValue.TryGetValue(out var value)
                ? Optional.Some(mapper(value))
                : Optional.None<TOut>();
        }

        public static T ValueOr<T>(this Optional<T> maybeValue, Func<T> alternativeFactory)
        {
            Ensure.NotNull(alternativeFactory, nameof(alternativeFactory));

            return maybeValue.TryGetValue(out var value)
                ? value
                : alternativeFactory();
        }

        public static TOut MapValueOr<TIn, TOut>(this Optional<TIn> maybeValue, Func<TIn, TOut> some, Func<TOut> none)
        {
            Ensure.NotNull(some, nameof(some));
            Ensure.NotNull(none, nameof(none));

            return maybeValue.TryGetValue(out var value)
                ? some(value)
                : none();
        }

        public static void Match<T>(this Optional<T> maybeValue, Action<T> some, Action none)
        {
            Ensure.NotNull(some, nameof(some));
            Ensure.NotNull(none, nameof(none));

            if (maybeValue.TryGetValue(out var value))
                some(value);
            else
                none();
        }

        public static void MatchSome<T>(this Optional<T> maybeValue, Action<T> some)
        {
            Ensure.NotNull(some, nameof(some));

            if (maybeValue.TryGetValue(out var value))
            {
                some(value);
            }
        }

        public static async Task MatchSomeAsync<T>(this Optional<T> maybeValue, Func<T, Task> some)
        {
            Ensure.NotNull(some, nameof(some));

            if (maybeValue.TryGetValue(out var value))
            {
                await some(value);
            }
        }

        public static void MatchNone<T>(this Optional<T> maybeValue, Action none)
        {
            Ensure.NotNull(none, nameof(none));

            if (!maybeValue.TryGetValue(out _))
            {
                none();
            }
        }

        public static async Task MatchNoneAsync<T>(this Optional<T> maybeValue, Func<Task> none)
        {
            Ensure.NotNull(none, nameof(none));

            if (!maybeValue.TryGetValue(out _))
            {
                await none();
            }
        }
    }
}