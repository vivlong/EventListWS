using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;

namespace ConfigIIS
{
    public class FolderSecurityHelper
    {
        public static void SetFolderRights(string FolderPath)
        {
            var security = new DirectorySecurity();
            security.AddAccessRule(new FileSystemAccessRule("IIS_IUSRS", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            Directory.SetAccessControl(FolderPath, security);
        }
    }
}
