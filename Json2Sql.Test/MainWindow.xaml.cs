using System;
using System.Windows;
using System.Windows.Controls;

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
            try
            {
                var btn = sender as Button;
                var jsonEntity = new JsonEntityToSql(ConnectionText.Text);
                switch (btn.Name)
                {
                    case "buttonInsert":
                        SqlText.Text = jsonEntity.Json2InsertSql(JsonText.Text);
                        break;
                    case "buttonDelete":
                        SqlText.Text = jsonEntity.Json2DeleteSql(JsonText.Text);
                        break;
                    case "buttonUpdate":
                        SqlText.Text = jsonEntity.Json2UpdateSql(JsonText.Text);
                        break;
                    case "buttonSelect":
                        //server=127.0.0.1;user id=root;password=123456abc;persistsecurityinfo=True;database=one_note
                        SqlText.Text = jsonEntity.Json2SelectSql(JsonText.Text);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SqlText.Text = ex.ToString();
            }


        }
    }
}
