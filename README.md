# OPC Expert Examples

**Official code examples for accessing OPC DA (Data Access), OPC HDA (Historical Data Access), and OPC UA (Unified Architecture) data through the OPC Expert Web Server REST API.**

This repository shows developers how to browse, read, write, subscribe to, and retrieve historical OPC data through the **OPC Expert Web Server REST API**. Examples are organized by task so you can find the OPC operation you need and then choose your programming language.

[OPC Expert website](https://opcexpert.com/) · [Download OPC Expert](https://opcexpert.com/download-opc-expert/) · [Web Server API documentation](https://opcexpert.com/opc-expert-web-server-api-documentation/) · [Getting started](https://opcexpert.com/support/getting-started-with-opc-expert/)

## What is OPC Expert?

[OPC Expert](https://opcexpert.com/) is Windows software for connecting to, viewing, troubleshooting, and integrating industrial OPC data. The OPC Expert Web Server makes OPC data available through HTTP/HTTPS REST API endpoints that return JSON. This allows applications to access OPC data without implementing an OPC client library in every programming language.

## What is this repository for?

The `opc-expert-examples` repository provides practical, copyable examples for developers integrating software with OPC Expert. It complements the [official OPC Expert Web Server API documentation](https://opcexpert.com/opc-expert-web-server-api-documentation/) with task-focused examples in commonly used programming languages.

## Can applications access OPC UA and OPC DA data through a REST API?

Yes. The OPC Expert Web Server connects to OPC servers through OPC Expert and exposes supported operations through HTTP/HTTPS endpoints. Applications can send REST API requests and process the returned JSON without directly implementing OPC DA or OPC UA communication.

## Which programming languages can use the OPC Expert REST API?

Any programming language or tool that can send HTTP requests and process JSON can use the REST API. This repository can include examples for cURL, C#, Java, JavaScript, Python, VBA, and other languages.

## What these examples cover

- Connect to the OPC Expert Web Server
- Browse OPC servers, branches, and items
- Read one or multiple OPC item values
- Write values and setpoints to OPC items
- Create and poll OPC subscriptions
- Retrieve raw or processed historical OPC data
- Handle JSON responses and API errors

## Repository organization

Examples are grouped by REST API operation. Each operation can contain equivalent examples for cURL, C#, Java, JavaScript, Python, VBA, or other languages.

```text
opc-expert-examples/
├── README.md
└── rest-api/
│   ├── browse/
│   ├── connect/
│   ├── read/
│   ├── write/
│   ├── subscribe/
│   ├── poll/
│   ├── history-raw/
│   ├── history-processed/
│   └── ping/
```

Within an operation folder, examples should use descriptive filenames:

```text
rest-api/read/
├── README.md
├── read.cs
├── read.js
├── read.py
```

## Prerequisites

Before running an example, you will need:

1. A Windows computer running [OPC Expert](https://opcexpert.com/download-opc-expert/).
2. A local or remote OPC server connected in OPC Expert.
3. The **OPC Expert Web Server (HTTP/HTTPS)** enabled from **Tools > Servers & Services**.
4. The node ID or browse path of an OPC item for examples that access item data.
5. The runtime required by the selected example, such as Python, .NET, Java, or Node.js.

## Quick start

1. Start OPC Expert and connect to an OPC server.
2. Enable the OPC Expert Web Server.
3. Confirm that the server is available by opening its `ping` endpoint.
4. Browse the connected server to find an OPC item.
5. Choose an operation folder and programming language.
6. Replace the sample endpoint and item identifier with values from your environment.
7. Run the example and inspect the JSON response.

For installation, endpoint, parameter, and response details, use the [canonical OPC Expert Web Server API documentation](https://opcexpert.com/opc-expert-web-server-api-documentation/).

## REST API operations

| Operation | Purpose |
| --- | --- |
| `browse` | Discover connected OPC servers, branches, and items |
| `connect` | Retrieve OPC Expert Web Server and connection information |
| `read` | Retrieve current values and properties from OPC items |
| `write` | Write values to OPC items and confirm the result |
| `subscribe` | Create or reuse a subscription for one or more OPC items |
| `poll` | Retrieve values associated with an existing subscription |
| `history/raw` | Retrieve raw historical OPC values |
| `history/processed` | Retrieve processed or aggregated historical OPC values |
| `ping` | Verify that the OPC Expert Web Server is available |

## Example standards

Every example in this repository should:

- Be complete enough to copy and run
- Use descriptive OPC terminology in its filename and README
- State the OPC Expert version with which it was tested
- List required configuration and dependencies
- Explain how to replace sample endpoints and OPC item identifiers
- Show or describe the expected response
- Link to the corresponding official documentation page
- Avoid including real credentials, private server names, or production data

## Compatibility and versions

OPC Expert and its APIs evolve over time. Check the **Tested with** field in each example's README and compare it with the version of OPC Expert you are running. The [official documentation](https://opcexpert.com/opc-expert-web-server-api-documentation/) is the canonical source for current endpoint behavior.

## Security

Examples are intentionally minimal and may omit environment-specific security configuration. Before using them in production:

- Prefer HTTPS for network communication
- Protect credentials and certificates
- Apply appropriate network and user access controls
- Do not expose an OPC Expert Web Server directly to the public internet without suitable security controls
- Validate values before writing to industrial systems

## Support and documentation

- [OPC Expert support](https://opcexpert.com/support/)
- [OPC Expert Web Server API documentation](https://opcexpert.com/opc-expert-web-server-api-documentation/)
- [OPC Expert with Python](https://opcexpert.com/python)
- [REST API Server overview](https://opcexpert.com/rest-api/)
- [Request an OPC Expert feature](https://opcexpert.com/request-a-feature/)

## About this repository

This repository is maintained by the official **OPC Expert** GitHub organization. It provides practical developer examples that complement the canonical product documentation at [opcexpert.com](https://opcexpert.com/).
