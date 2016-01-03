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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MasterPassword.Core;
using System.Security.Cryptography;
using KeePassLib;

namespace MPAGen {
    public partial class MasterPasswordDlg : Form {
        private PwDatabase database;
        public MasterPasswordDlg() {
            InitializeComponent();
        }
        public MasterPasswordDlg(PwDatabase db) : this() {
            database = db;
        }

        private void textBox2_TextChanged(object sender, EventArgs e) {
            var color = CalculateMD5Hash(textBox2.Text);
            label6.BackColor = ColorTranslator.FromHtml("#" + color.Substring(0, 6));
        }

        public static string CalculateMD5Hash(string input) {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                GeneratePassword();
            }
        }

        void GeneratePassword() {
            byte[] master = Algorithm.CalcMasterKey(textBox1.Text, textBox2.Text);
            int counter = 1;
            Int32.TryParse(textBox4.Text, out counter);
            byte[] seed = Algorithm.CalcTemplateSeed(master, textBox3.Text, counter);
            string pw = Algorithm.CalcPassword(seed, PasswordType.LongPassword);
            label5.Text = pw;
            PasswordResult = pw;
        }

        public String PasswordResult;

        private void button1_Click(object sender, EventArgs e) {
            GeneratePassword();
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        public void SetSite(string site, int counter) {
            textBox3.Text = site;
            textBox4.Text = counter.ToString();
        }

        
        private void MasterPasswordDlg_Load(object sender, EventArgs e) {
            if (database != null) {
                textBox1.Text = database.DefaultUserName;
            }
        }

        public string GetSiteName() { return textBox3.Text; }
        public string GetSiteCounter() { return textBox4.Text; }

        private void MasterPasswordDlg_Shown(object sender, EventArgs e) {
            if (textBox1.Text == "") textBox1.Focus(); else textBox2.Focus();
        }

    }
}
