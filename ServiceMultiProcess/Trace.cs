using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ServiceMultiProcess
{
    class Trace
    {
        /// <summary>
        /// To simply write logs in to a text file to keep track and to run Unit tests
        /// </summary>
        /// <param name="text"> Text to write in the trace file for error handling</param>
        public static void WriteLog(string text)
        {           
            StreamWriter SW = new StreamWriter("D:\\logs\\trace.txt", true);
            SW.WriteLine(text);
            SW.Flush();
            SW.Close();           
        }
    }
}
