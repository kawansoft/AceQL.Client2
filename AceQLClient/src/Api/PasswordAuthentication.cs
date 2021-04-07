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

/*
 * Copyright (c) 1997, 2013, Oracle and/or its affiliates. All rights reserved.
 * ORACLE PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 */

namespace AceQL.Client.Api
{

	/// <summary>
	/// The class PasswordAuthentication is a data holder that is used by
	/// Authenticator. It is simply a repository for a user name and a password.
	/// </summary>
	public sealed class PasswordAuthentication
	{

		private string userName;
		private char[] password;

		/// <summary>
		/// Creates a new {@code PasswordAuthentication} object from the given
		/// user name and password.
		/// 
		/// <para> Note that the given user password is cloned before it is stored in
		/// the new PasswordAuthentication object.
		/// 
		/// </para>
		/// </summary>
		/// <param name="userName"> the user name </param>
		/// <param name="password"> the user's password </param>
		public PasswordAuthentication(string userName, char[] password)
		{
			this.userName = userName;
			this.password = (char[])password.Clone();
		}

		/// <summary>
		/// Returns the user name.
		/// </summary>
		/// <returns> the user name </returns>
		public string UserName
		{
			get
			{
				return userName;
			}
		}

		/// <summary>
		/// Returns the user password.
		/// 
		/// <para> Note that this method returns a reference to the password. It is
		/// the caller's responsibility to zero out the password information after
		/// it is no longer needed.
		/// 
		/// </para>
		/// </summary>
		/// <returns> the password </returns>
		public char[] Password
		{
			get
			{
				return password;
			}
		}
	}
}
