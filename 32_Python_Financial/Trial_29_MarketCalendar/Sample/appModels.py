class TvSearchModel(object):
    def __init__(self, symbol, exchange, futures):
        self.__symbol = symbol
        self.__exchange = exchange
        self.__futures = futures

    @property
    def symbol(self):
        return self.__symbol

    @property
    def exchange(self):
        return self.__exchange

    @property
    def futures(self):
        return self.__futures