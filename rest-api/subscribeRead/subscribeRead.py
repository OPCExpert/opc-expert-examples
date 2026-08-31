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

while(True):
    response = requests.get("http://desktop-kn9ludo/subscribe?item=opcda://desktop-kn9ludo/ICONICS.SimulatorOPCDA.2/i:Numeric.Ramp")

    #parse response string(JSON) into a JSON object
    json_object = json.loads(response.text, object_hook=lambda d: SimpleNamespace(**d))
    data = json_object.data

    #each item contains the properties: ID, Value, Quality, SourceTimestamp, ServerTimestamp
    
    #print(data)
    subID = data.ID
    print(subID)
    response2 = requests.get("http://desktop-kn9ludo/read?subscription=" + subID)
    
    json_object2 = json.loads(response2.text, object_hook=lambda d: SimpleNamespace(**d))
    data2 = json_object2.data

    print(data2)
    break;

    #sleep for 1 second
    time.sleep(1)
