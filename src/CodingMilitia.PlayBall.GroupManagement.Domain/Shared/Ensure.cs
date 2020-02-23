using System;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.Shared
{
    public static class Ensure
    {
        public static T NotNull<T>(T parameterValue, string parameterName)
        {
            if (parameterValue is null) throw new ArgumentNullException(parameterName);

            return parameterValue;
        }

        public static void NotNull(params (object value, string name)[] parameters)
        {
            foreach (var (value, name) in parameters)
                if (value is null)
                    throw new ArgumentNullException(name);
        }
    }
}