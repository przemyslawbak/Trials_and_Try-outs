#https://site.financialmodelingprep.com/developer/docs/economic-calendar-api#Python
#PREMIUM
#https://site.financialmodelingprep.com/developer/docs/pricing

from urllib.request import urlopen
import certifi
import json

def get_jsonparsed_data(url):
    """
    Receive the content of ``url``, parse it as JSON and return the object.

    Parameters
    ----------
    url : str

    Returns
    -------
    dict
    """
    response = urlopen(url, cafile=certifi.where())
    data = response.read().decode("utf-8")
    return json.loads(data)

url = ("https://financialmodelingprep.com/api/v3/economic_calendar?from=2020-08-05&to=2020-10-20&apikey=YOUR_API_KEY")
print(get_jsonparsed_data(url))