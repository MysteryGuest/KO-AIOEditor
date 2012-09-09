using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.Win32;
using MySql.Data.MySqlClient;

namespace MysteryGuest_INC
{
    [System.Runtime.InteropServices.GuidAttribute("8BF2DB30-B792-4DC1-8798-B9BA2CC24FA2")]
    public partial class frmMain : Form
    {
        //Will decide if we gonna use the statement update or insert.
        bool blInsert;
        //Declaring variables.
        //Decides if need to keep connection alive or not.
        //0 = OFF
        //1 = ON
        //2 = Not set
        int connectionKeeper = 2;
        //Decides if connection has to be saved
        //0 = OFF
        //1 = ON
        //2 = Not set
        int saveConnection = 2;
        //Decides what kind of connection gonna be used.
        //1 = ODBC
        //2 = SQL
        int typeConnection = 1;
        //Declare slide effect.
        //0 = OFF
        int slideEffect = 0;
        //Count's the total amount of error's made that session...
        int amountError;
        //This is the total time that the program did run.
        int runTimeseconds;
        int runTimeminuts;
        int runTimehours;
        //This is for our rates, items...
        #region
        //Make the new int's, is being used for the "Insert" query.
        int item1;
        int item2;
        int item3;
        int item4;
        int item5;
        int rate1;
        int rate2;
        int rate3;
        int rate4;
        int rate5;
        #endregion
        //TEST CONNECTION   
        //Declare new connection.
        MySqlConnection sqlConnection = new MySqlConnection(
                "SERVER=108.167.174.76,3306;" +
                "DATABASE=mg18_INC;" +
                "UID=mg18_INC;" +
                "PASSWORD=;");
        //SQL
        SqlConnection sqlCon = new SqlConnection();
        //ODBC
        OdbcConnection odbcCon = new OdbcConnection();

        //Declaring a new form.
        frmLoading frmLoadingcall = new frmLoading();


