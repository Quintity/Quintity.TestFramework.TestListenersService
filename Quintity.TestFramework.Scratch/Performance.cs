using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Scratch
{
    [TestClass]
    public class Performance : TestClassBase
    {
        public Performance()
        {
            TestMetric.OnTestMetric += TestMetric_OnTestMetric;
        }

        void TestMetric_OnTestMetric(string virtualUser, TestMetricEventArgs args)
        {
            //TestTrace.Trace(args.ToString());
        }

        [TestMethod]
        public TestVerdict PerformanceTest()
        {
            try
            {
                Setup();

                this.PauseExecution();

                // #1 - Relying on sdet to start and stop timer.
                using (TestMetric testMetric = new TestMetric("797686977979", "Test of thread sleep"))
                {
                    testMetric.Start();
                    
                    Thread.Sleep(30000);

                    testMetric.Stop();  // This is technically not necessary as Dispose in this construct stops the running timer.

                    testMetric.StateArgs.Add("This is an important runtime comment.");
                }

                this.ContinueExecution();

                //// #2 - Calling with constructor automatically starting timer.
                //using (TestMetric testMetric = new TestMetric(this, true, "Test of thread sleep"))
                //{
                //    // Do whatever is being performance tested (e.g., rendering a window).
                //    Thread.Sleep(5000);
                //}

                //// #3 - Another way //
                //TestMetric testMetric2 = new TestMetric(this, "Test of another thread sleep.");

                //testMetric2.Start();

                //Thread.Sleep(5000);

                //testMetric2.Stop(); // This is actually not necessary if Dispose is called (it stops a running timer).

                //testMetric2.Dispose();

                TestVerdict = TestVerdict.Pass;
                TestMessage = "This is the initial test message.";
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }
            finally
            {
                Teardown();
            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict WritePerformanceConfig(string filePath)
        {
            try
            {
                Setup();

                var testProfile = new TestProfile(2, new TimeSpan(0, 0, 0, 30, 300), "TestAnalyst");
                TestProfile.WritetoFile(testProfile, filePath);

                testProfile = TestProfile.ReadFromFile(filePath);

                TestVerdict = TestVerdict.Pass;
                TestMessage = testProfile.ToString();
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }
            finally
            {
                Teardown();
            }

            return TestVerdict;
        }
    }
}
