using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace RegistrationSystem.UnitTests
{
    public class Request
    {
        public string PatientName { get; set; }
        public string ConditionType { get; set; }
    }

    public class RequestsFileReader
    {
        public static ICollection<Request> GetRequestsFromCsvFile(string filePath)
        {
            var requests = new Collection<Request>();

            var reader = new StreamReader(File.OpenRead(filePath));
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null)
                    continue;

                var values = line.Split(',');

                if (values.Count() == 2)
                {
                    var request = new Request() {PatientName = values[0], ConditionType = values[1]};
                    requests.Add(request);
                }
            }
            return requests;
        }
    }
}
