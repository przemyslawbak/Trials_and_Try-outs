#https://investpy.readthedocs.io/_api/news.html
import investpy

search_result = investpy.search_quotes(text='apple', products=['stocks'],countries=['united states'], n_results=1)

information = search_result.retrieve_information()
print(information)