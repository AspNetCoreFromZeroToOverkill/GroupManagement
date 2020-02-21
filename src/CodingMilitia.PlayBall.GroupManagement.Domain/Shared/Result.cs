namespace CodingMilitia.PlayBall.GroupManagement.Domain.Shared
{
    public static class Result
    {
        public static Either<Error, TValue> Success<TValue>(TValue value)
            => Either.Right<Error, TValue>(value);
        
        public static Either<Error, TValue> Invalid<TValue>(string message)
            => Either.Left<Error, TValue>(new Error.Invalid(message));

        public static Either<Error, TValue> NotFound<TValue>(string message)
            => Either.Left<Error, TValue>(new Error.NotFound(message));

        public static Either<Error, TValue> Unauthorized<TValue>(string message)
            => Either.Left<Error, TValue>(new Error.Unauthorized(message));
    }
}