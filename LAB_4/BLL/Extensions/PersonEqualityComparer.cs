using LAB_4.DAL.Models;

namespace LAB_4.BLL.Extensions
{
    public class PersonEqualityComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person? x, Person? y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            return x.Id == y.Id;
        }

        public int GetHashCode(Person obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj.Id.GetHashCode();
        }
    }
}
