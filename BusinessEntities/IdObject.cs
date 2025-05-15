using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessEntities
{
    public abstract class IdObject
    {
        public string Id { get; protected set; } = Guid.NewGuid().ToString();

        public bool EqualsById(IdObject other)
        {
            return other != null && Id.Equals(other.Id);
        }

        public static T GetById<T>(IEnumerable<T> items, string id) where T : IdObject
        {
            return items.FirstOrDefault(item => item.Id == id);
        }

        public static IList<string> Ids<T>(IEnumerable<T> items) where T : IdObject
        {
            return items.Select(q => q.Id).ToList();
        }

        public static bool EqualsById(IdObject a, IdObject b)
        {
            return a?.EqualsById(b) ?? b == null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.GetType() == GetType() && Id.Equals(((IdObject)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}