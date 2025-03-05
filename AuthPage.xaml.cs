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

namespace Шарафутдинов41размер
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private string captchaText;
        private DateTime blockUntil = DateTime.MinValue;
        private int k = 0;
        public AuthPage()
        {
            InitializeComponent();
            captchaInput.Visibility = Visibility.Hidden;
        }

        private void GuestLogin_Click(object sender, RoutedEventArgs e)
        {
            User user = null;
            LoginBox.Text = string.Empty;
            PasswordBox.Text = string.Empty;
            Manager.MainFrame.Navigate(new ProductPage(user));
        }
        private void GenerateCaptcha()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            captchaText = new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            captchaOneWord.Text = captchaText[0].ToString();
            captchaTwoWord.Text = captchaText[1].ToString();
            captchaThreeWord.Text = captchaText[2].ToString();
            captchaFourWord.Text = captchaText[3].ToString();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PasswordBox.Text;
            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }
            if (k == 0)
            {
                User user = Шарафутдинов41размерEntities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
                if (user != null)
                {
                    captchaInput.Visibility = Visibility.Hidden;
                    captchaOneWord.Visibility = Visibility.Hidden;
                    captchaTwoWord.Visibility = Visibility.Hidden;
                    captchaThreeWord.Visibility = Visibility.Hidden;
                    captchaFourWord.Visibility = Visibility.Hidden;
                    k = 0;
                    Manager.MainFrame.Navigate(new ProductPage(user));
                    LoginBox.Text = "";
                    PasswordBox.Text = "";
                }
                else
                {
                    MessageBox.Show("Введены неверные данные");
                    captchaInput.Visibility = Visibility.Visible;
                    k++;
                    GenerateCaptcha();
                }
            }
            else
            {
                
                string userInput = captchaInput.Text.Trim().ToUpper();
                if (DateTime.Now < blockUntil)
                {
                    MessageBox.Show($"Попробуйте снова через {(blockUntil - DateTime.Now).Seconds} секунд.");
                    return;
                }
                if (userInput == captchaText)
                {
                    User user = Шарафутдинов41размерEntities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
                    if (user != null)
                    {
                        Manager.MainFrame.Navigate(new ProductPage(user));
                        LoginBox.Text = "";
                        PasswordBox.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Введены неверные данные");
                        k++;
                        GenerateCaptcha();
                        Login.IsEnabled = false;
                        await Task.Delay(10000);
                        Login.IsEnabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Неверная CAPTCHA. Попробуйте снова.");
                    Login.IsEnabled = false;
                    GenerateCaptcha();
                    await Task.Delay(10000);
                    Login.IsEnabled = true;
                }
            }
        }
    }
}
