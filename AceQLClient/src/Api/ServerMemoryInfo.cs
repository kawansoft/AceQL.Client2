/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2022,  KawanSoft SAS
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
using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api
{
    /// <summary>
    /// Contains health check Java memory info of the AceQL server running instance. 
    /// </summary>
    public class ServerMemoryInfo
	{

        /// <summary>
        /// The initialize memory
        /// </summary>
        private long initMemory = -1;
        /// <summary>
        /// The used memory
        /// </summary>
        private long usedMemory = -1;
        /// <summary>
        /// The maximum memory
        /// </summary>
        private long maxMemory = -1;
        /// <summary>
        /// The committed memory
        /// </summary>
        private long committedMemory = -1;

        internal void SetCommittedMemory(long committedMemory)
        {
            this.committedMemory = committedMemory;
        }

        internal void SetInitMemory(long initMemory)
        {
            this.initMemory = initMemory;
        }

        internal void SetMaxMemory(long maxMemory)
        {
            this.maxMemory = maxMemory;
        }

        internal void SetUsedMemory(long usedMemory)
        {
            this.usedMemory = usedMemory;
        }

        ///<summary>
        ///Returns the amount of memory in bytes that the Java virtual machine
        ///initially requests from the operating system for memory management.
        ///This method returns -1 if the initial memory size is undefined.
        ///</summary>
        /// <value>The initial size of memory in bytes.</value>
        public long InitMemory { get => initMemory; }

        /// <summary>
        /// Returns the amount of used memory in bytes.
        /// </summary>
        /// <value>the amount of used memory in bytes.</value>
        public long UsedMemory { get => usedMemory; }

        /// <summary>
        /// Returns the maximum amount of memory in bytes that can be
        /// used for memory management.  This method returns <tt>-1</tt>
        /// if the maximum memory size is undefined.
        ///
        /// This amount of memory is not guaranteed to be available
        /// for memory management if it is greater than the amount of
        /// committed memory.  The Java virtual machine may fail to allocate
        /// memory even if the amount of used memory does not exceed this
        /// maximum size.
        /// </summary>
        /// <value>The maximum amount of memory in bytes; -1 if undefined.</value>
        public long MaxMemory { get => maxMemory; }

        /// <summary>
        /// Returns the amount of memory in bytes that is committed for
        /// the Java virtual machine to use.  This amount of memory is
        /// guaranteed for the Java virtual machine to use.
        /// </summary>
        /// <value>The amount of committed memory in bytes.</value>
        public long CommittedMemory { get => committedMemory; }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
		{
			return "ServerMemoryInfo [initMemory=" + InitMemory + ", usedMemory=" + UsedMemory + ", maxMemory=" + MaxMemory + ", committedMemory=" + CommittedMemory + "]";
		}

    }

}
