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

using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using KeePass.Plugins;
using KeePass.Forms;
using System.Text.RegularExpressions;

namespace MPAGen
{
	public sealed class MPAGen : CustomPwGenerator
	{
        IPluginHost keePass;
        // {231B3120-C11D-4816-9578-F359C7E5F6AB}
        private PwUuid generatorUUID = new PwUuid(new byte[] {
			    0x23, 0x1b, 0x31, 0x20, 0xc1, 0x1d, 0x48, 0x16, 
                0x95, 0x78, 0xf3, 0x59, 0xc7, 0xe5, 0xf6, 0xab });
            
        public override PwUuid Uuid {
            get {
                return generatorUUID;
            }
        }
        public MPAGen(IPluginHost host) : base() {
            keePass = host;
        }

		public override string Name
		{
			get { return "MasterPasswordApp"; }
		}

        private PwEntryForm GetCurrentPwEntryForm() {
            var frm = System.Windows.Forms.Form.ActiveForm;
            if (frm == null) return null;
            if (frm is PwEntryForm) return (PwEntryForm)frm;
            if (frm.Owner != null && frm.Owner is PwEntryForm) return (PwEntryForm)frm.Owner;
            return null;
        }

		public override ProtectedString Generate(PwProfile prf,
			CryptoRandomStream crsRandomSource)
		{
            PwEntryForm entry = GetCurrentPwEntryForm();
            entry.UpdateEntryStrings(true, false);
            if (entry == null) {
                System.Windows.Forms.MessageBox.Show("No Current PwEntry found!");
                return null;
            }
            
            MasterPasswordDlg dlg = new MasterPasswordDlg(keePass.Database);

            string site = "";
            site = entry.EntryStrings.ReadSafe("MPA Site");
            if (string.IsNullOrEmpty(site))
                site =Regex.Replace( entry.EntryStrings.ReadSafe("URL"),
                    @"^https?://(?:[^./]+\.)*([^./]+\.(?:[a-z.]{2,5}|[a-z]+))(?:[/].*)?$", "$1");
            
            int counter;
            if (!Int32.TryParse(entry.EntryStrings.ReadSafe("MPA Counter"), out counter)) counter=1;
            
            dlg.SetSite(site, counter);

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                if (site != dlg.GetSiteName()) 
                    entry.EntryStrings.Set("MPA Site", new ProtectedString(false, dlg.GetSiteName()));
                if (counter.ToString() != dlg.GetSiteCounter()) 
                    entry.EntryStrings.Set("MPA Counter", new ProtectedString(false, dlg.GetSiteCounter()));
                entry.UpdateEntryStrings(false, false);

                return new ProtectedString(true, dlg.PasswordResult);
            }
            return null;
            /*
			if(prf == null) { Debug.Assert(false); }
			else
			{
				Debug.Assert(prf.CustomAlgorithmUuid == Convert.ToBase64String(
					m_uuid.UuidBytes, Base64FormattingOptions.None));
			}
            
			PwProfile prfSub = new PwProfile();
			prfSub.GeneratorType = PasswordGeneratorType.CharSet;
			prfSub.CharSet = new PwCharSet(PwCharSet.UpperCase + PwCharSet.LowerCase +
				PwCharSet.Digits + m_strSpecialCharSet);
			prfSub.CollectUserEntropy = false;
			prfSub.ExcludeCharacters = string.Empty;
			prfSub.ExcludeLookAlike = false;
			prfSub.Length = 11;
			prfSub.NoRepeatingCharacters = false;

			string strPw;
			while(true)
			{
				ulong u = crsRandomSource.GetRandomUInt64();
				if(u >= 18446744073709551610UL) continue;
				u %= 10UL;

				strPw = new string((char)('0' + (int)u), 1);

				byte[] pbEntropy = crsRandomSource.GetRandomBytes(32);
				ProtectedString psSub;
				if(PwGenerator.Generate(out psSub, prfSub, pbEntropy, null) !=
					PwgError.Success) { Debug.Assert(false); continue; }

				strPw += psSub.ReadString();
				if(Validate(strPw)) break;
			}

			Debug.Assert(strPw.Length == 12);
			return new ProtectedString(false, strPw);*/
		}

	}
}
