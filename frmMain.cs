using System;
using System.Data;
using System.Windows.Forms;

namespace MysteryGuest_INC
{
    [System.Runtime.InteropServices.GuidAttribute("8BF2DB30-B792-4DC1-8798-B9BA2CC24FA2")]
    public partial class frmMain : Form
    {
        private frmLoading frmLoadingcall = new frmLoading();
        public frmMain()
        {
            InitializeComponent();
        }

        private void AppendInfoBox(string msg)
        {
            richtextboxDebugger.Text = msg + "\r\n"
                + richtextboxDebugger.Text;
        }

        private DatabaseConnection ConnectToDatabase(DatabaseType databaseType, ConnectionType connectionType)
        {
            // Shows the loading form.
            frmLoadingcall.Show();

            //Hides the main form.
            Hide();

            var dbConnection = DatabaseManager.CreateConnection(connectionType,
                txtServername.Text, txtUsername.Text, txtPassword.Text, (string)comboboxDatabase.SelectedItem);

            try
            {
                // Attempt to open a connection.
                dbConnection.Open();

                // We've successfully connected.
                var prefix = (databaseType == DatabaseType.DatabaseTypeAccount ? "Account" : "Game") + "Database\\";

                if (checkBoxOn1.Checked)
                {
                    Program.Settings.SetInt("SaveDatabaseSettings", 1);
                    Program.Settings.SetInt(prefix + "TestPassed", 1);
                    Program.Settings.SetString(prefix + "ConnectionMethod", radioButtonODBC.Checked ? "ODBC" : "SQL");
                    Program.Settings.SetString(prefix + "Server", txtServername.Text);
                    Program.Settings.SetString(prefix + "User", txtUsername.Text);
                    Program.Settings.SetString(prefix + "Password", txtPassword.Text);
                    Program.Settings.SetString(prefix + "DatabaseName", dbConnection.GetBaseConnection().Database);
                }
                else
                {
                    Program.Settings.SetInt("SaveDatabaseSettings", 0);
                    Program.Settings.SetInt(prefix + "TestPassed", 0);
                    Program.Settings.SetString(prefix + "ConnectionMethod", "ODBC");
                    Program.Settings.SetString(prefix + "Server", "KN_online");
                    Program.Settings.SetString(prefix + "User", "knight");
                    Program.Settings.SetString(prefix + "Password", "knight");
                    Program.Settings.SetString(prefix + "DatabaseName", "");
                }

                AppendInfoBox("SQL\r\n" + "You have successfully connected to " + dbConnection.GetBaseConnection().Database 
                    + " with server name: " + txtServername.Text 
                    + " (user: " + txtUsername.Text + ", password: " + txtPassword.Text + ")");
            }
            catch (Exception ex)
            {
                // Generate the error to an message box.
                AppendInfoBox("SQL\r\n" + ex.Message);
            }
            finally
            {
                frmLoadingcall.Hide();
                Show();
            }

            return dbConnection;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var prefix = "GameDatabase\\";

            switch (DatabaseManager.ConnectionTypeFromString(Program.Settings.GetString(prefix + "ConnectionMethod")))
            {
                case ConnectionType.ConnTypeSQL:
                    radioButtonSQL.Checked = true;
                    break;

                default: // ODBC, unknown...
                    radioButtonODBC_CheckedChanged(sender, e); // ODBC is default, so we don't need to set it (just trigger the event)
                    break;
            }

            txtServername.Text = Program.Settings.GetString(prefix + "Server");
            txtUsername.Text = Program.Settings.GetString(prefix + "User");
            txtPassword.Text = Program.Settings.GetString(prefix + "Password");


            if (Program.Settings.GetInt("SaveDatabaseSettings") == 1
                && Program.Settings.GetInt(prefix + "TestPassed") == 1)
            {
                comboboxDatabase.Enabled = true;
                comboboxDatabase.DropDownStyle = ComboBoxStyle.DropDownList;

                comboboxDatabase.Items.Clear();
                comboboxDatabase.Items.Add(Program.Settings.GetString(prefix + "DatabaseName"));
                comboboxDatabase.SelectedIndex = 0;

                DatabaseManager.GameDB = ConnectToDatabase(DatabaseType.DatabaseTypeGame,
                    DatabaseManager.ConnectionTypeFromString(Program.Settings.GetString(prefix + "ConnectionMethod")));

                btnTestconnection.Text = "(connected)";
                btnTestconnection.Enabled = false;
            }
        }

