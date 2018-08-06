//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Threading;
//using WinForms = System.Windows.Forms;
//using System.Runtime.InteropServices;
//using mshtml;
//using SHDocVw;
//using System.Windows.Automation;
//using QTF = Quintity.TestFramework.Core;

//namespace Quintity.TestFramework.Scratch.TestSuites
//{
//    [QTF.TestClass()]
//    [ComVisible(true)]
//    public class InternetExplorerTests : ScratchBase
//    {
//        [QTF.TestMethod]
//        public QTF.TestVerdict InternetExplorerTest()
//        {
//            try
//            {
//                Setup();
//                InternetExplorer ie = new InternetExplorer();
//                ie.Visible = false;
//                object mVal = System.Reflection.Missing.Value;
//                //gmailurl.Navigate2("http://www.gmail.com", ref mVal, ref mVal, ref mVal, ref mVal);

//                using (var metric = new QTF.TestMetric(this, "1234567", true, "Launching IE."))
//                {
//                    ie.Navigate2("https://www.google.com", ref mVal, ref mVal, ref mVal, ref mVal);
//                }

//                Thread.Sleep(2000);


//                HTMLDocument document = (mshtml.HTMLDocument)ie.Document;

//                IHTMLElementCollection objCollection = (mshtml.IHTMLElementCollection)document.getElementsByName("q");

//                using (var metric = new QTF.TestMetric(this, "7654321", true, "Search on cheese."))
//                {
//                    for (int i = 0; i < objCollection.length; i++)
//                    {
//                        mshtml.IHTMLElement objElement = (mshtml.IHTMLElement)objCollection.item(i, 0);
//                        objElement.innerText = "Cheese";
//                    }

//                    objCollection = (mshtml.IHTMLElementCollection)document.getElementsByName("btnG");

//                    for (int i = 0; i < objCollection.length; i++)
//                    {
//                        mshtml.IHTMLElement objElement = (mshtml.IHTMLElement)objCollection.item(i, 0);
//                        objElement.click();
//                    }
//                }


//                //IHTMLElement element = inputs[0]

//                //IHTMLElement2 pwInput = null;
//                //foreach (IHTMLElement input in inputs)
//                //{
//                //    int i = 1;
//                //    //if (input.getAttribute("type").ToString().ToLower() == "password")
//                //    //{
//                //    //    MessageBox.Show("SetFocusOnPasswordInput..");
//                //    //    pwInput = (IHTMLElement2)input;
//                //    //}
//                //}

//                // Find the text input element by its name
//                //mshtml.IHTMLDocument document = ie.Document;


//                //IWebElement query = driver.FindElement(By.Name("q"));

//                // Enter something to search for
//                // query.SendKeys("Cheese");

//                //while (ie.Busy)
//                //{
//                Thread.Sleep(10000);
//                //}


//                // https://armstest.cognosante.com/Arms

//                ie.Quit();

//                //mshtml.IHTMLDocument3 document = ie.Document;

//                //var element = (HTMLInputElement)document.all.item("username", 0);
//                // mshtml.IHTMLElement bob = (IHTMLElement)document.getElementById("SystemMaintenance");
//                //  bob.click();

//                //HTMLDocument myDoc = new HTMLDocumentClass();
//                //System.Threading.Thread.Sleep(500);
//                //myDoc = (HTMLDocument)gmailurl.Document;
//                //HTMLInputElement userID = (HTMLInputElement)myDoc.all.item("username", 0);
//                //userID.value = "youruserid";
//                //HTMLInputElement pwd = (HTMLInputElement)myDoc.all.item("pwd", 0);
//                //pwd.value = "yourpassword";
//                //HTMLInputElement btnsubmit = (HTMLInputElement)myDoc.all.item("signIn", 0);
//                //btnsubmit.click();
//                //gmailurl.Visible = true;


