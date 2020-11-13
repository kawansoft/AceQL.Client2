
using AceQL.Client.Api.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Src.Api.Util
{
    /// <summary>
    /// Class Tracer. Allows to create a single instance for a trace action.
    /// </summary>
    internal class SimpleTracer
    {
        /// <summary>
        /// The trace on
        /// </summary>
        private bool traceOn;

        /// <summary>
        /// The trace filePath for debug
        /// </summary>
        private string filePath;

        /// <summary>
        /// Says if trace is on
        /// </summary>
        /// <returns>true if trace is on</returns>
        internal bool IsTraceOn()
        {
            return traceOn;
        }

        /// <summary>
        /// Sets the trace on/off
        /// </summary>
        /// <param name="traceOn">if true, trace will be on; else race will be off</param>
        internal void SetTraceOn(bool traceOn)
        {
            this.traceOn = traceOn;
        }

        /// <summary>
        /// Traces this instance.
        /// </summary>
        internal void Trace()
        {
            Trace("");
        }

        /// <summary>
        /// Gets the name of the trace filePath.
        /// </summary>
        /// <returns>The name name fo the trace filePath</returns>
        internal string GetTraceFileName()
        {
            if (filePath == null)
            {
                filePath = AceQLCommandUtil.GetTraceFile();
            }

            return filePath;
        }

        /// <summary>
        /// Traces the specified string.
        /// </summary>
        /// <param name="contents">The string to trace</param>
        internal void Trace(String contents)
        {
            if (traceOn)
            {
                if (filePath == null)
                {
                    AceQLCommandUtil.GetTraceFile();
                }
                contents = DateTime.Now + " " + contents;
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(contents);
                }
            }
        }
    }
}
