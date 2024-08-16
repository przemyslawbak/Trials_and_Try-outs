#https://databento.com/docs/quickstart/build-first-app?historical=python&live=python&reference=python

import databento as db

client = db.Live(key="db-xxx")

client.subscribe(
    dataset="XNAS.ITCH",
    schema="trades",
    stype_in="parent",
    symbols="AAPL",
)

client.add_callback(print)

client.start()

client.block_for_close(timeout=10)