//                TestVerdict = QTF.TestVerdict.Pass;
//            }
//            catch (Exception e)
//            {
//                TestMessage += e.ToString();
//                TestVerdict = QTF.TestVerdict.Error;
//            }
//            finally
//            {
//                Teardown();
//            }

//            return TestVerdict;
//        }

//        [QTF.TestMethod]
//        public QTF.TestVerdict CERRSTest()
//        {
//            try
//            {
//                Setup();
//                InternetExplorer ie = new InternetExplorer();
//                ie.Visible = true;
//                object mVal = System.Reflection.Missing.Value;

//                using (var metric = new QTF.TestMetric(this, "1234567", true, "Launching IE."))
//                {
//                    int hwnd = ie.HWND;
//                    ie.Navigate2("http://52.0.14.216:5555/CERRSTEST1", ref mVal, ref mVal, ref mVal, ref mVal);

//                    Thread.Sleep(5000);

//                    //var element = this.getBrowserElement(ie.HWND);
//                    InternetExplorerTests.HandleAuthenticationDialogForIE(ie.HWND, "az\tst-1095analyst", "q@55w0RD$!");
//                }

//                Thread.Sleep(2000);

//                HTMLDocument document = (mshtml.HTMLDocument)ie.Document;

//                TestMessage = document.title;

//                //IHTMLElementCollection objCollection = (mshtml.IHTMLElementCollection)document.getElementsByName("q");

//                //using (var metric = new QTF.TestMetric(this, "7654321", true, "Search on cheese."))
//                //{
//                //    for (int i = 0; i < objCollection.length; i++)
//                //    {
//                //        mshtml.IHTMLElement objElement = (mshtml.IHTMLElement)objCollection.item(i, 0);
//                //        objElement.innerText = "Cheese";
//                //    }

//                //    objCollection = (mshtml.IHTMLElementCollection)document.getElementsByName("btnG");

//                //    for (int i = 0; i < objCollection.length; i++)
//                //    {
//                //        mshtml.IHTMLElement objElement = (mshtml.IHTMLElement)objCollection.item(i, 0);
//                //        objElement.click();
//                //    }
//                //}


//                //IHTMLElement element = inputs[0]

//                //IHTMLElement2 pwInput = null;
//                //foreach (IHTMLElement input in inputs)
//                //{
//                //    int i = 1;
//                //    //if (input.getAttribute("type").ToString().ToLower() == "password")
//                //    //{
//                //    //    MessageBox.Show("SetFocusOnPasswordInput..");
//                //    //    pwInput = (IHTMLElement2)input;
//                //    //}
//                //}

//                // Find the text input element by its name
//                //mshtml.IHTMLDocument document = ie.Document;


//                //IWebElement query = driver.FindElement(By.Name("q"));

//                // Enter something to search for
//                // query.SendKeys("Cheese");

//                //while (ie.Busy)
//                //{
//                //Thread.Sleep(10000);
//                //}


//                // https://armstest.cognosante.com/Arms

//                ie.Quit();

//                //mshtml.IHTMLDocument3 document = ie.Document;

//                //var element = (HTMLInputElement)document.all.item("username", 0);
//                // mshtml.IHTMLElement bob = (IHTMLElement)document.getElementById("SystemMaintenance");
//                //  bob.click();

//                //HTMLDocument myDoc = new HTMLDocumentClass();
//                //System.Threading.Thread.Sleep(500);
//                //myDoc = (HTMLDocument)gmailurl.Document;
//                //HTMLInputElement userID = (HTMLInputElement)myDoc.all.item("username", 0);
//                //userID.value = "youruserid";
//                //HTMLInputElement pwd = (HTMLInputElement)myDoc.all.item("pwd", 0);
//                //pwd.value = "yourpassword";
//                //HTMLInputElement btnsubmit = (HTMLInputElement)myDoc.all.item("signIn", 0);
//                //btnsubmit.click();
//                //gmailurl.Visible = true;


