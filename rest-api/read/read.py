
"""Read one or more OPC items with the OPC Expert Web Server REST API.
... 
... Official documentation:
... https://opcexpert.com/opc-expert-web-server-api-documentation/
... 
... Install the dependency with:
...     python -m pip install requests
... """

import json
from typing import Any
 
import requests
 
 
# OPC Expert Web Server endpoint. The documented default is http://localhost.
BASE_URL = "http://localhost"
 
# Replace these example browse paths or node IDs with items from your OPC server.
# Add more strings to the list to read multiple OPC items in one request.
ITEM_IDS = ["ICONICS.SimulatorOPCDA.2->Numeric.Memory",]
 
 # Optional Read API parameters.
VALUES_ONLY = False
UPDATE_RATE_MS = 1000
PATH_SEPARATOR = "->"
REQUEST_TIMEOUT_SECONDS = 65
 
 
def read_opc_items(
    item_ids: list[str],
    *,
    base_url: str = BASE_URL,
    values_only: bool = VALUES_ONLY,
    rate: int = UPDATE_RATE_MS,
    separator: str = PATH_SEPARATOR,
) -> dict[str, Any]:
    """Return the JSON response from the OPC Expert Read API.

    Each item is sent as a separate ``item`` query parameter, as required when
    reading multiple OPC items with the OPC Expert Web Server.
    """
    if not item_ids:
        raise ValueError("Provide at least one OPC item node ID or browse path.")

    url = f"{base_url.rstrip('/')}/read"
    params: list[tuple[str, str | int]] = [
        ("item", item_id) for item_id in item_ids
    ]
    params.extend(
        [
            ("values_only", str(values_only).lower()),
            ("rate", rate),
            ("separator", separator),
        ]
    )

    response = requests.get(url, params=params, timeout=REQUEST_TIMEOUT_SECONDS)
    response.raise_for_status()

    result: dict[str, Any] = response.json()
    metadata = result.get("meta", {})
    error_message = metadata.get("ErrorMessage")

    if error_message:
        raise RuntimeError(f"OPC Expert returned an error: {error_message}")

    return result


if __name__ == "__main__":
    try:
        result = read_opc_items(ITEM_IDS)
        print(json.dumps(result, indent=2))
    except requests.RequestException as exc:
        print(f"Could not complete the OPC Expert Read API request: {exc}")
    except (RuntimeError, ValueError) as exc:
        print(f"Error: {exc}")
