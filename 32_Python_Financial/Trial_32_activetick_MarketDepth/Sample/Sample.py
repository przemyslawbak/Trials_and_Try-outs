#https://api.activetick.com/en/index.html?python#about-market-depth

import requests

url = "https://api.activetick.com/book_snapshot.json"
params = {
    "sessionid": "xxx",
    "symbol": "ES_230300_FCU",
    "source": "cme"
}

response = requests.get(url, params=params)

if response.status_code == 200:
    data = response.json()
    book = data["book"]
    for i in range(len(book)):
        entry = book[i]
        print(f"Side: {entry['x']} | Index: {entry['i']} | Flags: {entry['f']} | Attribution: {entry['a']} | Price: {entry['p']} | Quantity: {entry['q']} | Orders: {entry['o']} | Timestamp: {entry['tm']}")
else:
    print(f"Request failed with status code {response.status_code}")