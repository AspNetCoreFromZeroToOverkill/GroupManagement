namespace CodingMilitia.PlayBall.GroupManagement.Domain.Shared
{
    public abstract class Error
    {
        private Error(string message)
        {
            Message = message;
        }

        public string Message { get; }


        public abstract TResult Accept<TVisitor, TResult>(TVisitor visitor)
            where TVisitor : IErrorVisitor<TResult>;

        public interface IErrorVisitor<out TVisitResult>
        {
            TVisitResult Visit(NotFound result);

            TVisitResult Visit(Invalid result);

            TVisitResult Visit(Unauthorized result);
        }

        public sealed class NotFound : Error
        {
            public NotFound(string message) : base(message)
            {
            }

            public override TResult Accept<TVisitor, TResult>(TVisitor visitor)
                => visitor.Visit(this);
        }

        public sealed class Invalid : Error
        {
            public Invalid(string message) : base(message)
            {
            }

            public override TResult Accept<TVisitor, TResult>(TVisitor visitor)
                => visitor.Visit(this);
        }

        public sealed class Unauthorized : Error
        {
            public Unauthorized(string message) : base(message)
            {
            }

            public override TResult Accept<TVisitor, TResult>(TVisitor visitor)
                => visitor.Visit(this);
        }
    }
}