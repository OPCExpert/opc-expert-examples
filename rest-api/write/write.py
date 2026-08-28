async function writeOpcItems(baseUrl, items) {
    const url = new URL("/write", baseUrl);

    for (const item of items) {
        url.searchParams.append("item", item.id);
        url.searchParams.append("value", String(item.value));
    }

    const response = await fetch(url, {
        method: "GET"
    });

    const result = await response.json();

    if (!response.ok) {
        throw new Error(
            `Write failed: ${response.status} ${response.statusText}`
        );
    }

    if (result.data?.ErrorCode) {
        throw new Error(
            `OPC Expert error ${result.data.ErrorCode}: ${result.data.Message}`
        );
    }

    return result;
}

async function main() {
    const items = [
        {
            id: "StressTest.Test01",
            value: 2
        }
    ];

    const result = await writeOpcItems(
        "http://localhost:80",
        items
    );

    console.log(JSON.stringify(result, null, 2));
}

main().catch(error => {
    console.error(error.message);
});
