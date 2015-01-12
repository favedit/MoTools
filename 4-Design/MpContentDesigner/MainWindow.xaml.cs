using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml;

namespace MpContentDesigner
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitListView();
        }
        //------------------------------------------------------------------------------
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            { imgWindowState.Source = new BitmapImage(new Uri("Resources\\Image\\WindowNormal.png", UriKind.Relative)); }
            else if (WindowState == WindowState.Normal)
            { imgWindowState.Source = new BitmapImage(new Uri("Resources\\Image\\WindowMaximize.png", UriKind.Relative)); }
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState != WindowState.Maximized) ? WindowState.Maximized : WindowState.Normal;
        }
        
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // 去掉textbox的键盘输入焦点
            if (Keyboard.FocusedElement != null)
            {
                DependencyObject deo = Keyboard.FocusedElement as DependencyObject;
                DependencyObject scope = FocusManager.GetFocusScope(deo);
                FocusManager.SetFocusedElement(scope, this);
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = (this.WindowState != WindowState.Maximized) ? WindowState.Maximized : WindowState.Normal;
            }
            else
            {
                this.DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        void InitListView()
        {
            Active(WorkMode.List);

            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://10.127.3.12:8040/content.data.frame.ws?action=list");
            //doc.Load(@"./test.xml");
            //doc.Save("./test.xml");

            XmlDataProvider provider = new XmlDataProvider();
            provider.Document = doc;
            provider.XPath = "/Service/Window";

            listXml.DataContext = provider;
            listXml.SetBinding(ListView.ItemsSourceProperty, new Binding());
        }

        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem lvi = sender as ListViewItem;
            if (lvi != null)
            {
                XmlElement xe = lvi.Content as XmlElement;
                if(xe != null)
                {
                    if(OpenFormDesigner(xe))
                    {
                        Active(WorkMode.Design);
                    }
                }
            }
        }


        bool OpenFormDesigner(XmlElement xe)
        {
            if(xe == null)
            {
                return false;
            }
            string header = xe.GetAttribute("label");
            if(string.IsNullOrEmpty(header))
            {
                return false;
            }

            CloseableTabItem closeableTabItem = FindOpennedFormDesigner(header);
            if (closeableTabItem == null)
            {
                closeableTabItem = new CloseableTabItem();
                closeableTabItem.Header = header;
                closeableTabItem.CloseTab += closeableTabItem_CloseTab;

                FormDesigner fd = new FormDesigner();
                closeableTabItem.Content = fd;

                tcFormDesignerContainer.Items.Add(closeableTabItem);
            }

            tcFormDesignerContainer.SelectedItem = closeableTabItem;

            return true;
        }

        CloseableTabItem FindOpennedFormDesigner(string header)
        {
            foreach (CloseableTabItem item in tcFormDesignerContainer.Items)
            {
                if(item.Header.Equals(header))
                {
                    return item;
                }
            }
            return null;
        }

        void closeableTabItem_CloseTab(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = e.Source as TabItem;
            if (tabItem != null)
            {
                TabControl tabControl = tabItem.Parent as TabControl;
                if (tabControl != null)
                    tabControl.Items.Remove(tabItem);
            }
        }

        private void btList_Click(object sender, RoutedEventArgs e)
        {
            Active(WorkMode.List);
        }
        private void btDesign_Click(object sender, RoutedEventArgs e)
        {
            Active(WorkMode.Design);
        }

        enum WorkMode
        {
            List,
            Design,
        }

        int i = 0;
        void Active(WorkMode wm)
        {
            if(wm == WorkMode.Design)
            {
                btList.Height = 22;
                btDesign.Height = 26;
                tcFormDesignerContainer.Visibility = System.Windows.Visibility.Visible;
                ucFormList.Visibility = System.Windows.Visibility.Collapsed;
                btDesign.Focus();
            }
            else if(wm == WorkMode.List)
            {
                btList.Height = 26;
                btDesign.Height = 22;
                tcFormDesignerContainer.Visibility = System.Windows.Visibility.Collapsed;
                ucFormList.Visibility = System.Windows.Visibility.Visible;
                btList.Focus();
            }
        }
    }
}
