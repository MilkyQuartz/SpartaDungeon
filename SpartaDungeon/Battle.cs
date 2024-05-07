using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public interface ICritical
    {
        public void CheckCritical(ref int attackDamage, ref bool isCritical);

    }

    public interface IDamage
    {
        public void TakeDamage(int damage, bool isCritical);
    }
}
