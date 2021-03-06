﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Consultations.Contract;
using RegistrationSystem.Contract;

namespace Simulator.Console
{
    public class Request
    {
        public string PatientName { get; set; }
        public string ConditionType { get; set; }
    }

    public class RequestsFileReader
    {
        public static void ProcessRequestsFile(string file, DateTime registrationDate, IConsultationBooker consultationBooker, PrintHelper resultHelper)
        {
            var requests = GetRequestsFromCsvFile(file);

            foreach (var entry in requests)
            {
                string patientName = entry.PatientName;
                if (patientName.Trim().Length == 0)
                {
                    System.Console.WriteLine(string.Format("An empty patient name was found in file {0}.",file));
                    continue;
                }
                string conditionAsString = entry.ConditionType;
                ConditionType condition;
                if (Enum.TryParse(conditionAsString, true, out condition))
                {
                    var request = new ConsultationRequest()
                    {
                        Condition = condition,
                        PatientName = patientName,
                        RegistrationDate = registrationDate
                    };

                    resultHelper.PrintRequest(request);

                    var result = consultationBooker.RequestFirstAvailableConsultation(request);

                    resultHelper.PrintRequestResult(result);
                }
                else
                {
                    System.Console.WriteLine("Can not parse condition type '{0}' in file {1}", conditionAsString, file);
                }
            }
        }

        private static ICollection<Request> GetRequestsFromCsvFile(string filePath)
        {
            var requests = new Collection<Request>();

            try
            {
                using (var reader = new StreamReader(File.OpenRead(filePath)))
                {
                    var listA = new List<string>();
                    var listB = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line == null)
                            continue;

                        var values = line.Split(',');

                        if (values.Count() == 2)
                        {
                            var request = new Request() { PatientName = values[0], ConditionType = values[1] };
                            requests.Add(request);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Could not process file '{0}'. {1}",filePath,e.Message);
            }

            return requests;

        }
    }
}
