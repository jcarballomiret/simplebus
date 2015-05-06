//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SimpleBus.Helpers
//{
//    public class PerformanceManager
//    {
//        private const string CategoryName = "SimpleBus.Counters";
//        private const string CategoryHelp = "SimpleBus performance counters";
//        private const string SmsCounterName = "RateOfMessagesPerSecond";
//        private const string SmsCounterHelp = "Number of sms sent per second";

//        static PerformanceManager()
//        {
//            if (!PerformanceCounterCategory.Exists(CategoryName))
//            {
//                var averageCreationData = new CounterCreationData(SmsCounterName,
//                                                                  SmsCounterHelp,
//                                                                  PerformanceCounterType.RateOfCountsPerSecond32);
//                var sampleCreationData = new CounterCreationData(IvrCounterName,
//                                                                  IvrCounterHelp,
//                                                                  PerformanceCounterType.RateOfCountsPerSecond32);
//                var counterCreationDataCollection = new CounterCreationDataCollection { averageCreationData, sampleCreationData };
//                PerformanceCounterCategory.Create(CategoryName,
//                                                  CategoryHelp,
//                                                  PerformanceCounterCategoryType.SingleInstance,
//                                                  counterCreationDataCollection);
//            }

//            SmsCounter = new PerformanceCounter(CategoryName, SmsCounterName, false) { RawValue = 0 };
//        }

//        private static PerformanceCounter SmsCounter { get; set; }
//        private static PerformanceCounter IvrCounter { get; set; }

//        public static void IncrementSmsCounter()
//        {
//            SmsCounter.Increment();
//        }

//        public static void IncrementIvrCounter()
//        {
//            IvrCounter.Increment();
//        }
//    }
//}