//                TestVerdict = QTF.TestVerdict.Pass;
//            }
//            catch (Exception e)
//            {
//                TestMessage += e.ToString();
//                TestVerdict = QTF.TestVerdict.Error;
//            }
//            finally
//            {
//                Teardown();
//            }

//            return TestVerdict;
//        }

//        private AutomationElement getBrowserElement(int hwnd)
//        {
//            int ct = 0;
//            AutomationElement dialogElement = null;

//            do
//            {
//                dialogElement = AutomationElement.RootElement.FindFirst(TreeScope.Descendants,
//                    new PropertyCondition(AutomationElement.NameProperty, "Windows Security"));

//                ++ct;
//                Thread.Sleep(1000);
//            } while (dialogElement == null && ct < 10);

//            AutomationElement OKBtnElement = null;
//            AutomationElement userNameElement = null;
//            //AutomationElement passwordElement = null;

//            if (dialogElement != null)
//            {
//                //dialogElement.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.)

//                OKBtnElement = dialogElement.FindFirst(TreeScope.Descendants,
//                   new PropertyCondition(AutomationElement.NameProperty, "Cancel"));


//                userNameElement = dialogElement.FindFirst(TreeScope.Descendants,
//                    new PropertyCondition(AutomationElement.ClassNameProperty, "Edit"));

//                //passwordElement = dialogElement.FindFirst(TreeScope.Descendants,
//                //new PropertyCondition(AutomationElement.NameProperty, "Cancel"));


//            }

//            GetInvokePattern(OKBtnElement).Invoke();

//            return OKBtnElement;
//        }
//        public InvokePattern GetInvokePattern(AutomationElement element)
//        {
//            return element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
//        }

//        WinForms.WebBrowser _webBrowser;
//        ManualResetEvent _resetEvent;

//        [QTF.TestMethod]
//        public QTF.TestVerdict InternetExplorerTest2()
//        {
//            try
//            {
//                Setup();
//                _resetEvent = new ManualResetEvent(false);

//                _webBrowser = new WinForms.WebBrowser();
//                _webBrowser.Url = new Uri("http://www.msn.com");

//                // Add an event handler that prints the document after it loads.
//                _webBrowser.Navigating += _webBrowser_Navigating;
//                _webBrowser.DocumentCompleted += _webBrowser_DocumentCompleted;
//                //new WebBrowserDocumentCompletedEventHandler(PrintDocument);

//                //_webBrowser.Url = new Uri("http://www.google.com");

//                _webBrowser.Navigate("https://armstest.cognosante.com", true);

//                _resetEvent.WaitOne();
//                ////_webBrowser.Visible = true;



//                //_webBrowser.Url = new Uri("http://www.google.com");
//                //_webBrowser.Url = new Uri(@"https://www.armstest.cognosante.com");
//                //_webBrowser.Navigate("https://www.armstest.cognosante.com");
//                // _webBrowser.Visible = true;

//                //Thread.Sleep(10000);

//                TestVerdict = QTF.TestVerdict.Pass;
//            }
//            catch (Exception e)
//            {
//                TestMessage += e.ToString();
//                TestVerdict = QTF.TestVerdict.Error;
//            }
//            finally
//            {
//                Teardown();
//            }

//            return TestVerdict;
//        }

//        void _webBrowser_Navigating(object sender, WinForms.WebBrowserNavigatingEventArgs e)
//        {
//        }

//        void _webBrowser_DocumentCompleted(object sender, WinForms.WebBrowserDocumentCompletedEventArgs e)
//        {
//            _resetEvent.Set();
//            //throw new NotImplementedException();
//        }

//        // Portions of code adapted from http://www.mathpirate.net/log/2009/09/27/swa-straight-outta-redmond/
//        public static void HandleAuthenticationDialogForIE(int hwnd, string userName, string password)
//        {
//            if (String.IsNullOrWhiteSpace(userName))
//            {
//                throw new ArgumentNullException(userName, "Must contain a value");
//            }

