using MsiToSqLiteConverter.Schema;
using System;
using System.Collections.Generic;

namespace MsiToSqLiteConverter.MsiProcessing
{
    public interface IMsiParser : IDisposable
    {
        IEnumerable<TableSchema> GetTableSchemas();

        IEnumerable<RowData> GetDataFromTable(TableSchema schema);

        int GetDbEncoding();

        // TODO: Summary info, not critical at the moment
    }
}
