using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zachet2
{
    /// <summary>
    /// Логика взаимодействия для AddZap.xaml
    /// </summary>
    public partial class AddZap : Page
    {
        private Service addzap;
        ClientService clientserv = new ClientService();
        bool flag;
        public AddZap(Service adzap)
        {
            InitializeComponent();
            addzap = adzap;
            TBServiceName.Text = addzap.Title;
            string fio = "";
            List<Client> C = BaseClass.Base.Client.ToList();

            for (int i = 0; i < C.Count; i++)
            {
                fio = C[i].LastName + " " + C[i].FirstName + " " + C[i].Patronymic;
                CBClients.Items.Add(fio);
            }
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int index = addzap.ID;
            List<Service> DT = BaseClass.Base.Service.Where(x => x.ID == index).ToList();
            int minutes = 0;
            foreach (Service s in DT)
            {
                minutes = s.DurationInSeconds / 60;
            }
            tb.Text = "Продолжительность курса - " + minutes + " минут";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.FrameMain.Navigate(new SerList());
        }

        private void AddReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int starttime=0;
                int clientid = CBClients.SelectedIndex + 1;
                int serviceid = addzap.ID;

                Regex r = new Regex("^[0-1]{1}[0-9]{1}:[0-5]{1}[0-9]{1}$");
                if (r.IsMatch(TBStartTime.Text) == false) 
                {
                    starttime = 0;
                }
                else { starttime += 1; }
                r = new Regex("^[2]{1}[0-3]{1}:[0-5]{1}[0-9]{1}$");
                if (r.IsMatch(TBStartTime.Text) == false) 
                {
                    starttime = 0;
                }
                else { starttime += 1; }
                if (starttime == 0)
                {
                    MessageBox.Show("Некорректное время", "Запись клиента");
                }
                if (starttime >= 1)
                {
                    DateTime dt = Convert.ToDateTime(DPDate.SelectedDate);                    
                    dt = dt.Add(TimeSpan.Parse(TBStartTime.Text));
                    if (dt < DateTime.Now)
                    {
                        MessageBox.Show("Некорректная дата!", "Запись клиента");
                    }
                    else
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        clientserv.ClientID = clientid; 
                        clientserv.ServiceID = serviceid; 
                        clientserv.StartTime = dt;
                        BaseClass.Base.ClientService.Add(clientserv);
                        BaseClass.Base.SaveChanges();
                        MessageBox.Show("Данные записаны!", "Запись клиента");
                        FrameClass.FrameMain.Navigate(new SerList());
                    }
                }
            }
            catch
            {
                MessageBox.Show("Запись не добавлена!", "Запись клиента");
            }
        }
    }
}