        //Setting our main key [is being used to save current settings of the program.
        RegistryKey keyOpen = Registry.CurrentUser.OpenSubKey(@"Software\MysterGuest inc\Beta001\", true);
        RegistryKey keyCreate = Registry.CurrentUser.CreateSubKey(@"Software\MysterGuest inc\Beta001\");
        //This is for ODBC settings.
        RegistryKey keyOpenodbc = Registry.CurrentUser.OpenSubKey(@"Software\MysterGuest inc\Beta001\ODBC\", true);
        RegistryKey keyCreateodbc = Registry.CurrentUser.CreateSubKey(@"Software\MysterGuest inc\Beta001\ODBC\");
        //This is for SQL settings.
        RegistryKey keyOpensql = Registry.CurrentUser.OpenSubKey(@"Software\MysterGuest inc\Beta001\SQL\", true);
        RegistryKey keyCreatesql = Registry.CurrentUser.CreateSubKey(@"Software\MysterGuest inc\Beta001\SQL\");
        //This is for making the ODBC working
        //RegistryKey keyOpenODBCX = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources", true);
        //RegistryKey keyOpenODBC = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBC.INI\KN_Online", true);
        public frmMain()
        {
            InitializeComponent();
        }

        //Here all connection functions will be stored.
        #region
        private void SQLstandardConnection()
        {
            //Shows the loading form.
            frmLoadingcall.Show();
            //Refreshes the loading form so we don t get any glitches.
            frmLoadingcall.Refresh();
            //Hides the main form.
            this.Hide();
            //Connection string.
            sqlCon.ConnectionString =
                        "SERVER=" + txtServername.Text + ";" +
                        "DATABASE="+ comboboxDatabase.SelectedItem.ToString() + ";" +
                        "UID=" + txtUsername.Text + ";"+
                        "PASSWORD=" + txtPassword.Text + ";";
            try
            {
                //Opens the connection.
                sqlCon.Open();
                //Closes the loading form.
                frmLoadingcall.Hide();
                //Shows the main form.
                this.Show();
                //Shows results to debugger.
                richtextboxDebugger.Text = richtextboxDebugger.Text + "SQL" + '\r';
                richtextboxDebugger.Text = richtextboxDebugger.Text + "You have successful connected to " + comboboxDatabase.Text + " By using server name: " + txtServername.Text + " With " + txtUsername.Text + " as username, and as password " + txtPassword.Text + '\r';
            }
            catch (Exception eX)
            {
                //Generate the error to an message box.
                richtextboxDebugger.Text = richtextboxDebugger.Text + "SQL" + '\r';
                richtextboxDebugger.Text = richtextboxDebugger.Text + eX.Message.ToString() + '\r';
                //Counts total of causes error.
                amountError = amountError + 1;
                lblError.Text = "Errors: " + amountError.ToString();
                //Closes the loading form.
                frmLoadingcall.Hide();
                //Shows the main form.
                this.Show();
                //Closes the connection.
                sqlCon.Close();
                //Changes the button text back to test.
                btnTestconnection.Text = "Test";
            }
            finally
            {
                //Closes the connection afterwards.
                if (connectionKeeper == 0)
                {
                    //Closes the connection.
                    sqlCon.Close();
                }
                //Maintaining the connection.
                if (connectionKeeper == 1)
                {
                    //Do nothing.
                }
            }
        }

        private void MySQLremoteConnection()
        {
            //Change the text of the loading form.
            frmLoadingcall.lblInfo.Text = "Connecting... Please wait";
            try
            {
                //Opens the connection.
                sqlConnection.Open();
                //Shows the form.
                this.Show();
                //Closes the loading form.
                frmLoadingcall.Hide();
                //This is for showing that the connection was done.
                toolStripStatusLabelStatus.Text = "Connected";
            }
            catch 
            {
                try
                {
                    //Opens the connection.
                    sqlConnection.Open();
                    //Shows the form.
                    this.Show();
                    //Closes the loading form.
                    frmLoadingcall.Hide();
                    //This is for showing that the connection was done.
                    toolStripStatusLabelStatus.Text = "Connected";
                }
                catch 
                {
                    try
                    {
                        sqlConnection.Open();
                        //Shows the form.
                        this.Show();
                        //Closes the loading form.
                        frmLoadingcall.Hide();
                        //This is for showing that the connection was done.
                        toolStripStatusLabelStatus.Text = "Connected";
                    }
                    catch (System.Exception ex)
                    {
                        //Shows an error message.
                        richtextboxDebugger.Text = ex.Message.ToString() + '\r';
                        //Hide the loading screen
                        frmLoadingcall.Hide();
                        //Closes the form.
                        //this.Close();
                    } 
                }
            }
        }

        private void ODBCstandardConnection()
        {
            //Shows the loading form.
            frmLoadingcall.Show();
            frmLoadingcall.Refresh();
            this.Hide();
            //Connection string
            odbcCon.ConnectionString =
                        "Driver={SQL Server};" +
                        "Server=" + txtServername.Text + ";" +
                        "DataBase=" + comboboxDatabase.Text + ";" +
                        "Uid=" + txtUsername.Text + ";" +
                        "Pwd=" + txtPassword.Text + ";";
            try
            {
                //Opens the connection.
                odbcCon.Open();
                //Closes the loading form.
                frmLoadingcall.Hide();
                //Shows the main form.
                this.Show();
                //Shows result to debugger.
                richtextboxDebugger.Text = richtextboxDebugger.Text + "ODBC" + '\r';
                richtextboxDebugger.Text = richtextboxDebugger.Text + "You have successful connected to " + comboboxDatabase.Text + " By using server name: " + txtServername.Text + " With " + txtUsername.Text + " as username, and as password " + txtPassword.Text + '\r';
            }
            catch (Exception eX)
            { 
                //Generate the error to an message box.
                richtextboxDebugger.Text = richtextboxDebugger.Text + "ODBC" + '\r';
                richtextboxDebugger.Text = richtextboxDebugger.Text + eX.Message.ToString() + '\r';
                //Closes the loading form.
                frmLoadingcall.Hide();
                //Shows the main form.
                this.Show();
                //Closes the connection.
                odbcCon.Close();
                //Changes the button text back to test.
                btnTestconnection.Text = "Test";
            }
           finally
            {
                //Closes the connection afterwards.
                if (connectionKeeper == 0)
                {
                    //Closes the connection.
                    odbcCon.Close();
                }
                //Maintaining the connection.
                if (connectionKeeper == 1)
                {
                    //Do nothing.
                }
            }
        }
        #endregion

        //Here you will find all check boxes.
        #region
        private void checkBoxOn_CheckedChanged(object sender, EventArgs e)
        {
            //Makes sure it checks on the right moment.
            if (checkBoxOn.Checked == true)
            {
                checkBoxOff.Checked = false;
            }
            else if (checkBoxOn.Checked == false)
            {
                checkBoxOff.Checked = true;
            }
            //Setting the value of the type of connection to 1.
            connectionKeeper = 1;
        }

        private void checkBoxOff_CheckedChanged(object sender, EventArgs e)
        {
            //Makes sure it checks on the right moment.
            if (checkBoxOff.Checked == true)
            {
                checkBoxOn.Checked = false;
            }
            else if (checkBoxOff.Checked == false)
            {
                checkBoxOn.Checked = true;
            }
            //Setting the value of the type of connection to 0.
            connectionKeeper = 0;
        }
        #endregion
      
        private void frmMain_Load(object sender, EventArgs e)
        {
            //This is for if the program should auto connect or not.
            try
            {
                int saveConnection = (int)keyOpen.GetValue("saveConnection");
                if (saveConnection == 1)
                {
                    int typeConnection = (int)keyOpen.GetValue("typeConnection");
                    //ODBC
                    if (typeConnection == 1)
                    {
                        string serverName = (string)keyOpenodbc.GetValue("serverName");
                        string database = (string)keyOpenodbc.GetValue("database");
                        string userName = (string)keyOpenodbc.GetValue("userName");
                        string passWord = (string)keyOpenodbc.GetValue("passWord");
                        odbcCon.ConnectionString =
                                      "Driver={SQL Server};" +
                                      "Server=" + serverName +
                                      "DataBase=" + database +
                                      "Uid=" + userName +
                                      "Pwd=" + passWord;
                        odbcCon.Open();
                    }

                }
            }
            catch
            {
            }
            //This is for gathing the date of the day...
            try
            {
                //Getting total run time.
                runTimeseconds = (int)keyOpen.GetValue("runTimeseconds");
                runTimeminuts = (int)keyOpen.GetValue("runTimeminutes");
                runTimehours = (int)keyOpen.GetValue("runTimehours");
                //Getting last run.
                toolStripStatusLabeldate.Text = (string)keyOpen.GetValue("lastRun");
            }
            catch
            { 
                //If can't find date, get the current date.
                toolStripStatusLabeldate.Text = System.DateTime.Now.ToLongDateString().ToString();
            }
            setMaxlength();
            //This is for clearing all tab pages.
            try
            {
                clearTabpages();
            }
            catch
            {
            }
            //This is for making an connection with the login/register function.
            toolStripStatusLabelStatus.Alignment = ToolStripItemAlignment.Right;
            //Hide the main form.
            this.Hide();
            //Shows the loading form.
            frmLoadingcall.Show();
            //Refresh the loading form.
            frmLoadingcall.Refresh();
            try
            { 
                //Connecting to domain.
                MySQLremoteConnection();
            }
            catch
            {
            	//Closing the loading form.
                frmLoadingcall.Hide();
            }
           
        }

        private void setMaxlength()
        {
            //Register section.
            txtRegisterusername.MaxLength = 32;
            txtRegisterpassword.MaxLength = 32;
            txtRegisteremail.MaxLength = 255;
            txtRegistersecuritycode.MaxLength = 4;
        }

        private void clearTabpages()
        {
            //This is the setting tab page!
            //This will make sure the right tab page get deleted.
            int tabSettings = (int)keyOpen.GetValue("setSettings");
            if (tabSettings == 1)
            {
                tabcontrolMain.TabPages.Remove(tabPage1);
            }
        }

        private void btnFirstnext_Click(object sender, EventArgs e)
        {
            //This is for setting the value in the registry.
            //This will make sure if you placed the first settings.
            #region
            keyCreate.SetValue("setSettings", 1);
            #endregion
            //Keep connection.
            #region
            if (checkBoxOn.Checked == true)
            {
                keyCreate.SetValue("keepConnected", 1);
                connectionKeeper = 1;
            }
            else if (checkBoxOff.Checked == true)
            {
                keyCreate.SetValue("keepConnected", 0);
                connectionKeeper = 0;
            }
            #endregion
            //Save connection.
            #region
            if (checkBoxOn1.Checked == true)
            {
                keyCreate.SetValue("saveConnection", 1);
                saveConnection = 1;
                //ODBC
                if (typeConnection == 1)
                {
                    //This is our storage.
                    keyCreateodbc.SetValue("serverName", txtServername.Text + ";");
                    keyCreateodbc.SetValue("database", comboboxDatabase.Text + ";");
                    keyCreateodbc.SetValue("userName", txtUsername.Text + ";");
                    keyCreateodbc.SetValue("passWord", txtPassword.Text + ";");
                }
                //SQL
                else if (typeConnection == 2)
                {
                    //This is our storage.
                    keyCreatesql.SetValue("serverName", txtServername.Text + ";");
                    keyCreatesql.SetValue("database", comboboxDatabase.Text + ";");
                    keyCreatesql.SetValue("userName", txtUsername.Text + ";");
                    keyCreatesql.SetValue("passWord", txtPassword.Text + ";");
                }
            }
            else if (checkBoxOff1.Checked == true)
            {
                keyCreate.SetValue("saveConnection", 0);
                saveConnection = 0;
            }
            #endregion
            //Type connection.
            #region
            if (radioButtonODBC.Checked == true)
            {
                keyCreate.SetValue("typeConnection", 1);
                typeConnection = 1;
            }
            else if (radioButtonSQL.Checked == true)
            {
                keyCreate.SetValue("typeConnection", 2);
                typeConnection = 2;
            }
            #endregion
        }

        private void btnFirstcancel_Click(object sender, EventArgs e)
        {
            //Closes the form. 
            this.Close();
        }

        private void checkBoxOn1_CheckedChanged(object sender, EventArgs e)
        {
            //Makes sure it checks on the right moment.
            if (checkBoxOn1.Checked == true)
            {
                checkBoxOff1.Checked = false;
            }
            else if (checkBoxOn1.Checked == false)
            {
                checkBoxOff1.Checked = true;
            }
        }

        private void checkBoxOff1_CheckedChanged(object sender, EventArgs e)
        {
            //Makes sure it checks on the right moment.
            if (checkBoxOff1.Checked == true)
            {
                checkBoxOn1.Checked = false;
            }
            else if (checkBoxOff1.Checked == false)
            {
                checkBoxOn1.Checked = true;
            }
        }

        private void radioButtonODBC_CheckedChanged(object sender, EventArgs e)
        {
            //Sets connection to ODBC
            typeConnection = 1;
            //When enter different state change text back to get db.
            btnTestconnection.Text = "Get DB";
            //Clears all items in combobox, and other details.
            comboboxDatabase.Items.Clear();
            txtServername.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        private void radioButtonSQL_CheckedChanged(object sender, EventArgs e)
        {
            //Sets connection to SQL
            typeConnection = 2;
            //When enter different state change text back to get db.
            btnTestconnection.Text = "Get DB";
            //Clears all items in combobox, and other details.
            comboboxDatabase.Items.Clear();
            txtServername.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        private void btnTestconnection_Click(object sender, EventArgs e)
        {
            //If button text is "Get DB", after this user will be able to test this connection.
            if (btnTestconnection.Text == "Get DB")
            {
                //Makes sure it saves typeConnection.
                if (radioButtonODBC.Checked == true)
                {
                    //Sets connection to odbc.
                    typeConnection = 1;
                }
                else if (radioButtonODBC.Checked == false)
                {
                    //Sets connection to sql.
                    typeConnection = 2;
                }
                if (typeConnection == 1)
                {
                    //Enables the combobox.
                    comboboxDatabase.Enabled = true;
                    //Clears all items.
                    comboboxDatabase.Items.Clear();
                    //Connection string.
                    String odbcString = "Driver={SQL Server};" +
                                        "Server=(local);" +
                                        "Uid=;" +
                                        "Pwd=;";
                    //Declare query.
                    string Query = "SELECT NAME from sysdatabases";
                    //Declare new odbcConnection.
                    OdbcConnection conOdbc = new OdbcConnection(odbcString);
                    //Declare new odbcCommand.
                    OdbcCommand odbcCommand = new OdbcCommand(Query, conOdbc);
                    //Opens the connection.
                    conOdbc.Open();
                    //Changes the button text to "Test".
                    btnTestconnection.Text = "Test";
                    OdbcDataReader odbcReader = odbcCommand.ExecuteReader();
                    try
                    {
                        while (odbcReader.Read())
                        {
                            //Adds every database to the combobox.
                            comboboxDatabase.Items.Add(odbcReader.GetString(0));
                        }
                    }
                    catch (Exception eX)
                    {
                        //Changes the button text back to "Get DB".
                        btnTestconnection.Text = "Get DB";
                        //Shows an error message if something goes wrong.
                        richtextboxDebugger.Text = eX.ToString();
                        //Disables comboBox.
                        comboboxDatabase.Enabled = false;
                    }
                    finally
                    {
                        // always call Close when done reading.
                        odbcReader.Close();
                        // always call Close when done reading.
                        conOdbc.Close();
                        //Select the first database.
                        comboboxDatabase.SelectedIndex = 0;
                    }
                }
                else if (typeConnection == 2)
                {
                    comboboxDatabase.Enabled = true;
                    comboboxDatabase.Items.Clear();
                    //Connection string
                    String conString =
                    "Data Source=" + "(local)" + ";" +
                    "Integrated Security=True;";
                    try
                    {
                        using (SqlConnection sqlCon = new SqlConnection(conString))
                        {
                            //Opens the connection.
                            sqlCon.Open();
                            //Declares data table.
                            DataTable tblDatabases = sqlCon.GetSchema("Databases");
                            //Closes connection
                            sqlCon.Close();
                            //Changes the button text to "Test".
                            btnTestconnection.Text = "Test";
                            //Get data to the combobox.
                            foreach (DataRow row in tblDatabases.Rows)
                            {
                                comboboxDatabase.Items.Add(row["database_name"]);
                            }
                        }
                        //Select the first database.
                        comboboxDatabase.SelectedIndex = 0;
                    }
                    catch (Exception eX)
                    {
                        //Changes the button text back to "Get DB".
                        btnTestconnection.Text = "Get DB";
                        //Shows error in message box.
                        richtextboxDebugger.Text = eX.ToString();
                        //Enables the combobox.
                        comboboxDatabase.Enabled = false;
                    }
                }
            }
            else if (btnTestconnection.Text == "Test")
            {
                //Checks if it is set to ODBC.
                if (typeConnection == 1)
                {
                    //The try/catch funtion prevent it from crashing.
                    try
                    {
                        //Change the value of the button.
                        btnTestconnection.Text = "Use";
                        //Using an standard ODBC connection.
                        ODBCstandardConnection();
                        
                    }
                    catch (Exception eX)
                    {
                        //Shows error to debugger.
                        richtextboxDebugger.Text = eX.ToString();
                    }
                }
                //Checks if it is set to SQL.
                else if (typeConnection == 2)
                {
                    //The try/catch funtion prevent it from crashing.
                    try
                    {
                        //Change the value of the button.
                        btnTestconnection.Text = "Use";
                        //Using an standard SQL connection.
                        SQLstandardConnection();                        
                    }
                    catch (Exception eX)
                    {
                        //Show error to debugger.
                        richtextboxDebugger.Text = eX.ToString();
                    }
                }
            }
        }

        private void checkboxManually_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxManually.Checked == true)
            {
                //Changes the style of the combobox.
                comboboxDatabase.DropDownStyle = ComboBoxStyle.DropDown;
                //Enables the combobox.
                comboboxDatabase.Enabled = true;
                //This will make sure you wont be able to receive any databases.
                btnTestconnection.Text = "Test";
            }
            else if (checkboxManually.Checked == false)
            {
                //Changes the style of the combobox.
                comboboxDatabase.DropDownStyle = ComboBoxStyle.DropDownList;
                //Disables the combobox.
                comboboxDatabase.Enabled = false;
                //This will make sure you will be able to receive databases.
                btnTestconnection.Text = "Get DB";
            }
        }

        private void radiobuttonCustom_CheckedChanged(object sender, EventArgs e)
        {
            //Enables all check boxes.
            checkBoxOn.Enabled = true;
            checkBoxOn1.Enabled = true;
            checkBoxOff.Enabled = true;
            checkBoxOff1.Enabled = true;
        }

        private void radiobuttonDefault_CheckedChanged(object sender, EventArgs e)
        {
            //Checks the right settings.
            checkBoxOn.Checked = true;
            checkBoxOn1.Checked = true;
            //Disables all check boxes.
            checkBoxOn.Enabled = false;
            checkBoxOn1.Enabled = false;
            checkBoxOff.Enabled = false;
            checkBoxOff1.Enabled = false;
            //Setting the right settings.
            connectionKeeper = 1;
            saveConnection = 1;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            btnRegister.Enabled = true;
            btnRecover.Enabled = true;
            //This is for setting it to the right position.
            gbLogin.Left = 209;
            //Makes sure it gets on the right place.
            if (gbRegister.Left == 209)
            {
                gbRegister.Left = 415;
            }
            else if (gbRecover.Left == 209)
            {
                gbRecover.Left = 3;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            btnRegister.Enabled = false;
            btnRecover.Enabled = true;
            btnLogin.Enabled = true;
            //This is for setting it to the right position.
            gbRegister.Left = 209;
            if (gbRecover.Left == 209)
            {
                gbRecover.Left = 3;
            }
        }

        private void btnRecover_Click(object sender, EventArgs e)
        {
            btnRecover.Enabled = false;
            btnRegister.Enabled = true;
            btnLogin.Enabled = true;
            //This is for setting it to the right position.
            gbRecover.Left = 209;
            if (gbRegister.Left == 209)
            {
                gbRegister.Left = 415;
            }
        }

        private void btnRegisterinsert_Click(object sender, EventArgs e)
        {
            if (pictureboxRegistercheck1.Visible == true)
            {
            }
            else
            {
                return;
            }
            if (pictureboxRegistercheck2.Visible == true)
            {
            }
            else
            {
                return;
            }
            if (pictureboxRegistercheck3.Visible == true)
            {
            }
            else
            {
                return;
            }
            try
            {
                //Declaring the query.
                MySqlCommand cmdExecute = new MySqlCommand ("INSERT INTO TB_USER VALUES ('','" + txtRegisterusername.Text + "','" + txtRegisterpassword.Text + "','" + txtRegisteremail.Text + "','" + txtRegistersecuritycode.Text + "')",sqlConnection);
                //Execute the query.
                cmdExecute.ExecuteNonQuery();
                //Gives you notice that is has been successfully.
                MessageBox.Show("Your account has been registered!");
            }
            catch(System.Exception eX)
            {
                sqlConnection.Open();
                MessageBox.Show(eX.Message);                
            }
        }

        private void txtRegisterusername_TextChanged(object sender, EventArgs e)
        {
            if (txtRegisterusername.TextLength >= 4)
            {
                pictureboxRegistercheck1.Visible = true;
            }
            else if (txtRegisterusername.TextLength < 4)
            {
                pictureboxRegistercheck1.Visible = false;
            }
        }

        private void txtRegisterpassword_TextChanged(object sender, EventArgs e)
        {
            if (txtRegisterpassword.TextLength >= 4)
            {
                pictureboxRegistercheck2.Visible = true;
            }
            else if (txtRegisterpassword.TextLength < 4)
            {
                pictureboxRegistercheck2.Visible = false;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Total amount of errors made.
            keyCreate.SetValue("totalErrors", amountError);
            //Total running time.
            keyCreate.SetValue("runTimehours", runTimehours);
            keyCreate.SetValue("runTimeminuts", runTimeminuts);
            keyCreate.SetValue("runTimeseconds", runTimeseconds);
            //Last time used.
            keyCreate.SetValue("lastRun", toolStripStatusLabeldate.Text);
        }

        private void tmrRuntime_Tick(object sender, EventArgs e)
        {
            runTimeseconds = runTimeseconds + 1;
            //Makes sure it resets the seconds.
            if (runTimeseconds == 60)
            {
                //Reset int.
                runTimeseconds = 0;
                //Add 1 minute.
                runTimeminuts = runTimeminuts + 1;
            }
            if (runTimeminuts == 60)
            {
                //Reset int.
                runTimeminuts = 0;
                //Add 1 hour.
                runTimehours = runTimehours + 1;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Closes the program.
            this.Close();
        }
        //Here will all things be stored what has to do with the drop editor.

        private void lbItemname_SelectedIndexChanged(object sender, EventArgs e)
        {
            //This is our own litle system, i am using it to get the right ID. 
            //Works perfect till now.
            int i = 0;
            while (lbItemname.SelectedItem != lbItemname.Items[i])
                i++;
            lbItemid.SelectedIndex = i;
        }

        private void btnItemsearch_Click(object sender, EventArgs e)
        { 
            try
            {
                typeConnection = (int)keyOpen.GetValue("typeConnection");
                if (typeConnection == 1)
                {
                    if (txtDropeditoritemsearch.TextLength < 1)
                    {
                        //Gives notice if less then 1 chars.
                        richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + "Please search again, but this time use 1 or more letters! this is being blocked out to improve performance." + '\r' ;
                        return;
                    }
                    //We want to make sure that we clean the lists everytime we are searching.
                    lbItemname.Items.Clear();
                    lbItemid.Items.Clear();
                    //We also need the QUERY's for receiving item NAMES and ID's.
                    String itemName = "SELECT strName FROM ITEM WHERE strName like" + "'" + txtDropeditoritemsearch.Text + "%';";
                    String itemId = "SELECT Num FROM ITEM WHERE strName like" + "'" + txtDropeditoritemsearch.Text + "%';";
                    //Here we are binding the odbc command to the connection.
                    OdbcCommand cmdItemname = new OdbcCommand(itemName, odbcCon);
                    OdbcCommand cmdItemid = new OdbcCommand(itemId, odbcCon);
                    try
                    {
                        cmdItemname.ExecuteNonQuery();
                        using (OdbcDataReader readerItemname = cmdItemname.ExecuteReader())
                        {
                            while (readerItemname.Read())
                            {
                                string sqlItemname = readerItemname.GetString(0);
                                lbItemname.Items.Add(sqlItemname);
                            }
                        }
                        cmdItemid.ExecuteNonQuery();
                        using (OdbcDataReader readerItemid = cmdItemid.ExecuteReader())
                        {
                            while (readerItemid.Read())
                            {
                                string sqlItemid = readerItemid.GetString(0);
                                lbItemid.Items.Add(sqlItemid);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch
            {
            }
        }

        private void btnItemadd1_Click(object sender, EventArgs e)
        {
            //Get id to textbox.
            txtItem1.Text = lbItemid.SelectedItem.ToString();
        }

        private void btnItemadd2_Click(object sender, EventArgs e)
        {
            //Get id to textbox.
            txtItem2.Text = lbItemid.SelectedItem.ToString();
        }

        private void btnItemadd3_Click(object sender, EventArgs e)
        {
            //Get id to textbox.
            txtItem3.Text = lbItemid.SelectedItem.ToString();
        }

        private void btnItemadd4_Click(object sender, EventArgs e)
        {
            //Get id to textbox.
            txtItem4.Text = lbItemid.SelectedItem.ToString();
        }

        private void btnItemadd5_Click(object sender, EventArgs e)
        {
            //Get id to textbox.
            txtItem5.Text = lbItemid.SelectedItem.ToString();
        }

        private void lbMonstername_SelectedIndexChanged(object sender, EventArgs e)
        {
            //This is our own litle system, i am using it to get the right ID. 
            //Works perfect till now.
            int i = 0;
            while (lbMonstername.SelectedItem != lbMonstername.Items[i])
                i++;
            lbMonsterid.SelectedIndex = i;
            //Shows the monster name in the label.
            lblMonstername.Text = lbMonstername.SelectedItem.ToString();
        }

        private void btnMonstersearch_Click(object sender, EventArgs e)
        {
            try
            {
                typeConnection = (int)keyOpen.GetValue("typeConnection");
                if (typeConnection == 1)
                {
                    lbMonstername.Items.Clear();
                    lbMonsterid.Items.Clear();

                    String monsterName = "SELECT strName FROM K_MONSTER WHERE strName like" + "'" + txtDropeditormonstersearch.Text + "%';";
                    String monsterId = "SELECT sSid FROM K_MONSTER WHERE strName like" + "'" + txtDropeditormonstersearch.Text + "%';";

                    OdbcCommand cmdMonstername = new OdbcCommand(monsterName, odbcCon);
                    OdbcCommand cmdMonsterId = new OdbcCommand(monsterId, odbcCon);
                    try
                    {
                        cmdMonstername.ExecuteNonQuery();
                        using (OdbcDataReader readerMonstername = cmdMonstername.ExecuteReader())
                        {
                            while (readerMonstername.Read())
                            {
                                string sqlMonstername = readerMonstername.GetString(0);
                                lbMonstername.Items.Add(sqlMonstername);
                            }
                        }
                        cmdMonsterId.ExecuteNonQuery();
                        using (OdbcDataReader readerMonsterid = cmdMonsterId.ExecuteReader())
                        {
                            while (readerMonsterid.Read())
                            {
                                string sqlMonsterid = readerMonsterid.GetString(0);
                                lbMonsterid.Items.Add(sqlMonsterid);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        richTextBoxdropdebugger.Text = ex.Message.ToString();
                    }
                }
            }
            catch
            {
            }
        }

        private void lbMonsterid_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get the right ID of the monster.
            txtSsid.Text = lbMonsterid.SelectedItem.ToString();
            blInsert = false;
            txtItem1.Text = "";
            txtItem2.Text = "";
            txtItem3.Text = "";
            txtItem4.Text = "";
            txtItem5.Text = "";
            txtRate1.Text = "";
            txtRate2.Text = "";
            txtRate3.Text = "";
            txtRate4.Text = "";
            txtRate5.Text = "";
            txtNoah.Text = "";

            //Here you will find the new QUERY'S!
            #region
            String NOAH = "SELECT iMoney FROM K_MONSTER WHERE sSid = " + "'" + txtSsid.Text + "';";
            String ITEM1 = "SELECT iItem01 FROM K_MONSTER_ITEM WHERE sIndex = " + "'" + txtSsid.Text + "';";
            String ITEM2 = "SELECT iItem02 FROM K_MONSTER_ITEM WHERE sIndex = " + "'" + txtSsid.Text + "';";
            String ITEM3 = "SELECT iItem03 FROM K_MONSTER_ITEM WHERE sIndex = " + "'" + txtSsid.Text + "';";
            String ITEM4 = "SELECT iItem04 FROM K_MONSTER_ITEM WHERE sIndex = " + "'" + txtSsid.Text + "';";
            String ITEM5 = "SELECT iItem05 FROM K_MONSTER_ITEM WHERE sIndex = " + "'" + txtSsid.Text + "';";
            String RATE1 = "SELECT sPersent01 FROM K_MONSTER_ITEM WHERE sIndex = " + "'" + txtSsid.Text + "';";
            String RATE2 = "SELECT sPersent02 FROM K_MONSTER_ITEM WHERE sIndex = " + "'" + txtSsid.Text + "';";
            String RATE3 = "SELECT sPersent03 FROM K_MONSTER_ITEM WHERE sIndex = " + "'" + txtSsid.Text + "';";
            String RATE4 = "SELECT sPersent04 FROM K_MONSTER_ITEM WHERE sIndex = " + "'" + txtSsid.Text + "';";
            String RATE5 = "SELECT sPersent05 FROM K_MONSTER_ITEM WHERE sIndex = " + "'" + txtSsid.Text + "';";
            #endregion

            //Here you will find the new ODBCCOMMANDS!
            #region
            OdbcCommand cmdNoah = new OdbcCommand(NOAH, odbcCon);
            OdbcCommand cmdItem1 = new OdbcCommand(ITEM1, odbcCon);
            OdbcCommand cmdItem2 = new OdbcCommand(ITEM2, odbcCon);
            OdbcCommand cmdItem3 = new OdbcCommand(ITEM3, odbcCon);
            OdbcCommand cmdItem4 = new OdbcCommand(ITEM4, odbcCon);
            OdbcCommand cmdItem5 = new OdbcCommand(ITEM5, odbcCon);
            OdbcCommand cmdRate1 = new OdbcCommand(RATE1, odbcCon);
            OdbcCommand cmdRate2 = new OdbcCommand(RATE2, odbcCon);
            OdbcCommand cmdRate3 = new OdbcCommand(RATE3, odbcCon);
            OdbcCommand cmdRate4 = new OdbcCommand(RATE4, odbcCon);
            OdbcCommand cmdRate5 = new OdbcCommand(RATE5, odbcCon);
            #endregion

            //This is all for getting drops and rate's to the given textboxes!
            #region
            //For noah!
            #region
            try
            {

                cmdNoah.ExecuteNonQuery();
                using (OdbcDataReader readerNoah = cmdNoah.ExecuteReader())
                {
                    while (readerNoah.Read())
                    {
                        string sqlNoah = readerNoah.GetString(0);
                        txtNoah.Text = sqlNoah;
                    }
                }
            }
            catch (Exception ex)
            {

                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            //For item 1!
            #region
            try
            {

                cmdItem1.ExecuteNonQuery();
                using (OdbcDataReader readerItem1 = cmdItem1.ExecuteReader())
                {
                    while (readerItem1.Read())
                    {
                        string sqlItem1 = readerItem1.GetString(0);
                        txtItem1.Text = sqlItem1;
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            //For item 2!
            #region
            try
            {

                cmdItem2.ExecuteNonQuery();
                using (OdbcDataReader readerItem2 = cmdItem2.ExecuteReader())
                {
                    while (readerItem2.Read())
                    {
                        string sqlItem2 = readerItem2.GetString(0);
                        txtItem2.Text = sqlItem2;
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            //For item 3!
            #region
            try
            {

                cmdItem3.ExecuteNonQuery();
                using (OdbcDataReader readerItem3 = cmdItem3.ExecuteReader())
                {
                    while (readerItem3.Read())
                    {
                        string sqlItem3 = readerItem3.GetString(0);
                        txtItem3.Text = sqlItem3;
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            //For item 4!
            #region
            try
            {

                cmdItem4.ExecuteNonQuery();
                using (OdbcDataReader readerItem4 = cmdItem4.ExecuteReader())
                {
                    while (readerItem4.Read())
                    {
                        string sqlItem4 = readerItem4.GetString(0);
                        txtItem4.Text = sqlItem4;
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            //For item 5!
            #region
            try
            {
                cmdItem5.ExecuteNonQuery();
                using (OdbcDataReader readerItem5 = cmdItem5.ExecuteReader())
                {
                    while (readerItem5.Read())
                    {
                        string sqlItem5 = readerItem5.GetString(0);
                        txtItem5.Text = sqlItem5;
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            //For rate 1!
            #region
            try
            {

                cmdRate1.ExecuteNonQuery();
                using (OdbcDataReader readerRate1 = cmdRate1.ExecuteReader())
                {
                    while (readerRate1.Read())
                    {
                        string sqlRate1 = readerRate1.GetString(0);
                        txtRate1.Text = sqlRate1;
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            //For rate 2!
            #region
            try
            {

                cmdRate2.ExecuteNonQuery();
                using (OdbcDataReader readerRate2 = cmdRate2.ExecuteReader())
                {
                    while (readerRate2.Read())
                    {
                        string sqlRate2 = readerRate2.GetString(0);
                        txtRate2.Text = sqlRate2;
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            //For rate 3!
            #region
            try
            {

                cmdRate3.ExecuteNonQuery();
                using (OdbcDataReader readerRate3 = cmdRate3.ExecuteReader())
                {
                    while (readerRate3.Read())
                    {
                        string sqlRate3 = readerRate3.GetString(0);
                        txtRate3.Text = sqlRate3;
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            //For rate 4!
            #region
            try
            {
                cmdRate4.ExecuteNonQuery();
                using (OdbcDataReader readerRate4 = cmdRate4.ExecuteReader())
                {
                    while (readerRate4.Read())
                    {
                        string sqlRate4 = readerRate4.GetString(0);
                        txtRate4.Text = sqlRate4;
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            //For rate 5!
            #region
            try
            {

                cmdRate5.ExecuteNonQuery();
                using (OdbcDataReader readerRate5 = cmdRate5.ExecuteReader())
                {
                    while (readerRate5.Read())
                    {
                        string sqlRate5 = readerRate5.GetString(0);
                        txtRate5.Text = sqlRate5;
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
            #endregion
            #endregion
            //The most important part is here... we will have to check if we need an "UPDATE" or an "INSERT"
            #region
            if (txtItem1.Text == "")
            {
                txtItem1.Text = "0";
                blInsert = true;
            }
            if (txtItem2.Text == "")
            {
                txtItem2.Text = "0";
                blInsert = true;
            }
            if (txtItem3.Text == "")
            {
                txtItem3.Text = "0";
                blInsert = true;
            }
            if (txtItem4.Text == "")
            {
                txtItem4.Text = "0";
                blInsert = true;
            }
            if (txtItem5.Text == "")
            {
                txtItem5.Text = "0";
                blInsert = true;
            }
            if (txtRate1.Text == "")
            {
                txtRate1.Text = "0";
                blInsert = true;
            }
            if (txtRate2.Text == "")
            {
                txtRate2.Text = "0";
                blInsert = true;
            }
            if (txtRate3.Text == "")
            {
                txtRate3.Text = "0";
                blInsert = true;
            }
            if (txtRate4.Text == "")
            {
                txtRate4.Text = "0";
                blInsert = true;
            }
            if (txtRate5.Text == "")
            {
                txtRate5.Text = "0";
                blInsert = true;
            }
            #endregion
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //If insertOrupdate = 0, we will do an update.
            if (blInsert == false)
            {
                //We will using only 1 string this will look neat, and improves performance.
                String NOAH = "UPDATE K_MONSTER SET iMoney =" + "'" + txtNoah.Text + "'" + "WHERE sSid =" + "'" + txtSsid.Text + "';";
                String completeUpdate = "UPDATE K_MONSTER_ITEM SET iItem01 ='" + txtItem1.Text + "',iItem02 ='" + txtItem2.Text + "',iItem03 ='" + txtItem3.Text + "',iItem04 ='" + txtItem4.Text + "',iItem05 ='" + txtItem5.Text + "',sPersent01 ='" + txtRate1.Text + "',sPersent02 ='" + txtRate2.Text + "',sPersent03 ='" + txtRate3.Text + "',sPersent04 ='" + txtRate4.Text + "',sPersent05 ='" + txtRate5.Text + "'" + "WHERE sIndex =" + "'" + txtSsid.Text + "';";
               
                OdbcCommand cmdNoah = new OdbcCommand(NOAH, odbcCon);
                OdbcCommand cmdCompleteupdate = new OdbcCommand(completeUpdate, odbcCon);
               
                try
                {
                    cmdNoah.ExecuteNonQuery();
                    cmdCompleteupdate.ExecuteNonQuery();
                   
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + '\r' + "Drop has been sucessfull imported";
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Monstername:  " + lbMonstername.SelectedItem.ToString() + '\t' + " Monster ID:  " + lbMonsterid.SelectedItem.ToString();
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Item ID 1:  " + txtItem1.Text + '\t' + '\t' + '\t' + " Item RATE 1:  " + txtRate1.Text;
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Item ID 1:  " + txtItem2.Text + '\t' + '\t' + '\t' + " Item RATE 2:  " + txtRate2.Text;
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Item ID 1:  " + txtItem3.Text + '\t' + '\t' + " Item RATE 3:  " + txtRate3.Text;
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Item ID 1:  " + txtItem4.Text + '\t' + '\t' + " Item RATE 4:  " + txtRate4.Text;
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Item ID 1:  " + txtItem5.Text + '\t' + '\t' + " Item RATE 5:  " + txtRate5.Text;
                }
                catch (Exception Ex)
                {
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + Ex.Message;
                }
            }
            //If insertOrupdate = 1, we will do an Insert this means i also will have to do an delete!
            if (blInsert == true)
            {
                //Lets write some data to int!
                //ITEMS
                string strItem1 = txtItem1.Text;
                item1 = int.Parse(strItem1);
                string strItem2 = txtItem2.Text;
                item2 = int.Parse(strItem2);
                string strItem3 = txtItem3.Text;
                item3 = int.Parse(strItem3);
                string strItem4 = txtItem4.Text;
                item4 = int.Parse(strItem4);
                string strItem5 = txtItem5.Text;
                item5 = int.Parse(strItem5);
                //RATES
                string strRate1 = txtRate1.Text;
                rate1 = int.Parse(strRate1);
                string strRate2 = txtRate2.Text;
                rate2 = int.Parse(strRate2);
                string strRate3 = txtRate3.Text;
                rate3 = int.Parse(strRate3);
                string strRate4 = txtRate4.Text;
                rate4 = int.Parse(strRate4);
                string strRate5 = txtRate5.Text;
                rate5 = int.Parse(strRate5);
                //First we wanna delete our monster, so we can sucesfull import the new drops.
                String deleteMonster = "DELETE FROM K_MONSTER_ITEM WHERE sIndex =" + "'" + txtSsid.Text + "';";
                //Here we will update the monster amount of noah, simply because the monster can always be updated in this chase.
                String NOAH = "UPDATE K_MONSTER SET iMoney =" + "'" + txtNoah.Text + "'" + "WHERE sSid =" + "'" + txtSsid.Text + "';";

                String insertMonster = "INSERT INTO K_MONSTER_ITEM ( [sIndex], [iItem01], [sPersent01], [iItem02] , [sPersent02] , [iItem03] , [sPersent03] , [iItem04] , [sPersent04] , [iItem05] , [sPersent05] ) VALUES ( " + txtSsid.Text + "," + item1 + "," + rate1 + "," + item2 + "," + rate2 + "," + item3 + "," + rate3 + "," + item4 + "," + rate4 + "," + item5 + "," + rate5 + ");";
                OdbcCommand cmdNoah = new OdbcCommand(NOAH, odbcCon);
                OdbcCommand cmdDeletemonster = new OdbcCommand(deleteMonster, odbcCon);
                OdbcCommand cmdInsertmonster = new OdbcCommand(insertMonster, odbcCon);

                try
                {
                    cmdDeletemonster.ExecuteNonQuery();
                    cmdInsertmonster.ExecuteNonQuery();
                    cmdNoah.ExecuteNonQuery();
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + '\r' + "Drop has been sucessfull imported";
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Monstername:  " + lbMonstername.SelectedItem.ToString() + '\t' + " Monster ID:  " + lbMonsterid.SelectedItem.ToString();
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Item ID 1:  " + txtItem1.Text + '\t' + '\t' + '\t' + " Item RATE 1:  " + txtRate1.Text;
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Item ID 1:  " + txtItem2.Text + '\t' + '\t' + '\t' + " Item RATE 2:  " + txtRate2.Text;
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Item ID 1:  " + txtItem3.Text + '\t' + '\t' + " Item RATE 3:  " + txtRate3.Text;
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Item ID 1:  " + txtItem4.Text + '\t' + '\t' + " Item RATE 4:  " + txtRate4.Text;
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + "Item ID 1:  " + txtItem5.Text + '\t' + '\t' + " Item RATE 5:  " + txtRate5.Text;
                }
                catch (Exception Ex)
                {
                    richTextBoxdropdebugger.Text = richTextBoxdropdebugger.Text + '\r' + Ex.Message;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show (blInsert.ToString());
        }

        private void dropEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Makes the drop editor visible.
            panelDropeditor.Visible = true;
            //Get the groupitems.
            lbGroupitems.Items.Clear();
            String groupItems = "SELECT iItemGroupNum FROM MAKE_ITEM_GROUP WHERE iItemGroupNum like" + "'" + "" + "%';";
            OdbcCommand cmdGroupitems = new OdbcCommand(groupItems, odbcCon);
            try
            {
                cmdGroupitems.ExecuteNonQuery();
                using (OdbcDataReader readerGroupitems = cmdGroupitems.ExecuteReader())
                {
                    while (readerGroupitems.Read())
                    {
                        string sqlGroupitems = readerGroupitems.GetString(0);
                        lbGroupitems.Items.Add(sqlGroupitems);
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBoxdropdebugger.Text = ex.Message;
            }
        }
    }
}
