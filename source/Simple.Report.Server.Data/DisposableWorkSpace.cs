using System;
using System.IO;

namespace Simple.Report.Server.Data
{
    public class DisposableWorkSpace : IDisposable
    {
        public string TmpPath { get; private set; }

        public DisposableWorkSpace()
        {
            TmpPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(TmpPath);
        }

        public void Dispose()
        {
            if (!Directory.Exists(TmpPath)) return;

            Directory.Delete(TmpPath, true);
        }
    }
}