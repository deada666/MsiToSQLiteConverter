using MsiToSqLiteConverter.Schema;
using System;
using System.Collections.Generic;

namespace MsiToSqLiteConverter.Interaction
{
    public interface IDbWriter : IDisposable
    {
        void WriteDataToDb(IEnumerable<RowData> dataToWrite);
    }
}
