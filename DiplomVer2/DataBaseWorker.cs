using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DiplomVer2
{
    class DataBaseWorker
    {
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dan\source\repos\DiplomVer2\DiplomVer2\Database.mdf;Integrated Security=True";

        private static SqlConnection sqlConnection = null;

        private static SqlDataAdapter dataAdapter = null;

        private static DataSet dataSet = null;

        private static DataTable table = null;

        public static CalculateEMS calculate;

        public static void ConnectionDataBase(string connection)
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + @connection + ";Integrated Security=True";
        }

        public static async void OpenConnection()
        {
            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync();
        }

        public static void CloseConnection()
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

        public static List<string[]> ExecuteQuery(string query, int col)
        {
            SqlCommand command = new SqlCommand(query, sqlConnection);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> response = new List<string[]>();

            while (reader.Read())
            {
                response.Add(new string[col]);

                for (int i = 0; i < col; i++)
                {
                    response[response.Count - 1][i] = reader[i].ToString();
                }
            }
            reader.Close();

            if (response.Count != 0)
                return response;
            else
                return null;

        }

        public static string ExecuteQuery(string query)
        {
            SqlCommand command = new SqlCommand(query, sqlConnection);

            SqlDataReader reader = command.ExecuteReader();

            string response = null;

            while (reader.Read())
            {
                response = reader[0].ToString();
            }

            reader.Close();

            return response;
        }

        public static void ExecuteQueryWithoutResponse(string ColumName, string text, string ProjectName)
        {
            

            sqlConnection.Open();

            string query = string.Format("UPDATE [ValueEMS] SET {0}={1} WHERE [ProjectName]='{2}'", ColumName, text, ProjectName);

            SqlCommand command = new SqlCommand(query, sqlConnection);

            command.Parameters.AddWithValue(ColumName, text);

            command.ExecuteNonQuery();

            sqlConnection.Close();
        }

        #region listBox

        public static ListBox ListBoxItemsAdd(ListBox listBox1)
        {
            sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();

            SqlDataReader sqlReader = null;

            SqlCommand command = new SqlCommand("SELECT * FROM [TableProject]", sqlConnection);

            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {

                    listBox1.Items.Add(
                                       Convert.ToString(sqlReader["ProjectName"])
                                       );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }

            sqlConnection.Close();

            return listBox1;
        }

        public static ListBox ListBoxRefresh(ListBox listBox1)
        {
            sqlConnection.Open();

            listBox1.Items.Clear();

            SqlDataReader sqlReader = null;

            SqlCommand command = new SqlCommand("SELECT * FROM [TableProject]", sqlConnection);

            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    listBox1.Items.Add(
                                      Convert.ToString(sqlReader["ProjectName"])
                                      );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }

            sqlConnection.Close();

            return listBox1;
        }

        #endregion

        #region buttons

        public static void ButtonDelete(object value)
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("DELETE FROM [TableProject] WHERE [ProjectName]=@ProjectName", sqlConnection);
            SqlCommand command1 = new SqlCommand("DELETE FROM [ValueEMS] WHERE [ProjectName]=@ProjectName", sqlConnection);

            command.Parameters.AddWithValue("ProjectName", value);
            command1.Parameters.AddWithValue("ProjectName", value);
       

            if (value != null)
            {
                DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить?", "Удаление проекта", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    command.ExecuteNonQuery();
                    command1.ExecuteNonQuery();
                }
            }
            else
                MessageBox.Show("Выберете проект для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);


            sqlConnection.Close();
        }

        public static void ButtonAdd(string text)
        {
            sqlConnection.Open();

            string query = "INSERT INTO [TableProject] (ProjectName)VALUES(@ProjectName)";

            SqlCommand command = new SqlCommand(query, sqlConnection);

            command.Parameters.AddWithValue("ProjectName", text);

            command.ExecuteNonQuery();

            query = "INSERT INTO[ValueEMS](ProjectName)VALUES(@ProjectName)";

            SqlCommand command1 = new SqlCommand(query, sqlConnection);

            command1.Parameters.AddWithValue("ProjectName", text);

            command1.ExecuteNonQuery();

            sqlConnection.Close();
        }

        public static string GetValueFromDB(string ColParamName, string ProjectName)
        {
            sqlConnection.Open();

            SqlDataReader sqlReader = null;

            string query = string.Format("SELECT * FROM [{0}] WHERE ProjectName = '{1}'", "ValueEMS", ProjectName);

            SqlCommand command = new SqlCommand(query, sqlConnection);

            string value = "";

            try
            {
                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    value = Convert.ToString(sqlReader[ColParamName]);
                                     
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }

            sqlConnection.Close();
            
            return value.Replace(",", "."); 
        }



        //public static void SetValueFromDB(string ColParamName, string text ,string ProjectName)
        //{
        //sqlConnection.Open();

        //    string query = string.Format("INSERT INTO [ValueEMS] ({0})VALUES(@{1}) WHERE  ProjectName = '{2}'", ColParamName, ColParamName, ProjectName);

        //SqlCommand command = new SqlCommand(query, sqlConnection);

        //command.Parameters.AddWithValue(ColParamName, text);


        //    command.ExecuteNonQuery();

        //    sqlConnection.Close();
        //}

        public static void ButtonChange(DataGridView value)
        {
            //sqlConnection.Open();

            //SqlCommand command = new SqlCommand("", sqlConnection);

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM ValueEMS", sqlConnection);

            DataTable table = new DataTable();

            table.Clear();

            adapter.Fill(table);

            value.DataSource = table;

            //sqlConnection.Close();
        }

        #endregion

        #region charts 

        public static void ChartsLoad(LiveCharts.WinForms.CartesianChart cartesianChart, string ProjectName, string title)
        {
            //sqlConnection.Open();

            dataAdapter = new SqlDataAdapter("SELECT * FROM [" + ProjectName + "]", sqlConnection);

            dataSet = new DataSet();

            cartesianChart.LegendLocation = LegendLocation.Bottom;

            if (dataSet.Tables[ProjectName] != null)
                dataSet.Tables[ProjectName].Clear();

            dataAdapter.Fill(dataSet, ProjectName);

            table = dataSet.Tables[ProjectName];

            SeriesCollection series = new SeriesCollection();

            ChartValues<double> voltage = new ChartValues<double>();

            List<string> dates = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                voltage.Add(Convert.ToDouble(row["voltageValues"]));

                dates.Add(Convert.ToInt32(row["timeValues"]).ToString());
            }

            cartesianChart.AxisX.Clear();

            cartesianChart.AxisX.Add(new Axis()
            {
                Title = "время",
                Labels = dates
            });

            LineSeries line = new LineSeries();
            line.Title = title;
            line.Values = voltage;

            series.Add(line);

            cartesianChart.Series = series;
        }

        #endregion

        #region CreateTableResult

        public static void CreateTableResult(string ProjectName, Dictionary<int, double> coordinats)
        {
            sqlConnection = new SqlConnection(connectionString);

            string query = string.Format("CREATE TABLE [{0}] ([timeValues] INT NULL,[voltageValues] NCHAR (50) NULL)", ProjectName);

            SqlCommand command = new SqlCommand(query, sqlConnection);

            sqlConnection.Open();

            command.ExecuteNonQuery();

            string queryFill = null;

            SqlCommand command2 = null;

            foreach (KeyValuePair<int, double> keyValue in coordinats)
            {
                queryFill = string.Format("INSERT INTO [{0}] (timeValues, voltageValues) VALUES ({1}, '{2}'); ", ProjectName, keyValue.Key, keyValue.Value.ToString());

                command2 = new SqlCommand(queryFill, sqlConnection);

                command2.ExecuteNonQuery();
            }

            sqlConnection.Close();
        }

        public static void DeleteTableRusult(string ProjectName)
        {
            string query = (string.Format("DROP TABLE [{0}]", ProjectName));

            SqlCommand command = new SqlCommand(query, sqlConnection);

            sqlConnection.Open();

            command.ExecuteNonQuery();

            sqlConnection.Close();
        }


        #endregion

        #region SelectTableDataItem

        public static string SelectTableDataItem(string ProjectName, string TableName, string colName)
        {
            sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();

            string query = string.Format("SELECT * FROM [{0}] WHERE ProjectName='{1}'", TableName, ProjectName);
 
            string item = null;

            SqlCommand command = new SqlCommand(query, sqlConnection);

            SqlDataReader sqlReader = command.ExecuteReader();

            try
            {
                sqlReader.Read();

                item = Convert.ToString(sqlReader[colName]);                        
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }

            sqlConnection.Close();

            return item;
        }

        #endregion

        #region ConstructorFill

        public static void ConstructorFillFromDB(string TableName, string ProjectName)
        {
            sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();

            string query = string.Format("SELECT * FROM [{0}] WHERE ProjectName='{1}'", TableName, ProjectName);

            SqlCommand command = new SqlCommand(query, sqlConnection);

            SqlDataReader sqlReader = command.ExecuteReader();

            try
            {
                sqlReader.Read();

                calculate = new CalculateEMS(
                        Convert.ToString(sqlReader["ProjectName"]), 
                        Convert.ToDouble(sqlReader["wireLength "]),
                        Convert.ToDouble(sqlReader["resistanceZ1"]),
                        Convert.ToDouble(sqlReader["resistanceZ2"]),
                        Convert.ToDouble(sqlReader["distanceBetweenWire"]),
                        Convert.ToDouble(sqlReader["radiusWire"]),
                        Convert.ToDouble(sqlReader["electricAntennaWire"]),
                        Convert.ToDouble(sqlReader["lengthAntennfWire"]),
                        Convert.ToDouble(sqlReader["dielectricConstant"]),
                        Convert.ToDouble(sqlReader["circularFrequency"]),
                        Convert.ToDouble(sqlReader["wavelengthOfField"]),
                        Convert.ToDouble(sqlReader["mediumPower"]),
                        Convert.ToDouble(sqlReader["сoeffAntenn"])
                    );
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }

            sqlConnection.Close();

        }

        #endregion

    }

}
