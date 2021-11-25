using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainLibrary.Classes
{
    public class ResultContract : ITransferContract<byte[]>
    {

        private string requestTemplate = "RESULT name";
        private Regex requestRegex = new(@"RESULT (\w+)");
        public bool IsRequest(string request, out string[] args)
        {
            if (requestRegex.IsMatch(request))
            {
                Match match = requestRegex.Match(request);
                List<string> groups = new();
                for (int i = 1; i < match.Groups.Count; i++)
                    groups.Add(match.Groups[i].Value);
                args = groups.ToArray();
                return true;
            }
            else
            {
                args = Array.Empty<string>();
                return false;
            }
        }

        public Task<byte[]> ReceiveData(Stream stream, string[] args)
        {
            throw new NotImplementedException();
        }

        public Task SendData(Stream stream, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
