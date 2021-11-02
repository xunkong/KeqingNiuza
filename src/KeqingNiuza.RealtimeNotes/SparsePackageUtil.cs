using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WF = Windows.Foundation;
using Windows.Management.Deployment;
using System.Runtime.InteropServices;

namespace KeqingNiuza.RealtimeNotes
{
    internal class SparsePackageUtil
    {
        internal static bool RegisterSparsePackage(string externalLocation, string sparsePkgPath, out string info)
        {
            info = null;
            bool registration = false;
            try
            {
                Uri externalUri = new Uri(externalLocation);
                Uri packageUri = new Uri(sparsePkgPath);

                Console.WriteLine("exe Location {0}", externalLocation);
                Console.WriteLine("msix Address {0}", sparsePkgPath);

                Console.WriteLine("  exe Uri {0}", externalUri);
                Console.WriteLine("  msix Uri {0}", packageUri);

                PackageManager packageManager = new PackageManager();

                //Declare use of an external location
                var options = new AddPackageOptions();
                options.ExternalLocationUri = externalUri;

                WF.IAsyncOperationWithProgress<DeploymentResult, DeploymentProgress> deploymentOperation = packageManager.AddPackageByUriAsync(packageUri, options);

                ManualResetEvent opCompletedEvent = new ManualResetEvent(false); // this event will be signaled when the deployment operation has completed.

                deploymentOperation.Completed = (depProgress, status) => { opCompletedEvent.Set(); };

                Console.WriteLine("Installing package {0}", sparsePkgPath);

                Debug.WriteLine("Waiting for package registration to complete...");

                opCompletedEvent.WaitOne();

                if (deploymentOperation.Status == WF.AsyncStatus.Error)
                {
                    DeploymentResult deploymentResult = deploymentOperation.GetResults();
                    Debug.WriteLine("Installation Error: {0}", deploymentOperation.ErrorCode);
                    Debug.WriteLine("Detailed Error Text: {0}", deploymentResult.ErrorText);
                    info = deploymentResult.ErrorText;

                }
                else if (deploymentOperation.Status == WF.AsyncStatus.Canceled)
                {
                    Debug.WriteLine("Package Registration Canceled");
                    info = "Package Registration Canceled";
                }
                else if (deploymentOperation.Status == WF.AsyncStatus.Completed)
                {
                    registration = true;
                    Debug.WriteLine("Package Registration succeeded!");
                    info = "Package Registration succeeded!";
                }
                else
                {
                    Debug.WriteLine("Installation status unknown");
                    info = "Installation status unknown";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("AddPackageSample failed, error message: {0}", ex.Message);
                Console.WriteLine("Full Stacktrace: {0}", ex.ToString());
                info = ex.Message;
                return registration;
            }
            return registration;
        }



        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetCurrentPackageFullName(ref int packageFullNameLength, ref StringBuilder packageFullName);

        internal static bool IsRunningWithIdentity(out string packageName)
        {
            if (isWindows7OrLower())
            {
                packageName = null;
                return false;
            }
            else
            {
                StringBuilder sb = new StringBuilder(1024);
                int length = 0;
                int result = GetCurrentPackageFullName(ref length, ref sb);
                packageName = sb.ToString();
                return result != 15700;
            }
        }

        private static bool isWindows7OrLower()
        {
            int versionMajor = Environment.OSVersion.Version.Major;
            int versionMinor = Environment.OSVersion.Version.Minor;
            double version = versionMajor + (double)versionMinor / 10;
            return version <= 6.1;
        }


        internal static string GetSafeAppxLocalFolder()
        {
            try
            {
                return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            }
            catch (Exception ioe)
            {

                Debug.WriteLine(ioe.Message);
            }

            return null;
        }


    }
}
