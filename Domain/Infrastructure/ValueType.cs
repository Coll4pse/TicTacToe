using System.Linq;
using System.Reflection;

namespace Domain.Infrastructure
{
    /// <summary>
    ///     Базовый класс типов-значений
    /// </summary>
    /// <typeparam name="T">Тип наследуещегося класса</typeparam>
    public abstract class ValueType<T>
    {
        private static readonly PropertyInfo[] Properties =
            typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        protected bool Equals(T other)
        {
            if (ReferenceEquals(null, other)) return false;
            foreach (var propertyInfo in Properties)
            {
                var thisPropertyValue = propertyInfo.GetValue(this);
                var otherPropertyValue = propertyInfo.GetValue(other);

                if (!Equals(thisPropertyValue, otherPropertyValue)) return false;
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            return Equals((T) obj);
        }

        public override int GetHashCode()
        {
            return string.Join(null, Properties.Select(prop => prop.GetValue(this)?.GetHashCode())).GetHashCode();
        }

        public override string ToString()
        {
            var formattedProps = string.Join(";",
                Properties
                    .OrderBy(p => p.Name)
                    .Select(p => $"{p.Name}: {p.GetValue(this)}"));
            return '(' + formattedProps + ')';
        }
    }
}