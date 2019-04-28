using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class CovertibleExtensions
    {
        public static T To<T>(this IConvertible value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
