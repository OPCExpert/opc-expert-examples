import requests
import time
import json
from types import SimpleNamespace

#To fix error "ImportError: No module named requests"
#   1. Start Command Prompt (cmd.exe)
#   2. Enter "py -3 -m pip install requests"

#To fix error "ImportError: No module named json"
#   1. Start Command Prompt (cmd.exe)
#   2. Enter "py -3 -m pip install json"

value = 0;

while(True):
    url = "http://192.168.1.101:80/write?item=opcda://desktop-kn9ludo/ICONICS.SimulatorOPCDA.2/i:Numeric.Memory&value=(value)"
    url = url.replace("(value)", str(value))
    response = requests.get(url)
    
    #parse response string(JSON) into a JSON object
    json_object = json.loads(response.text, object_hook=lambda d: SimpleNamespace(**d))
    data = json_object.data

    for item in data:
        print(f"{item.ID} | Value: {item.Properties.Value} StatusCode: {item.Properties.StatusCode} Timestamp: {item.Properties.SourceTimestamp}")

    #toggle the value between 0 and 1
    value = 1 - value
    time.sleep(2)
