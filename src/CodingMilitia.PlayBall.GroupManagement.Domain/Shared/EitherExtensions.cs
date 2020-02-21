using System;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Shared
{
    public static class EitherExtensions
    {
        public static Either<TLeftOut, TRightOut> Fold<TLeftIn, TRightIn, TLeftOut, TRightOut>(
            this Either<TLeftIn, TRightIn> result,
            Func<TLeftIn, TLeftOut> left,
            Func<TRightIn, TRightOut> right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            return result switch
            {
                Either<TLeftIn, TRightIn>.Left error => Either.Left<TLeftOut, TRightOut>(left(error.Value)),
                Either<TLeftIn, TRightIn>.Right success => Either.Right<TLeftOut, TRightOut>(right(success.Value)),
                _ => throw CreateUnexpectedResultTypeException(nameof(result))
            };
        }

        public static async Task<Either<TLeftOut, TRightOut>> FoldAsync<TLeftIn, TRightIn, TLeftOut, TRightOut>(
            this Either<TLeftIn, TRightIn> result,
            Func<TLeftIn, Task<TLeftOut>> left,
            Func<TRightIn, Task<TRightOut>> right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            return result switch
            {
                Either<TLeftIn, TRightIn>.Left error => Either.Left<TLeftOut, TRightOut>(await left(error.Value)),
                Either<TLeftIn, TRightIn>.Right success => Either.Right<TLeftOut, TRightOut>(await right(success.Value)),
                _ => throw CreateUnexpectedResultTypeException(nameof(result))
            };
        }

        public static Either<TLeft, TRightOut> Map<TLeft, TRightIn, TRightOut>(
            this Either<TLeft, TRightIn> result, Func<TRightIn, TRightOut> right)
        {
            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            return result switch
            {
                Either<TLeft, TRightIn>.Left error => Either.Left<TLeft, TRightOut>(error.Value),
                Either<TLeft, TRightIn>.Right success => Either.Right<TLeft, TRightOut>(right(success.Value)),
                _ => throw CreateUnexpectedResultTypeException(nameof(result))
            };
        }

        public static async Task<Either<TLeft, TRightOut>> MapAsync<TLeft, TRightIn, TRightOut>(
            this Either<TLeft, TRightIn> result, Func<TRightIn, Task<TRightOut>> right)
        {
            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }
            
            return result switch
            {
                Either<TLeft, TRightIn>.Left error => Either.Left<TLeft, TRightOut>(error.Value),
                Either<TLeft, TRightIn>.Right success => Either.Right<TLeft, TRightOut>(await right(success.Value)),
                _ => throw CreateUnexpectedResultTypeException(nameof(result))
            };
        }
        
        public static Either<TLeft, TRightOut> FlatMap<TLeft, TRightIn, TRightOut>(
            this Either<TLeft, TRightIn> result, Func<TRightIn, Either<TLeft, TRightOut>> right)
        {
            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            return result switch
            {
                Either<TLeft, TRightIn>.Left error => Either.Left<TLeft, TRightOut>(error.Value),
                Either<TLeft, TRightIn>.Right success => right(success.Value),
                _ => throw CreateUnexpectedResultTypeException(nameof(result))
            };
        }

        public static async Task<Either<TLeft, TRightOut>> FlatMapAsync<TLeft, TRightIn, TRightOut>(
            this Either<TLeft, TRightIn> result, Func<TRightIn, Task<Either<TLeft, TRightOut>>> right)
        {
            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }
            
            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }
            
            return result switch
            {
                Either<TLeft, TRightIn>.Left error => Either.Left<TLeft, TRightOut>(error.Value),
                Either<TLeft, TRightIn>.Right success => await right(success.Value),
                _ => throw CreateUnexpectedResultTypeException(nameof(result))
            };
        }
        
        private static Exception CreateUnexpectedResultTypeException(string parameterName)
            => new ArgumentOutOfRangeException(
                parameterName,
                "Should never happen -> Either is always Left or Right");
    }
}