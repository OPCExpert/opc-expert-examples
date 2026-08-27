/**
 * Read one or more OPC items with the OPC Expert Web Server REST API.
 *
 * Official documentation:
 * https://opcexpert.com/opc-expert-web-server-api-documentation/
 *
 * Requirements: Node.js 18 or later. No external packages are required.
 */

// OPC Expert Web Server endpoint.
const BASE_URL = "http://localhost";

// Replace these with browse paths or node IDs from your OPC server.
// Add more entries to read multiple OPC items.
const ITEM_IDS = [
  "ICONICS.SimulatorOPCDA.2->Numeric.Memory",
];

// Optional Read API parameters.
const VALUES_ONLY = false;
const UPDATE_RATE_MS = 1000;
const PATH_SEPARATOR = "->";
const REQUEST_TIMEOUT_MS = 65_000;

async function readOpcItems(
  itemIds,
  {
    baseUrl = BASE_URL,
    valuesOnly = VALUES_ONLY,
    rate = UPDATE_RATE_MS,
    separator = PATH_SEPARATOR,
  } = {},
) {
  if (!Array.isArray(itemIds) || itemIds.length === 0) {
    throw new Error("Provide at least one OPC item node ID or browse path.");
  }

  const url = new URL("/read", `${baseUrl.replace(/\/$/, "")}/`);

  // Repeating the item parameter reads multiple OPC items.
  for (const itemId of itemIds) {
    url.searchParams.append("item", itemId);
  }

  url.searchParams.set("values_only", String(valuesOnly));
  url.searchParams.set("rate", String(rate));
  url.searchParams.set("separator", separator);

  const response = await fetch(url, {
    method: "GET",
    signal: AbortSignal.timeout(REQUEST_TIMEOUT_MS),
  });

  const responseText = await response.text();
  let result;

  try {
    result = JSON.parse(responseText);
  } catch {
    throw new Error(
      `OPC Expert returned a non-JSON response (${response.status}): ${responseText}`,
    );
  }

  if (!response.ok) {
    throw new Error(
      `OPC Expert Read API request failed (${response.status}): ${response.statusText}`,
    );
  }

  if (result?.meta?.ErrorMessage) {
    throw new Error(
      `OPC Expert returned an error: ${result.meta.ErrorMessage}`,
    );
  }

  return result;
}

async function main() {
  try {
    const result = await readOpcItems(ITEM_IDS);
    console.log(JSON.stringify(result, null, 2));
  } catch (error) {
    console.error(
      `Could not complete the OPC Expert Read API request: ${error.message}`,
    );
    process.exitCode = 1;
  }
}

if (require.main === module) {
  main();
}

module.exports = { readOpcItems };
