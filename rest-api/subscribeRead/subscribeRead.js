const SERVER_URL = "http://192.168.1.101:80";

const SUBSCRIBE_URL =
    SERVER_URL +
    "/subscribe?item=opcda%3A%2F%2Fdesktop-kn9ludo%2FICONICS.SimulatorOPCDA.2%2Fi%3ANumeric.Ramp&rate=1000";

async function getJson(url, operationName) {
    const response = await fetch(url, {
        method: "GET"
    });

    const result = await response.json();

    if (!response.ok) {
        throw new Error(
            `${operationName} failed: ${response.status} ${response.statusText}`
        );
    }

    if (result?.data?.ErrorCode) {
        throw new Error(
            `OPC Expert error ${result.data.ErrorCode}: ${result.data.Message}`
        );
    }

    return result;
}

async function subscribe() {
    return getJson(SUBSCRIBE_URL, "Subscribe request");
}

async function readSubscription(subscriptionId) {
    const readUrl =
        `${SERVER_URL}/read?subscription=${encodeURIComponent(subscriptionId)}`;

    console.log("\nRead URL:");
    console.log(readUrl);

    return getJson(readUrl, "Subscription read");
}

function isGoodItem(item) {
    const properties = item?.Properties;

    if (!properties) {
        return false;
    }

    const description = properties.StatusCodeDescription;

    if (typeof description === "string" && description.trim() !== "") {
        return description.toLowerCase().startsWith("good");
    }

    return properties.ResultID === 0;
}

function delay(milliseconds) {
    return new Promise(resolve => setTimeout(resolve, milliseconds));
}

async function main() {
    console.log("Creating subscription...");

    const subscribeResult = await subscribe();

    console.log("\nSubscribe response:");
    console.log(JSON.stringify(subscribeResult, null, 2));

    const subscription = subscribeResult?.data;
    const subscriptionId = subscription?.ID;

    if (
        typeof subscriptionId !== "string" ||
        subscriptionId.trim() === ""
    ) {
        throw new Error(
            "The subscribe response did not include a valid subscription ID."
        );
    }

    if (
        !Array.isArray(subscription.SubscribedItems) ||
        subscription.SubscribedItems.length === 0
    ) {
        throw new Error(
            "The subscribe response did not include any subscribed items."
        );
    }

    console.log("\nSubscription ID:", subscriptionId);

    // Wait for the subscription's 1,000 ms update rate.
    await delay(1000);

    console.log("\nReading with the subscription ID...");

    const readResult = await readSubscription(subscriptionId);

    console.log("\nRead response:");
    console.log(JSON.stringify(readResult, null, 2));

    if (!Array.isArray(readResult?.data)) {
        throw new Error(
            "The subscription read did not return an item array."
        );
    }

    if (readResult.data.length === 0) {
        throw new Error(
            "The subscription read did not return any items."
        );
    }

    const failedItems = readResult.data.filter(item => !isGoodItem(item));

    if (failedItems.length > 0) {
        console.log("\nRead completed, but one or more items were not Good:");
        console.log(JSON.stringify(failedItems, null, 2));
        return;
    }

    console.log("\nSubscription read completed successfully.");
}

main().catch(error => {
    console.error("\nError:", error.message);
});
