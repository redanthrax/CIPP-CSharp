using CIPP.Shared.DTOs.Alerts;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CIPP.Api.Modules.Alerts.Services;

public class AlertRuleEvaluator {
    private readonly ILogger<AlertRuleEvaluator> _logger;

    public AlertRuleEvaluator(ILogger<AlertRuleEvaluator> logger) {
        _logger = logger;
    }

    public bool EvaluateConditions(List<AlertConditionDto> conditions, Dictionary<string, object> enrichedData) {
        if (!conditions.Any()) {
            return false;
        }

        foreach (var condition in conditions) {
            if (condition.Property == null || condition.Operator == null || condition.Input == null) {
                continue;
            }

            if (!EvaluateCondition(condition, enrichedData)) {
                return false;
            }
        }

        return true;
    }

    private bool EvaluateCondition(AlertConditionDto condition, Dictionary<string, object> data) {
        var propertyName = condition.Property!.Label;
        
        if (!data.TryGetValue(propertyName, out var value)) {
            return false;
        }

        var operatorValue = condition.Operator!.Value;
        var expectedValue = condition.Input!.Value;

        if (string.IsNullOrEmpty(expectedValue)) {
            return false;
        }

        var actualValue = value?.ToString() ?? string.Empty;

        return operatorValue switch {
            "eq" => StringEquals(actualValue, expectedValue),
            "ne" => !StringEquals(actualValue, expectedValue),
            "like" => StringContains(actualValue, expectedValue),
            "notlike" => !StringContains(actualValue, expectedValue),
            "notmatch" => !StringMatches(actualValue, expectedValue),
            "gt" => CompareNumeric(actualValue, expectedValue, (a, b) => a > b),
            "lt" => CompareNumeric(actualValue, expectedValue, (a, b) => a < b),
            "in" => IsInList(actualValue, expectedValue),
            "notIn" => !IsInList(actualValue, expectedValue),
            _ => false
        };
    }

    private static bool StringEquals(string actual, string expected) {
        return string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);
    }

    private static bool StringContains(string actual, string expected) {
        return actual.Contains(expected, StringComparison.OrdinalIgnoreCase);
    }

    private static bool StringMatches(string actual, string pattern) {
        try {
            return Regex.IsMatch(actual, pattern, RegexOptions.IgnoreCase);
        } catch {
            return false;
        }
    }

    private static bool CompareNumeric(string actual, string expected, Func<double, double, bool> comparison) {
        if (double.TryParse(actual, out var actualNum) && double.TryParse(expected, out var expectedNum)) {
            return comparison(actualNum, expectedNum);
        }
        return false;
    }

    private static bool IsInList(string actual, string expected) {
        var list = expected.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return list.Any(item => string.Equals(actual, item, StringComparison.OrdinalIgnoreCase));
    }
}
