using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ServiceMultiProcess
{
    static class Program
    {
        /// <summary>
        /// Service application starting point 
        /// </summary>
        static void Main()
        {
                //Short Cut Key CTRL + K+ S

                #if DEBUG            
                    ClassMP.LaunchThreads();

                #else                
                            ServiceBase[] ServicesToRun;
                            ServicesToRun = new ServiceBase[] 
			                { 
				                new ClassMP() 
			                };
                            ServiceBase.Run(ServicesToRun);
                #endif
        }
    }
}
