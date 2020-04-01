using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;



namespace dbLabs.MacOS {
        [Register ("AppDelegate")]
        public class AppDelegate : FormsApplicationDelegate {
                NSWindow _window;

                public AppDelegate ()
                {
                        var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

                        var rect = new CoreGraphics.CGRect (500, 1000, 1000, 1000);
                        _window = new NSWindow (rect, style, NSBackingStore.Buffered, false);
                        _window.Title = "Xamarin.Forms on Mac!";
                        _window.TitleVisibility = NSWindowTitleVisibility.Hidden;
                }

                public override NSWindow MainWindow {
                        get { return _window; }
                }

                public override void DidFinishLaunching (NSNotification notification)
                {
                        //Forms.Init();
                        Forms.Init ();
                        Syncfusion.SfDataGrid.XForms.MacOS.SfDataGridRenderer.Init ();

						LoadApplication (new App ());
                        base.DidFinishLaunching (notification);
                }

                public override void WillTerminate (NSNotification notification)
                {
                        // Insert code here to tear down your application
                }
        }
}