        private void btnFirstnext_Click(object sender, EventArgs e)
        {
        }

        private void btnFirstcancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBoxOn1_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxOff1.Checked = !checkBoxOn1.Checked;
        }

        private void checkBoxOff1_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxOn1.Checked = !checkBoxOff1.Checked;
        }

        private void radioButtonODBC_CheckedChanged(object sender, EventArgs e)
        {
            lbServerName.Text = "Datasource name (DSN)";
            ResetDatabaseSettings();
        }

        private void radioButtonSQL_CheckedChanged(object sender, EventArgs e)
        {
            lbServerName.Text = "Server name/host";
            ResetDatabaseSettings();
        }

        private void ResetDatabaseSettings()
        {
            btnTestconnection.Text = "Get DB";
            btnTestconnection.Enabled = true;
            comboboxDatabase.Items.Clear();
            comboboxDatabase.Enabled = false;
            radiobuttonCustom.Checked = false;
            txtServername.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        private void btnTestconnection_Click(object sender, EventArgs e)
        {
            // If button text is "Get DB", after this user will be able to test this connection.
            if (btnTestconnection.Text == "Get DB")
            {
                comboboxDatabase.Enabled = true;
                comboboxDatabase.Items.Clear();

                try
                {
                    var dbConnection = DatabaseManager.CreateConnection(radioButtonODBC.Checked ? ConnectionType.ConnTypeODBC : ConnectionType.ConnTypeSQL, txtServername.Text, txtUsername.Text, txtPassword.Text);
                    using (var dbReader = dbConnection.Lookup("SELECT name FROM master..sysdatabases"))
                    {
                        while (dbReader.Read())
                            comboboxDatabase.Items.Add(dbReader.GetString(0));
                    }

                    comboboxDatabase.SelectedIndex = 0;
                    btnTestconnection.Text = "Test";
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    AppendInfoBox(ex.Message);
                    comboboxDatabase.Enabled = false;
                }
            }
            else if (btnTestconnection.Text == "Test")
            {
                DatabaseManager.GameDB = ConnectToDatabase(DatabaseType.DatabaseTypeGame, 
                    radioButtonODBC.Checked ? ConnectionType.ConnTypeODBC : ConnectionType.ConnTypeSQL);

                if (DatabaseManager.GameDB != null)
                    btnTestconnection.Text = "Use";
            }
        }

        private void checkboxManually_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxManually.Checked)
            {
                comboboxDatabase.DropDownStyle = ComboBoxStyle.DropDown;
                comboboxDatabase.Enabled = true;
                btnTestconnection.Text = "Test";
            }
            else if (!checkboxManually.Checked)
            {
                comboboxDatabase.DropDownStyle = ComboBoxStyle.DropDownList;
                comboboxDatabase.Enabled = false;
                btnTestconnection.Text = "Get DB";
            }
        }

        private void radiobuttonCustom_CheckedChanged(object sender, EventArgs e)
        {
            //Enables all check boxes.
            checkBoxOn1.Enabled = true;
            checkBoxOff1.Enabled = true;
        }

        private void radiobuttonDefault_CheckedChanged(object sender, EventArgs e)
        {
            //Checks the right settings.
            checkBoxOn1.Checked = true;
            //Disables all check boxes.
            checkBoxOn1.Enabled = false;
            checkBoxOff1.Enabled = false;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnItemsearch_Click(object sender, EventArgs e)
        { 
            if (txtDropeditoritemsearch.TextLength < 2)
            {
                AppendInfoBox("Please use at least 2 or more characters in your name. It'll be much faster for you to find what you're looking for this way.");
                return;

            }

            var itemTable = DatabaseManager.GetItemTable();
            var searchString = txtDropeditoritemsearch.Text.ToLower();

            lvwItems.Items.Clear();

            foreach (var row in itemTable)
            {
                if (!row.Value.strName.ToLower().Contains(searchString))
                    continue;

                var lvwItem = new ListViewItem();

                lvwItem.Text = row.Key.ToString();
                lvwItem.SubItems.Add(row.Value.strName);

                lvwItems.Items.Add(lvwItem);
            }
        }

        private void btnItemadd1_Click(object sender, EventArgs e)
        {
            if (lbGroupitems.SelectedItems.Count == 0)
                return;

            txtItem1.Text = (string)lbGroupitems.SelectedItems[0];
        }

        private void btnItemadd2_Click(object sender, EventArgs e)
        {
            if (lbGroupitems.SelectedItems.Count == 0)
                return;

            txtItem2.Text = (string)lbGroupitems.SelectedItems[0];
        }

        private void btnItemadd3_Click(object sender, EventArgs e)
        {
            if (lvwItems.SelectedItems.Count == 0)
                return;

            txtItem3.Text = lvwItems.SelectedItems[0].Text;
        }

        private void btnItemadd4_Click(object sender, EventArgs e)
        {
            if (lvwItems.SelectedItems.Count == 0)
                return;

            txtItem4.Text = lvwItems.SelectedItems[0].Text;
        }

        private void btnItemadd5_Click(object sender, EventArgs e)
        {
            if (lvwItems.SelectedItems.Count == 0)
                return;

            txtItem5.Text = lvwItems.SelectedItems[0].Text;
        }

        private void btnMonstersearch_Click(object sender, EventArgs e)
        {
            var monsterTable = DatabaseManager.GetMonsterTable();
            var searchString = txtDropeditormonstersearch.Text.ToLower();

            lvwMonsters.Items.Clear();

            foreach (var row in monsterTable)
            {
                if (!row.Value.strName.ToLower().Contains(searchString))
                    continue;

                var lvwItem = new ListViewItem();

                lvwItem.Text = row.Key.ToString();
                lvwItem.SubItems.Add(row.Value.strName);

                lvwMonsters.Items.Add(lvwItem);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int nMoney, sSid;
                if (!Int32.TryParse(txtNoah.Text, out nMoney))
                {
                    MessageBox.Show("Coins must be a valid number.");
                    return;
                }

                sSid = Int32.Parse(txtSsid.Text);
                var monsterTable = DatabaseManager.GetMonsterTable();
                if (!monsterTable.ContainsKey(sSid))
                    return;

                var row = monsterTable[sSid];

                DatabaseManager.GameDB.Execute("UPDATE K_MONSTER SET iMoney = ? WHERE sSid = ?", new DBParameter(nMoney), new DBParameter(sSid));
                DatabaseManager.GameDB.Execute("UPDATE K_MONSTER_ITEM SET iItem01 = ?, sPersent01 = ?, iItem02 = ?, sPersent02 = ?, iItem03 = ?, sPersent03 = ?, iItem04 = ?, sPersent04 = ?, iItem05 = ?, sPersent05 = ? WHERE sIndex = ?",
                    new DBParameter(Int32.Parse(txtItem1.Text)), new DBParameter(Int32.Parse(txtRate1.Text)),
                    new DBParameter(Int32.Parse(txtItem2.Text)), new DBParameter(Int32.Parse(txtRate2.Text)),
                    new DBParameter(Int32.Parse(txtItem3.Text)), new DBParameter(Int32.Parse(txtRate3.Text)),
                    new DBParameter(Int32.Parse(txtItem4.Text)), new DBParameter(Int32.Parse(txtRate4.Text)),
                    new DBParameter(Int32.Parse(txtItem5.Text)), new DBParameter(Int32.Parse(txtRate5.Text)),
                    new DBParameter(row.sItem));


                AppendInfoBox("Drop has been sucessfully imported\r\n" +
                              "Monster name:  " + row.strName + "\tMonster ID:  " + row.sSid.ToString() + "\r\n" +
                              "Item ID 1:  " + txtItem1.Text + "\t\t\tItem rate 1:  " + txtRate1.Text + "\r\n" +
                              "Item ID 2:  " + txtItem2.Text + "\t\t\tItem rate 2:  " + txtRate2.Text + "\r\n" +
                              "Item ID 3:  " + txtItem3.Text + "\t\t\tItem rate 3:  " + txtRate3.Text + "\r\n" +
                              "Item ID 4:  " + txtItem4.Text + "\t\t\tItem rate 4:  " + txtRate4.Text + "\r\n" +
                              "Item ID 5:  " + txtItem5.Text + "\t\t\tItem rate 5:  " + txtRate5.Text);
            }
            catch (Exception ex)
            {
                AppendInfoBox(ex.Message);
            }
        }

        private void dropEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Makes the drop editor visible.
            panelDropeditor.Visible = true;

            // Get the items in the group.
            lbGroupitems.Items.Clear();

            try
            {
                using (var dbReader = DatabaseManager.GameDB.Lookup("SELECT iItemGroupNum FROM MAKE_ITEM_GROUP"))
                {
                    while (dbReader.Read())
                        lbGroupitems.Items.Add(dbReader.GetInt32(0).ToString());
                }
            }
            catch (Exception ex)
            {
                AppendInfoBox(ex.Message);
            }
        }

        private void btnCopydebugger_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richtextboxDebugger.Text);
        }

        private void lvwMonsters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwMonsters.SelectedItems.Count == 0)
                return;

            int sSid;
            lblMonstername.Text = lvwMonsters.SelectedItems[0].SubItems[1].Text;
            txtSsid.Text = lvwMonsters.SelectedItems[0].Text;
            if (!Int32.TryParse(txtSsid.Text, out sSid))
                return;

            try
            {
                using (var dbReader = DatabaseManager.GameDB.Lookup("SELECT TOP 1 iMoney, iItem01, sPersent01, iItem02, sPersent02, iItem03, sPersent03, iItem04, sPersent04, iItem05, sPersent05 FROM K_MONSTER INNER JOIN K_MONSTER_ITEM ON sIndex = sItem WHERE sSid = ?", new DBParameter(sSid)))
                {
                    // only expecting one row
                    if (!dbReader.Read())
                    {
                        // no row exists for it, let's setup defaults and insert this row into our database (so that we can simply update later)
                        txtNoah.Text = txtItem1.Text = txtRate1.Text = txtItem2.Text = txtRate2.Text = txtItem3.Text = txtRate3.Text 
                            = txtItem4.Text = txtRate4.Text = txtItem5.Text = txtRate5.Text = "0";

                        DatabaseManager.GameDB.Execute("INSERT INTO K_MONSTER_ITEM (sIndex, iItem01, iItem02, iItem03, iItem04, iItem05, sPersent01, sPersent02, sPersent03, sPersent04, sPersent05) " +
                                                        "VALUES(?, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)", new DBParameter(sSid));
                        return;
                    }

                    txtNoah.Text = dbReader.GetInt32(0).ToString();
                    txtItem1.Text = dbReader.GetInt32(1).ToString();
                    txtRate1.Text = dbReader.GetInt32(2).ToString();
                    txtItem2.Text = dbReader.GetInt32(3).ToString();
                    txtRate2.Text = dbReader.GetInt32(4).ToString();
                    txtItem3.Text = dbReader.GetInt32(5).ToString();
                    txtRate3.Text = dbReader.GetInt32(6).ToString();
                    txtItem4.Text = dbReader.GetInt32(7).ToString();
                    txtRate4.Text = dbReader.GetInt32(8).ToString();
                    txtItem5.Text = dbReader.GetInt32(9).ToString();
                    txtRate5.Text = dbReader.GetInt32(10).ToString();
                }
            }
            catch (Exception ex)
            {
                AppendInfoBox(ex.Message);
            }
        }

        private void txtDropeditormonstersearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnMonstersearch_Click(sender, null);
        }

        private void txtDropeditoritemsearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnItemsearch_Click(sender, null);
        }

        private bool isDigit(KeyEventArgs e)
        {
            return !((e.KeyValue < '0' || e.KeyValue > '9')
                && (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete && e.KeyCode != Keys.Clear && e.KeyCode != Keys.OemClear
                && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right
                && !e.Shift && !e.Control && !e.Alt));
       }

        private void txtRate1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isDigit(e))
                e.SuppressKeyPress = true;
        }
    }
}
