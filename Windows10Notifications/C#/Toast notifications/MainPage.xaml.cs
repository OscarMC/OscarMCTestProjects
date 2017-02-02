using NotificationsExtensions.Toasts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Toast_notifications
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        

        private void Show(ToastContent content)
        {
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
        }

        //private void ButtonCustomSnoozeTimes_Click(object sender, RoutedEventArgs e)
        //{
        //    Show(new ToastContent()
        //    {
        //        Scenario = ToastScenario.Reminder,

        //        Visual = new ToastVisual()
        //        {
        //            TitleText = new ToastText() { Text = "Daily Triage" },
        //            BodyTextLine1 = new ToastText() { Text = "10:00 AM - 10:30 AM" }
        //        },

        //        Launch = "392914",

        //        Actions = new ToastActionsCustom()
        //        {
        //            Inputs =
        //            {
        //                new ToastSelectionBox("snoozeAmount")
        //                {
        //                    Title = "Remind me...",
        //                    Items =
        //                    {
        //                        new ToastSelectionBoxItem("1", "Super soon (1 min)"),
        //                        new ToastSelectionBoxItem("5", "In a few mins"),
        //                        new ToastSelectionBoxItem("15", "When it starts"),
        //                        new ToastSelectionBoxItem("60", "After it's done")
        //                    },
        //                    DefaultSelectionBoxItemId = "1"
        //                }
        //            },

        //            Buttons =
        //            {
        //                new ToastButtonSnooze()
        //                {
        //                    SelectionBoxId = "snoozeAmount"
        //                },

        //                new ToastButtonDismiss()
        //            }
        //        }
        //    });
        //}

        //private void ButtonCustomSnoozeAndDismissText_Click(object sender, RoutedEventArgs e)
        //{
        //    Show(new ToastContent()
        //    {
        //        Visual = new ToastVisual()
        //        {
        //            TitleText = new ToastText() { Text = "Work" },
        //            BodyTextLine1 = new ToastText() { Text = "Wake up & go to work!!!" }
        //        },

        //        Launch = "394815",

        //        Scenario = ToastScenario.Alarm,

        //        Actions = new ToastActionsCustom()
        //        {
        //            Buttons =
        //            {
        //                new ToastButtonSnooze("5 more mins plz"),
        //                new ToastButtonDismiss("ok im awake")
        //            }
        //        }
        //    });
        //}

        //private void ButtonSystemSnoozeDismiss_Click(object sender, RoutedEventArgs e)
        //{
        //    Show(new ToastContent()
        //    {
        //        Visual = new ToastVisual()
        //        {
        //            TitleText = new ToastText() { Text = "Se ha recibido la anotación Registro General de Entradas 2016/234" },
        //            BodyTextLine1 = new ToastText() { Text = "Destinada al usuario 'OMAS'" }
        //        },
        //        Launch = "984910",
        //        Scenario = ToastScenario.Reminder,
        //        Actions = new ToastActionsSnoozeAndDismiss()
        //    });
        //}

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            // template to load for showing Toast Notification
            StringBuilder xmlToast = new StringBuilder();
            xmlToast.Append("<toast launch=\"abs-alerta\">");
            xmlToast.Append("<visual>");
            xmlToast.Append("<binding template =\"ToastGeneric\">");
            xmlToast.Append("<image placement=\"AppLogoOverride\" src=\"Assets/Samples/Toasts/logoAbsis.gif\"/>");
            xmlToast.Append("<text>Sistema de Alertas ABSIS</text>");
            xmlToast.Append("<text>");
            xmlToast.Append("Registro General de Entradas 2.016/345");
            xmlToast.Append("</text>");
            xmlToast.Append("</binding>");
            xmlToast.Append("</visual>");
            xmlToast.Append("<actions>");
            xmlToast.Append("<action content=\"check\" arguments=\"check\" imageUri=\"check.png\" />");
            xmlToast.Append("<action content=\"cancel\" arguments=\"cancel\" />");
            xmlToast.Append("</actions>");
            xmlToast.Append("<audio src=\"ms-winsoundevent:Notification.Reminder\"/>");
            xmlToast.Append("</toast>");

            var xmlDocument = new Windows.Data.Xml.Dom.XmlDocument();
            xmlDocument.LoadXml(xmlToast.ToString());
            
            var toastNotification = new ToastNotification(xmlDocument);
            var notification = ToastNotificationManager.CreateToastNotifier();
            notification.Show(toastNotification);
        }

        async void LaunchGoogle()
        {
            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(@"http://www.google.es"));

            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }
        }


    }
}
