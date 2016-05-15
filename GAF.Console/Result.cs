using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GAF.ConsoleTest
{
    public class Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
    }

    public class Result
    {
        public Result(int run, int generation, long evaluation, double fitness)
        {
            Run = run;
            Generation = generation;
            Evaluation = evaluation;
            Fitness = fitness;
        }

        public int Run { get; private set; }
        public int Generation { get; private set; }
        public long Evaluation { get; private set; }
        public double Fitness { get; private set; }
    }

    public class Results
    {
        private readonly List<Result> _items = new List<Result>();
 
        public void Add(Result result)
        {
            _items.Add(result);
        }

        public List<Point> GetResultSummary()
        {
            var resultSummary = new List<Point>();

            if (_items != null || _items.Count > 0)
            {

                //get the number of runs (incl. rangge) and generations
                var runMin = _items.Min(r => r.Run);
                var runMax = _items.Max(r => r.Run);
                var runs = runMax - runMin + 1;

                var generations = _items.Count(r => r.Run == runMin);

                //build temporary matrix for holding the results
                var resultMatrix = new Result[runMax - runMin + 1, generations];

                //rattle through each run
                for (var run = 0; run < runs; run++)
                {
                    var runLoacal = run;

                    //get the list of generation results from the results
                    var q1 = from r in _items
                             where r.Run == runLoacal + runMin
                             select r;

                    var runResults = q1.ToList();

                    //validate the results
                    if (runResults.Count != generations)
                    {
                        throw new ApplicationException("The data is inconsistent");
                    }

                    //rattle through these to build the matrix
                    for (var generation = 0; generation < generations; generation++)
                    {
                        resultMatrix[run, generation] = runResults[generation];
                    }

                }

                //now we have a matrix of all generation results for each run
                //now we need to transform the matrix
                for (var generation = 0; generation < generations; generation++)
                {
                    var averageMaxFitness = 0.0;
                    var evaluation = resultMatrix[0, generation].Evaluation;

                    //calculate the average for each evaluation over each run
                    for (var run = 0; run < runs; run++)
                    {
                        var res = resultMatrix[run, generation];

                        averageMaxFitness += res.Fitness;
                    }

                    //build a list of plottable points
                    resultSummary.Add(new Point(evaluation, averageMaxFitness / runs));
                }
            }

            return resultSummary;
        }

        public void SaveResults(string filename, string summaryText)
        {
			if (!string.IsNullOrWhiteSpace (filename) && !string.IsNullOrWhiteSpace (summaryText)) {
				File.AppendAllText (filename,
					string.Format ("{0}, \n", summaryText));

				File.AppendAllText (filename, "Evaluation Number, Maximum Fitness, Consecutive 9s\n");

				var resultSummary = GetResultSummary ();

				foreach (var point in resultSummary) {

					File.AppendAllText (filename,
						string.Format ("{0},{1},{2}\n",
							point.X.ToString (CultureInfo.InvariantCulture),
							point.Y.ToString (CultureInfo.InvariantCulture),
							ConsecutiveNines (point.Y)));

				}

				Debug.WriteLine ("Results written to File.");
			}
        }

        private double ConsecutiveNines(double value)
        {
            double result = 0;
            string stringResult = value.ToString(CultureInfo.InvariantCulture);

            for (int index = 2; index < stringResult.Length; index++)
            {
                if (stringResult[index] == '9')
                {
                    result++;
                }
                else
                {
                    string sub = string.Format("{0}.{1}", result.ToString(CultureInfo.InvariantCulture),
                                               stringResult.Substring(index));
                    result = Convert.ToDouble(sub);
                    break;
                }
            }

            return result;
        }

    }

    //public class Results
    //{
    //    private List<EvaluationResults> _results = new List<EvaluationResults>();

    //    public List<EvaluationResults> Results { set; get; }

    //    /// <summary>
    //    /// Returns the minimum fitness value for the results.
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<double> MinimumFitnessForAllRuns
    //    {
    //        get { return null; }
    //        //_results.Min(c => c.MaximumFitness); }
    //    }

    //    /// <summary>
    //    /// Returns the average (mean) fitness value for the results.
    //    /// </summary>
    //    public List<double> AverageFitnessForAllRuns
    //    {
    //        get { return null; }
    //        //_results.Average(c => c.AverageFitness); }
    //    }
    //}
}
