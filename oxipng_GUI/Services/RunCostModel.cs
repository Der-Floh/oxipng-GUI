using OxipngGui.Models;

namespace OxipngGui.Services;

/// <summary>
/// Learns a <see cref="RunCost"/> from how long finished runs took, by least squares. Each run's error is weighted by
/// one over its duration, so the few long runs at the start of a batch don't decide what the many short ones after
/// them cost. A fit with a negative cost is left out, and so is one that puts more on each run or file than starting
/// oxipng and handling a small file take: that fit has mistaken slow large files for fixed costs.
/// </summary>
public sealed class RunCostModel
{
    private const int FeatureCount = 3;
    private const int PerRun = 0;
    private const int PerFile = 1;
    private const int PerMegabyte = 2;
    private const double BytesPerMegabyte = 1024 * 1024;
    private const double MinWeightedSeconds = 0.05;
    private const double MinPivot = 1e-6;
    private const double RequiredImprovement = 0.999;
    private const double MaxSecondsPerRun = 0.75;
    private const double MaxSecondsPerFile = 0.5;
    private static readonly int[] Subsets =
    [
        1 << PerMegabyte,
        1 << PerRun | 1 << PerMegabyte,
        1 << PerFile | 1 << PerMegabyte,
        1 << PerRun | 1 << PerFile | 1 << PerMegabyte,
    ];

    private readonly double[,] _products = new double[FeatureCount, FeatureCount];
    private readonly double[] _productsWithSeconds = new double[FeatureCount];
    private double _secondsSquared;
    private int _runs;

    public void Add(int files, long bytes, TimeSpan duration)
    {
        double[] features = [1, files, bytes / BytesPerMegabyte];
        var seconds = duration.TotalSeconds;
        var weight = 1 / Math.Max(seconds, MinWeightedSeconds);
        for (var row = 0; row < FeatureCount; row++)
        {
            for (var column = 0; column < FeatureCount; column++)
                _products[row, column] += weight * features[row] * features[column];

            _productsWithSeconds[row] += weight * features[row] * seconds;
        }

        _secondsSquared += weight * seconds * seconds;
        _runs++;
    }

    /// <summary>
    /// Fits the cost per megabyte alone and together with the cost per run, per file or both, and keeps the fit that
    /// leaves the least unexplained variance per degree of freedom, so a cost the runs can't tell apart from another
    /// stays out. The cost per megabyte always stays in: the batch goes on to smaller files, and without it a tiny run
    /// would look as slow as a large one.
    /// </summary>
    /// <returns>The cost, or <see langword="null"/> while too few runs have finished.</returns>
    public RunCost? Fit()
    {
        double[]? best = null;
        var bestScore = double.PositiveInfinity;
        foreach (var subset in Subsets)
        {
            if (TryFit(subset, out var coefficients, out var score)
                && coefficients[PerRun] <= MaxSecondsPerRun
                && coefficients[PerFile] <= MaxSecondsPerFile
                && score < bestScore * RequiredImprovement)
            {
                best = coefficients;
                bestScore = score;
            }
        }

        return best is null ? null : new RunCost(best[PerRun], best[PerFile], best[PerMegabyte]);
    }

    /// <param name="subset">A bit per cost to fit; the others stay 0.</param>
    /// <param name="score">The unexplained variance per degree of freedom.</param>
    /// <returns>
    /// <see langword="false"/> if there are too few runs, the runs can't tell the costs apart, or one comes out negative.
    /// </returns>
    private bool TryFit(int subset, out double[] coefficients, out double score)
    {
        int[] features = [.. Enumerable.Range(0, FeatureCount).Where(feature => (subset & (1 << feature)) != 0)];
        coefficients = new double[FeatureCount];
        score = double.PositiveInfinity;

        var degreesOfFreedom = _runs - features.Length;
        if (degreesOfFreedom <= 0 || !TrySolve(features, out var solution) || solution.Any(value => value < 0))
            return false;

        var explained = 0.0;
        for (var i = 0; i < features.Length; i++)
        {
            coefficients[features[i]] = solution[i];
            explained += solution[i] * _productsWithSeconds[features[i]];
        }

        score = Math.Max(0, _secondsSquared - explained) / degreesOfFreedom;
        return true;
    }

    /// <summary>
    /// Solves the normal equations of <paramref name="features"/>. They're scaled to a unit diagonal first, so costs
    /// the runs can't tell apart show up as a tiny pivot.
    /// </summary>
    private bool TrySolve(int[] features, out double[] solution)
    {
        var size = features.Length;
        solution = new double[size];
        var scale = new double[size];
        for (var i = 0; i < size; i++)
        {
            var diagonal = _products[features[i], features[i]];
            if (diagonal <= 0)
                return false;

            scale[i] = 1 / Math.Sqrt(diagonal);
        }

        var matrix = new double[size, size + 1];
        for (var row = 0; row < size; row++)
        {
            for (var column = 0; column < size; column++)
                matrix[row, column] = _products[features[row], features[column]] * scale[row] * scale[column];

            matrix[row, size] = _productsWithSeconds[features[row]] * scale[row];
        }

        for (var pivot = 0; pivot < size; pivot++)
        {
            if (matrix[pivot, pivot] < MinPivot)
                return false;

            for (var row = pivot + 1; row < size; row++)
            {
                var factor = matrix[row, pivot] / matrix[pivot, pivot];
                for (var column = pivot; column <= size; column++)
                    matrix[row, column] -= factor * matrix[pivot, column];
            }
        }

        var scaled = new double[size];
        for (var row = size - 1; row >= 0; row--)
        {
            var value = matrix[row, size];
            for (var column = row + 1; column < size; column++)
                value -= matrix[row, column] * scaled[column];

            scaled[row] = value / matrix[row, row];
            solution[row] = scaled[row] * scale[row];
        }

        return true;
    }
}
