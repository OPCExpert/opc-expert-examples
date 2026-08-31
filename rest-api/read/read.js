
// For Multiple Items:
const READ_URL =
    "http://192.168.1.101:80/read?item=opcda%3A%2F%2Fdesktop-kn9ludo%2FICONICS.SimulatorOPCDA.2%2Fi%3ANumeric.Memory";

const READ_URL_MULTIPLE = "http://192.168.1.101:80/read?item=opcda%3A%2F%2Fdesktop-kn9ludo%2FICONICS.SimulatorOPCDA.2%2Fi%3ANumeric.Memory&item=opcda%3A%2F%2Fdesktop-kn9ludo%2FICONICS.SimulatorOPCDA.2%2Fi%3ANumeric.Sine"   

async function readItems() {
    const response = await fetch(READ_URL, {
        method: "GET"
    });

    const result = await response.json();

    if (!response.ok) {
        throw new Error(
            `Read request failed: ${response.status} ${response.statusText}`
        );
    }

    if (result?.data?.ErrorCode) {
        throw new Error(
            `OPC Expert error ${result.data.ErrorCode}: ${result.data.Message}`
        );
    }

    return result;
}

async function main() {
    const result = await readItems();

    console.log(JSON.stringify(result, null, 2));
}

main().catch(error => {
    console.error(error.message);
});
