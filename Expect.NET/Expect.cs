using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    class Expect
    {
        public static ISession Spawn(ISpawnable spawnable) 
        {
            spawnable.Init();
            return new Session(spawnable);
        }
    }
}
