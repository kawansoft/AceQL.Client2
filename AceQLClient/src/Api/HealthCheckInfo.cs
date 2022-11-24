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
    /// Contains health check info about running AceQL HTTP on remote server.
    /// </summary>
    public class HealthCheckInfo
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

        /// <summary>
        /// Gets the initialize memory.
        /// </summary>
        /// <value>The initialize memory.</value>
        public long InitMemory { get => initMemory; }
        /// <summary>
        /// Gets the used memory.
        /// </summary>
        /// <value>The used memory.</value>
        public long UsedMemory { get => usedMemory; }
        /// <summary>
        /// Gets the maximum memory.
        /// </summary>
        /// <value>The maximum memory.</value>
        public long MaxMemory { get => maxMemory; }
        /// <summary>
        /// Gets the committed memory.
        /// </summary>
        /// <value>The committed memory.</value>
        public long CommittedMemory { get => committedMemory; }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
		{
			return "HealthCheckInfo [initMemory=" + InitMemory + ", usedMemory=" + UsedMemory + ", maxMemory=" + MaxMemory + ", committedMemory=" + CommittedMemory + "]";
		}

    }

}
