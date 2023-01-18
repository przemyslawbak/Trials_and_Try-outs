import asyncio
import aiohttp

from pyonvista import PyOnVista

async def main():
    client = aiohttp.ClientSession()
    api = PyOnVista()
    await api.install_client(client)
    async with client:
        instruments = await api.search_instrument("VW")
        instrument = await api.request_instrument(instruments[0])
        quotes = await api.request_quotes(instrument, )
        print(instrument)
        print(quotes[:3])
        # prints a lot of information of VW Stonk
        # try a etf
        instruments = await api.search_instrument(key="IE00B42NKQ00")
        quotes = await api.request_quotes(instruments[0])
    print(quotes[0].instrument)

    await client.close()
    await asyncio.sleep(.1)

if __name__ == '__main__':
    asyncio.run(main())