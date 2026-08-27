/*
 * Read one or more OPC items with the OPC Expert Web Server REST API.
 *
 * Official documentation:
 * https://opcexpert.com/opc-expert-web-server-api-documentation/
 *
 * Requirements: .NET 6 or later.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

internal class Program
{
    // OPC Expert Web Server endpoint.
    private const string BaseUrl = "http://localhost";

    // Replace these with browse paths or node IDs from your OPC server.
    // Add more entries to read multiple OPC items.
    private static readonly string[] ItemIds =
    {
        "ICONICS.SimulatorOPCDA.2->Numeric.Memory"
    };

    // Optional Read API parameters.
    private const bool ValuesOnly = false;
    private const uint UpdateRateMilliseconds = 1000;
    private const string PathSeparator = "->";
    private const int RequestTimeoutSeconds = 65;

    private static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(RequestTimeoutSeconds)
    };

    private static async Task<JsonDocument> ReadOpcItemsAsync(
        IEnumerable<string> itemIds,
        string baseUrl = BaseUrl,
        bool valuesOnly = ValuesOnly,
        uint rate = UpdateRateMilliseconds,
        string separator = PathSeparator)
    {
        string[] items = itemIds
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .ToArray();

        if (items.Length == 0)
        {
            throw new ArgumentException(
                "Provide at least one OPC item node ID or browse path.",
                nameof(itemIds));
        }

        var queryParameters = new List<string>();

        // Repeating the item parameter reads multiple OPC items.
        foreach (string item in items)
        {
            queryParameters.Add(
                $"item={Uri.EscapeDataString(item)}");
        }

        queryParameters.Add(
            $"values_only={valuesOnly.ToString().ToLowerInvariant()}");

        queryParameters.Add(
            $"rate={rate}");

        queryParameters.Add(
            $"separator={Uri.EscapeDataString(separator)}");

        string requestUrl =
            $"{baseUrl.TrimEnd('/')}/read?{string.Join("&", queryParameters)}";

        using HttpResponseMessage response =
            await HttpClient.GetAsync(requestUrl);

        string responseBody =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"OPC Expert Read API request failed " +
                $"({(int)response.StatusCode} {response.ReasonPhrase}): " +
                responseBody);
        }

        JsonDocument result;

        try
        {
            result = JsonDocument.Parse(responseBody);
        }
        catch (JsonException exception)
        {
            throw new JsonException(
                "OPC Expert returned a non-JSON response: " +
                responseBody,
                exception);
        }

        if (result.RootElement.TryGetProperty("meta", out JsonElement metadata) &&
            metadata.ValueKind == JsonValueKind.Object &&
            metadata.TryGetProperty(
                "ErrorMessage",
                out JsonElement errorElement) &&
            errorElement.ValueKind == JsonValueKind.String)
        {
            string? errorMessage = errorElement.GetString();

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                result.Dispose();

                throw new InvalidOperationException(
                    $"OPC Expert returned an error: {errorMessage}");
            }
        }

        return result;
    }

    private static async Task Main()
    {
        try
        {
            using JsonDocument result =
                await ReadOpcItemsAsync(ItemIds);

            string formattedJson = JsonSerializer.Serialize(
                result.RootElement,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            Console.WriteLine(formattedJson);
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(
                "Could not complete the OPC Expert Read API request: " +
                exception.Message);

            Environment.ExitCode = 1;
        }
    }
}
