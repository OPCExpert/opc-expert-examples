const SUBSCRIBE_URL =
    "http://192.168.1.101:80/subscribe?item=opcda%3A%2F%2Fdesktop-kn9ludo%2FICONICS.SimulatorOPCDA.2%2Fi%3ANumeric.Memory&rate=1000";
    
async function subscribe() {
    const response = await fetch(SUBSCRIBE_URL, {
        method: "GET"
    });

    const result = await response.json();

    if (!response.ok) {
        throw new Error(
            `Subscribe request failed: ${response.status} ${response.statusText}`
        );
    }

    return result;
}

async function main() {
    const result = await subscribe();

    console.log(JSON.stringify(result, null, 2));

    const subscriptionId = result?.data?.ID;

    if (subscriptionId) {
        console.log("\nSubscription ID:", subscriptionId);
    } else {
        console.log("\nNo subscription ID was returned.");
    }
}

main().catch(error => {
    console.error(error.message);
});
