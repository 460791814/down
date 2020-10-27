﻿using System;
using System.IO;
using CefSharp;

namespace Common.CefProcessor
{
    /// <summary>
    /// MemoryStreamResponseFilter - copies all data from IResponseFilter.Filter
    /// to a MemoryStream. This is provided as an example to get you started and has not been
    /// production tested. If you experience problems you should refer to the CEF documentation
    /// and ask any questions you have on http://magpcss.org/ceforum/index.php
    /// Make sure to ask your question in the context of the CEF API (remember that CefSharp is just a wrapper).
    /// https://magpcss.org/ceforum/apidocs3/projects/(default)/CefResponseFilter.html#Filter(void*,size_t,size_t&,void*,size_t,size_t&)
    /// </summary>
    public class MemoryStreamResponseFilter : IResponseFilter
    {
        private MemoryStream _memoryStream;

        bool IResponseFilter.InitFilter()
        {
            _memoryStream = new MemoryStream();
            return true;
        }

        FilterStatus IResponseFilter.Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
        {
            if (dataIn == null)
            {
                dataInRead = 0;
                dataOutWritten = 0;

                return FilterStatus.Done;
            }

            //Calculate how much data we can read, in some instances dataIn.Length is
            //greater than dataOut.Length
            dataInRead = Math.Min(dataIn.Length, dataOut.Length);
            dataOutWritten = dataInRead;

            var readBytes = new byte[dataInRead];
            dataIn.Read(readBytes, 0, readBytes.Length);
            dataOut.Write(readBytes, 0, readBytes.Length);

            //Write buffer to the memory stream
            _memoryStream.Write(readBytes, 0, readBytes.Length);

            //If we read less than the total amount avaliable then we need
            //return FilterStatus.NeedMoreData so we can then write the rest
            if (dataInRead < dataIn.Length)
            {
                return FilterStatus.NeedMoreData;
            }

            return FilterStatus.Done;
        }

        void IDisposable.Dispose()
        {
            _memoryStream.Dispose();
            _memoryStream = null;
        }

        public byte[] Data => _memoryStream.ToArray();
    }
}