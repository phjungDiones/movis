using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FISMES.DATA
{
    public class GlobalVariable1
    {
        public bool bFistConnect { get; set; }




        private static GlobalVariable1 instance;
        private static object syncRoot = new Object();

        private GlobalVariable1() { }

        public static GlobalVariable1 Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new GlobalVariable1();
                    }
                }
                return instance;
            }
        }





    }
}
