using System;
using System.Windows;
using System.Windows.Controls;
using Json2Sql.ConditionModel;
using Json2Sql.ModelEx;

namespace Json2Sql.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn.Name == "buttonInsert")
            {
                try
                {
                    var jsonEntity = JsonText.Text.ToJsonDictionary();
                    var sqlString = new SqlString(jsonEntity);
                    var str = sqlString.GetInsertSqls();

                    SqlText.Text = str;
                }
                catch (Exception ex)
                {
                    SqlText.Text = ex.ToString();
                }
            }

            if (btn.Name == "buttonDelete")
            {
                try
                {
                    var jsonEntity = JsonText.Text.ToJsonDictionary();
                    var sqlString = new SqlString(jsonEntity);
                    var str = sqlString.GetDeleteSqls();

                    SqlText.Text = str;
                }
                catch (Exception ex)
                {
                    SqlText.Text = ex.ToString();
                }
            }

            if (btn.Name == "buttonUpdate")
            {
                try
                {
                    var jsonEntity = JsonText.Text.ToJsonDictionary();
                    var sqlString = new SqlString(jsonEntity);
                    var str = sqlString.GetUpdateSqls();

                    SqlText.Text = str;
                }
                catch (Exception ex)
                {
                    SqlText.Text = ex.ToString();
                }
            }

            if (btn.Name == "buttonSelect")
            {
                try
                {
                    var jsonEntity = JsonText.Text.ToJsonObject();
                    var sqlString = new SqlString(jsonEntity);
                    //  sqlString.MySqlConnection = new MySql.Data.MySqlClient.MySqlConnection("server=127.0.0.1;user id=root;password=123456abc;persistsecurityinfo=True;database=one_note");
                    sqlString.MySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionText.Text);
                    var str = sqlString.GetQuerySql();
                    SqlText.Text = str;
                }
                catch (Exception ex)
                {
                    SqlText.Text = ex.ToString();
                }

            }

        }
    }
}
