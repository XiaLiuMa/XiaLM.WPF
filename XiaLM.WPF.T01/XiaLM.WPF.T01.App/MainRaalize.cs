using System.Data.Entity;
using XiaLM.WPF.T01.App.Windows;
using XiaLM.WPF.T01.Db;
using XiaLM.WPF.T01.Db.Manaments;

namespace XiaLM.WPF.T01.App
{
    public class MainRaalize
    {
        private static MainRaalize instance;
        private readonly static object objLock = new object();
        public static MainRaalize GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new MainRaalize();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 数据库初始化
        /// </summary>
        public void DbInit()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BaseDBContext>());
            using (BaseDBContext dbContext = new BaseDBContext())
            {
                dbContext.Database.CreateIfNotExists();
            }
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        public void UiInit()
        {
            bool flag = UserManament.GetInstance().AutoLogin(); //自动登陆
            if (flag)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
            }
        }


    }

}