//            if (String.IsNullOrWhiteSpace(password))
//            {
//                throw new ArgumentNullException(password, "Must contain a value");
//            }

//            // Condition for finding all "pane" elements
//            Condition paneCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane);

//            // Conditions for finding windows with a class of type dialog that's labeled Windows Security
//            Condition windowsSecurityCondition = new AndCondition(
//                                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
//                                new PropertyCondition(AutomationElement.ClassNameProperty, "#32770"),
//                                new PropertyCondition(AutomationElement.NameProperty, "Windows Security"));

//            // Conditions for finding list elements with an AutomationId of "UserList"
//            Condition userListCondition = new AndCondition(
//                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.List),
//                            new PropertyCondition(AutomationElement.AutomationIdProperty, "UserList"));

//            // Conditions for finding the account listitem element
//            Condition userTileCondition = new AndCondition(
//                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem),
//                            new PropertyCondition(AutomationElement.ClassNameProperty, "CredProvUserTile"),
//                            new PropertyCondition(AutomationElement.NameProperty, "Use another account"));

//            // Conditions for finding the OK button
//            Condition submitButtonCondition = new AndCondition(
//                                new PropertyCondition(AutomationElement.IsEnabledProperty, true),
//                                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
//                                new PropertyCondition(AutomationElement.AutomationIdProperty, "SubmitButton"));

//            // Conditions for finding the edit controls
//            Condition editCondition = new AndCondition(
//                            new PropertyCondition(AutomationElement.IsEnabledProperty, true),
//                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

//            // Find correc browser by windows handle.
//            AutomationElement ie = AutomationElement.RootElement.FindFirst(TreeScope.Descendants,
//                new PropertyCondition(AutomationElement.NativeWindowHandleProperty, hwnd));

//            // Find all "pane" elements that are children of the desktop
//            AutomationElementCollection panes = ie.FindAll(TreeScope.Children, paneCondition);

//            // Iterate through the collection of "panes"
//            foreach (AutomationElement pane in panes)
//            {
//                // Check to see if the current pane is labeled as IE
//                if (pane.Current.Name.Contains("Internet Explorer"))
//                {
//                    // Ok, we found IE. Now find all children of the IE pane that meets the windowSecurityCondition defined above
//                    AutomationElement windowsSecurityDialog = pane.FindFirst(TreeScope.Children, windowsSecurityCondition);

//                    if (windowsSecurityDialog != null)
//                    {
//                        // Get all children of the UserTile that are edit controls
//                        AutomationElementCollection edits = windowsSecurityDialog.FindAll(TreeScope.Children, editCondition);
//                        edits = windowsSecurityDialog.FindAll(TreeScope.Descendants, editCondition);

//                        // Iterate thru the edit controls
//                        foreach (AutomationElement edit in edits)
//                        {
//                            if (edit.Current.Name == "User name")
//                            {
//                                // We found the username edit control. Let's set the contents of the box to the username.
//                                ValuePattern userNamePattern = (ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern);
//                                userNamePattern.SetValue(userName);
//                            }
//                            if (edit.Current.Name == "Password")
//                            {
//                                // We found the password edit control. Let's set the contents of the box to the password.
//                                ValuePattern userNamePattern = (ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern);
//                                userNamePattern.SetValue(password);
//                            }
//                        }

//                        // Find the first child of the security dialog that meets the submitButtonCondition defined above
//                        AutomationElement submitButton = windowsSecurityDialog.FindFirst(TreeScope.Children, submitButtonCondition);

//                        // Now press the button
//                        InvokePattern buttonPattern = (InvokePattern)submitButton.GetCurrentPattern(InvokePattern.Pattern);
//                        buttonPattern.Invoke();

//                        break;
//                    }
//                }
//            }
//        }
//    }
//}
