/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2021,  KawanSoft SAS
 * (http://www.kawansoft.com). All rights reserved.                                
 *                                                                               
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this filePath except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. 
 */

using AceQL.Client.Api.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Api.Util
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
