using project_management.DAO;
using project_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace project_management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Console.WriteLine("Vindue åbnet ");

            UserDAO dao = new UserDAO();
          //    User user = new User(10, "Alen", "Hasanagic", "tim", "alenhasana@yahoo.dk", "test", 1, DateTime.Today, DateTime.Now);
           User user = new User(10, "Alennnnnn", "Hasanagic", "tim", "alenhasana@yahoo.dk", "test", 1, DateTime.Today, DateTime.Now);
            Console.WriteLine("bruger lavet ");

            //    dao.delete(10);
            //    dao.read(10);
            //  dao.create(user);
            dao.update(user);
                dao.read(10);
            Console.WriteLine("ruger oprettet ");

            //Kald metoden fra dao f.eks. new user etc
            InitializeComponent();
        }
    }
}
