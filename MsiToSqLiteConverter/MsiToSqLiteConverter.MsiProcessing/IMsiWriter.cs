using MsiToSqLiteConverter.Schema;
using System;
using System.Collections.Generic;

namespace MsiToSqLiteConverter.MsiProcessing
{
    public interface IMsiWriter : IDisposable
    {
        void WriteDataToMsi(IEnumerable<RowData> dataToWrite);
    }
}
