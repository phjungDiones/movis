using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FISMES.GLOBAL
{
    public enum fn : int
    {
        err = -1,
        busy,
        success
    };

    public class GlobalDefine
    {
        public static string STARTUP_PATH
        {
            get;
            set;
        }
    }
}
