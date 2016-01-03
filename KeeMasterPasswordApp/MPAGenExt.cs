/*
  MasterPasswordApp compatible PwGenerator Plugin
  Copyright (C) 2016 Max Weller <max@teamwiki.de>

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using KeePass.Plugins;

namespace MPAGen
{
	public sealed class MPAGenExt : Plugin
	{
		private IPluginHost pluginHost = null;
		private MPAGen pwGen = null;

		public override bool Initialize(IPluginHost host)
		{
			if(host == null) return false;
			pluginHost = host;

			pwGen = new MPAGen(host);
			pluginHost.PwGeneratorPool.Add(pwGen);
            

			return true;
		}

		public override void Terminate()
		{
			if(pluginHost != null)
			{
				pluginHost.PwGeneratorPool.Remove(pwGen.Uuid);
				pwGen = null;
				pluginHost = null;
			}
		}
	}
}
