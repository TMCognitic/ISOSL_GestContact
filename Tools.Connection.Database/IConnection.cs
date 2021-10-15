using System;
using System.Collections.Generic;
using System.Data;

namespace Tools.Connection.Database
{
    public interface IConnection
    {
        int ExecuteNonQuery(Command command);
        IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector, bool executeImmediately = false);
        object ExecuteScalar(Command command);
    }
}