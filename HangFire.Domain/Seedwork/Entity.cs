using System;
using System.Collections.Generic;
using System.Text;

namespace HangFire.Domain.Seedwork
{
    public abstract class Entity
    {
        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
