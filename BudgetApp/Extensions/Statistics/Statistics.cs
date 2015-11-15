using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Extensions.Statistics
{
    public static class Statistics
    {
        public static double Variance(this IEnumerable<double> source)
        {
            int n = 0;
            double mean = 0;
            double M2 = 0;

            foreach (double x in source)
            {
                n = n + 1;
                double delta = x - mean;
                mean = mean + delta / n;
                M2 += delta * (x - mean);
            }
            return M2 / (n - 1);
        }

        public static double StandardDeviation(this IEnumerable<double> source)
        {
            return Math.Sqrt(source.Variance());
        }

        public static double Median(this IEnumerable<double> source)
        {
            if (!source.Any())
                return 0;

            var sortedList = from number in source
                             orderby number
                             select number;

            int count = sortedList.Count();
            int itemIndex = count / 2;
            if (count % 2 == 0) // Even number of items. 
                return (sortedList.ElementAt(itemIndex) +
                        sortedList.ElementAt(itemIndex - 1)) / 2;

            // Odd number of items. 
            return sortedList.ElementAt(itemIndex);
        }

        public static double Range(this IEnumerable<double> source)
        {
            return source.Max() - source.Min();
        }

        public static double Covariance(this IEnumerable<double> source, IEnumerable<double> other)
        {
            int len = source.Count();

            double avgSource = source.Average();
            double avgOther = other.Average();
            double covariance = 0;

            for (int i = 0; i < len; i++)
                covariance += (source.ElementAt(i) - avgSource) * (other.ElementAt(i) - avgOther);

            return covariance / len;
        }
    }
}